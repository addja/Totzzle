using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GOD
{
    public class GridMgr : MonoBehaviour
    {
        public static GridMgr Instance
        {
            get { return s_Instance; }
        }

        protected static GridMgr s_Instance;

        // I think we can unify these three into a single enum
        protected bool m_InPause = false;
        protected bool m_InPausingProcess = false;
        protected bool m_QueueOpened = false;

        public string PauseSceneName = "";

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one GridMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one GridMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }

        private Dictionary<string, Tile> m_tileMap = new Dictionary<string, Tile>();
        private TriggerTile m_triggerTile;

        private string TileIdentifier(float x, float y)
        {
            return x.ToString() + "/" + y.ToString();
        }

        private void Start()
        {
            foreach (Tile tile in GetComponentsInChildren<Tile>())
            {
                Vector3 tilePosition = tile.transform.position;
                m_tileMap[TileIdentifier(tilePosition.x, tilePosition.y)] = tile;
                if (tile.m_type == Tile.TileType.trigger) {
                    Assert.IsFalse(m_triggerTile); // We only want a triggerTiler per grid
                    m_triggerTile = (TriggerTile)tile;
                }
            }

            Assert.IsTrue(m_triggerTile); // We need one triggerTile per grid
        }

        public void QueueEditorClose()
        {
            m_QueueOpened = false;
            PlayerMgr.Instance.EnablePlayer();
            QueuePanelMgr.Instance.DisableQueue();
        }

        public void QueueEditorOpen()
        {
            m_QueueOpened = true;
            PlayerMgr.Instance.DisablePlayer();
            QueuePanelMgr.Instance.EnableQueue();
        }

        public void QueueLoaded()
        {
            m_triggerTile.EnableTrigger();
        }

        public bool CanMove(float x, float y)
        {
            Tile tile;
            m_tileMap.TryGetValue(TileIdentifier(x, y), out tile);
            return tile != null;
        }

        public void UpdateWorld()
        {
            foreach (Tile tile in m_tileMap.Values)
            {
                if (tile != null)
                {
                    tile.UpdateTile();
                }
            }
        }

        public void StartCountdown()
        {
            foreach (Tile tile in m_tileMap.Values)
            {
                if (tile != null)
                {
                    tile.StartCountdown();
                }
            }
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (GridInput.Instance.Pause.Down)
            {
                if (!m_InPause)
                {
                    Pause();
                }
                else
                {
                    Unpause();
                }
            }
            // else if (GridInput.Instance.QueueEditor.Down && !m_InPause) // Guille this is buggy as fuck
            else if (Input.GetKeyDown(KeyCode.Tab) && !m_InPause)
            {
                if (m_QueueOpened)
                {
                    QueueEditorClose();
                }
                else
                {
                    QueueEditorOpen();
                }
            }
        }

        public void Pause()
        {
            if (!m_InPause)
            {
                GridInput.Instance.ReleaseControl(false);
                GridInput.Instance.Pause.GainControl();
                m_InPause = true;

                // Hack for the input from the 2D Game Kit tutorial:
                //  If the time scale is not zeroed here, the input component will register
                //  twice the key downs.
                Time.timeScale = 0;

                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(PauseSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

                // stop input processing from Player an Queue
                PlayerMgr.Instance.DisableInput();
                QueuePanelMgr.Instance.DisableInput();
            }
        }

        public void Unpause()
        {
            //if the timescale is already > 0, we 
            if (Time.timeScale > 0)
                return;

            if (m_InPause)
            {
                StartCoroutine(UnpauseCoroutine());
            }
        }

        protected IEnumerator UnpauseCoroutine()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(PauseSceneName);
            GridInput.Instance.GainControl();
            //we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
            //of this script happen BEFORE the input is updated, leading to setting the game in pause once again
            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            m_InPause = false;

            // resume input processing from Player an Queue
            PlayerMgr.Instance.EnableInput();
            QueuePanelMgr.Instance.EnableInput();
        }
    }

}