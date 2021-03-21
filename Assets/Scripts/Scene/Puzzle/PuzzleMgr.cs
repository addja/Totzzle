using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GOD
{
    public class PuzzleMgr : Singleton<PuzzleMgr>
    {
        public enum PauseType
        {
            none,
            menus,
            win,
            lose
        }

        public string MenusSceneName = "";
        public string WinSceneName = "";
        public string LoseSceneName = "";
        private PauseType m_currentPause = PauseType.none;
        private bool m_QueueOpened = false;
        private bool m_countdownStarted = false;

        private Dictionary<string, Tile> m_tileMap = new Dictionary<string, Tile>();
        private TriggerTile m_triggerTile;
        
        // TODO: We can do better than a list efficy-wise
        // Leaving for the moment as we are focused on bring-up
        private List<Item> m_items = new List<Item>(); 

        private string TileIdentifier(float x, float y)
        {
            return x.ToString() + "/" + y.ToString();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            GameObject tiles = transform.Find("Tiles").gameObject;
            Assert.IsTrue(tiles);
            foreach (Tile tile in tiles.GetComponentsInChildren<Tile>())
            {
                Vector3 tilePosition = tile.transform.position;
                m_tileMap[TileIdentifier(tilePosition.x, tilePosition.y)] = tile;
                if (tile.m_type == Tile.TileType.trigger) {
                    Assert.IsFalse(m_triggerTile); // We only want a triggerTiler per grid
                    m_triggerTile = (TriggerTile)tile;
                }
            }

            Assert.IsTrue(m_triggerTile); // We need one triggerTile per grid

            GameObject items = transform.Find("Items").gameObject;
            Assert.IsTrue(items);
            foreach (Item item in items.GetComponentsInChildren<Item>())
            {
                m_items.Add(item);
            }

            // Restore the hack for the input from the 2D Game Kit tutorial
            //  in those cases when we leave the level in the middle of a pause.
            Time.timeScale = 1;

            HUDMgr hudMgr = HUDMgr.Instance;
            Assert.IsTrue(hudMgr); // We need the HUD manager.
            PlayerMgr playerMgr = PlayerMgr.Instance;
            Assert.IsTrue(playerMgr); // We need the player manager.

            if (hudMgr.m_state != HUDMgr.State.idle)
            {
                m_QueueOpened = true;
                playerMgr.DisablePlayer();
            }
            else
            {
                playerMgr.EnablePlayer();
            }

            hudMgr.m_queueLoadedEvent.AddListener(() => m_triggerTile.EnableTrigger());
            hudMgr.m_queueUnloadedEvent.AddListener(() => m_triggerTile.DisableTrigger());
        }

        public void QueueEditorClose()
        {
            if (m_QueueOpened)
            {
                m_QueueOpened = false;
                PlayerMgr.Instance.EnablePlayer();
                HUDMgr.Instance.SetState(HUDMgr.State.idle);
            }
        }

        public void QueueEditorOpen()
        {
            if (!m_QueueOpened && !m_countdownStarted)
            {
                m_QueueOpened = true;
                PlayerMgr.Instance.DisablePlayer();
                HUDMgr.Instance.SetState(HUDMgr.State.queue);
            }
        }

        public bool CanMove(float x, float y)
        {
            Tile tile;
            m_tileMap.TryGetValue(TileIdentifier(x,y), out tile);
            if (tile == null)
            {
                return false;
            };
            Item item = ItemInTile(x, y);
            if (item != null) {
                if (!item.IsMovable()) {
                    return false;
                }

                Vector2 direction = PlayerMgr.Instance.MovementDirection().normalized;
                direction *= PuzzleMgr.Instance.transform.localScale;
                return CanMove(x+direction.x,y+direction.y);
            }

            return true;
        }

        private Item ItemInTile(float x, float y)
        {
            foreach (Item item in m_items)
            {
                if (item == null) {
                    continue;
                }
                Vector3 itemPosition = item.transform.position;
                if (itemPosition.x == x && itemPosition.y == y)
                {
                    return item;
                }
            }

            return null;
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

            if (m_countdownStarted)
            {
                foreach (Tile tile in m_tileMap.Values)
                {
                    if (tile != null)
                    {
                        tile.StartCountdown();
                    }
                }
            }
        }

        public void StartCountdown()
        {
            if (!m_countdownStarted)
            {
                m_countdownStarted = true;

                QueueEditorClose();

                HUDMgr.Instance.SetState(HUDMgr.State.countdown);

                AudioMgr.Instance.Play("Count down");
            }
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (PuzzleInput.Instance.Pause.Down)
            {
                switch (m_currentPause)
                {
                    case PauseType.none:
                    {
                        Pause(PauseType.menus);
                    }
                    break;

                    case PauseType.menus:
                    {
                        Unpause();
                    }
                    break;
                }
            }
            // else if (PuzzleInput.Instance.QueueEditor.Down && m_currentPause == PauseType.none) // Guille this is buggy as fuck
            else if (Input.GetKeyDown(KeyCode.Tab) && m_currentPause == PauseType.none)
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

        public void Reset()
        {
            if (m_QueueOpened)
            {
                QueueEditorClose();
            }

            if (m_currentPause != PauseType.none)
            {
                Unpause();
            }

            SceneMgr.RestartScene();
        }

        public void Pause(PauseType pauseType)
        {
            if (m_currentPause == PauseType.none)
            {
                m_currentPause = pauseType;
                PuzzleInput.Instance.ReleaseControl(false);
                PuzzleInput.Instance.Pause.GainControl();

                // stop input processing from Player an Queue
                if (!m_QueueOpened)
                {
                    PlayerMgr.Instance.DisableInput();
                }
                else
                {
                    HUDMgr.Instance.DisableInput();
                }

                // Hack for the input from the 2D Game Kit tutorial:
                //  If the time scale is not zeroed here, the input component will register
                //  twice the key downs.
                Time.timeScale = 0;

                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GetPauseSceneName(m_currentPause), UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
        }

        public void Unpause()
        {
            //if the timescale is already > 0, we 
            if (Time.timeScale > 0)
                return;

            if (m_currentPause != PauseType.none)
            {
                StartCoroutine(UnpauseCoroutine());
            }
        }

        protected IEnumerator UnpauseCoroutine()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(GetPauseSceneName(m_currentPause));
            PuzzleInput.Instance.GainControl();

            // resume input processing from Player an Queue
            if (!m_QueueOpened)
            {
                PlayerMgr.Instance.EnableInput();
            }
            else
            {
                HUDMgr.Instance.EnableInput();
            }

            //we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
            //of this script happen BEFORE the input is updated, leading to setting the game in pause once again
            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            m_currentPause = PauseType.none;
        }

        private string GetPauseSceneName(PauseType pauseType)
        {
            string SceneName = "";

            switch (pauseType)
            {
                case PauseType.menus:
                {
                    SceneName = MenusSceneName;
                }
                break;

                case PauseType.win:
                {
                    SceneName = WinSceneName;
                }
                break;

                case PauseType.lose:
                {
                    SceneName = LoseSceneName;
                }
                break;
            }

            return SceneName;
        }
    }

}
