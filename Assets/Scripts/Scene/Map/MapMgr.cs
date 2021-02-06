using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class MapMgr : MonoBehaviour
    {
        public static MapMgr Instance
        {
            get { return s_Instance; }
        }

        protected static MapMgr s_Instance;
        protected bool m_InPause = false;
        protected bool m_InPausingProcess = false;
        protected bool m_QueueOpened = false;

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one MapMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one MapMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }

        public GameObject grid;

        private Dictionary<string, Tile> tileMap = new Dictionary<string, Tile>();

        private Vector2Int playerPosition = new Vector2Int();
        private enum GameState
        {
            exploration, // pre queueLoaded
            queueLoaded, // ready to go to point b
            countdown // go to point a
        };
        private GameState gameState;

        private string TileIdentifier(int x, int y)
        {
            return x.ToString() + "/" + y.ToString();
        }

        private void Start()
        {
            gameState = GameState.exploration;

            foreach (Tile tile in grid.GetComponentsInChildren<Tile>())
            {
                Vector3 tilePosition = tile.transform.position;
                tileMap[TileIdentifier((int)tilePosition.x, (int)tilePosition.y)] = tile;
            }
        }

        public void QueueEditorClose()
        {
            m_QueueOpened = false;
            PlayerCharacter.Instance.EnableInput();
            QueuePanelMgr.Instance.ToggleQueueExpanded();
        }

        public void QueueEditorOpen()
        {
            m_QueueOpened = true;
            PlayerCharacter.Instance.DisableInput();
            QueuePanelMgr.Instance.ToggleQueueExpanded();
            // TODO: implement queue logic. Need to load queue
            if (gameState == GameState.exploration)
            {
                // assumption: Player is on a valid tile
                Tile tile;
                tileMap.TryGetValue(TileIdentifier(playerPosition.x, playerPosition.y), out tile);
                if (tile.type == Tile.TileType.trigger)
                {
                    StartCountdown();
                }
                else
                {
                    gameState = GameState.queueLoaded;
                }
            }
        }

        public void NewPlayerPosition(int x, int y)
        {
            playerPosition = new Vector2Int(x, y);
            Tile tile; // assumption: Player moves to a valid tile
            tileMap.TryGetValue(TileIdentifier(x, y), out tile);
            switch (tile.type)
            {
                case Tile.TileType.origin:
                    if (gameState == GameState.countdown)
                    {
                        Debug.Log("player wins");
                    }
                    break;
                case Tile.TileType.trigger:
                    if (gameState == GameState.queueLoaded)
                    {
                        StartCountdown();
                    }
                    break;
                default:
                    // nothing
                    break;
            }
        }

        public bool CanMove(int x, int y)
        {
            Tile tile; // assumption: Player moves to a valid tile
            return (tileMap.TryGetValue(TileIdentifier(x, y), out tile) && tile != null);
        }

        public void UpdateWorld()
        {
            foreach (Tile tile in tileMap.Values)
            {
                if (tile != null)
                {
                    tile.GetComponent<Tile>().UpdateTile();
                }
            }

        }

        void StartCountdown()
        {
            gameState = GameState.countdown;
            foreach (Tile tile in tileMap.Values)
            {
                if (tile != null)
                {
                    tile.StartCountdown();
                }
            }
        }

        void Update()
        {
            ProcessInput();
        }

        void ProcessInput()
        {
            if (MapInput.Instance.Pause.Down)
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
            // else if (MapInput.Instance.QueueEditor.Down && !m_InPause) // Guille this is buggy as fuck
            else if (Input.GetKeyDown(KeyCode.Tab) && !m_InPause)
            {
                ProcessQueueEditorInput();
            }
        }

        void ProcessQueueEditorInput()
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

        public void Pause()
        {
            if (!m_InPause)
            {
                MapInput.Instance.ReleaseControl(false);
                MapInput.Instance.Pause.GainControl();
                m_InPause = true;

                // Hack for the input from the 2D Game Kit tutorial:
                //  If the time scale is not zeroed here, the input component will register
                //  twice the key downs.
                Time.timeScale = 0;

                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UIMenus", UnityEngine.SceneManagement.LoadSceneMode.Additive);

                // stop input processing from Player an Queue
                PlayerCharacter.Instance.DisableInput();
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
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("UIMenus");
            MapInput.Instance.GainControl();
            //we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
            //of this script happen BEFORE the input is updated, leading to setting the game in pause once again
            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            m_InPause = false;

            // resume input processing from Player an Queue
            PlayerCharacter.Instance.EnableInput();
            QueuePanelMgr.Instance.EnableInput();
        }
    }

}