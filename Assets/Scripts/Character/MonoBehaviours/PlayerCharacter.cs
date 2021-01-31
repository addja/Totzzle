using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class PlayerCharacter : MonoBehaviour
    {
        static protected PlayerCharacter s_Instance;
        static public PlayerCharacter Instance { get { return s_Instance; } }

        public float timeToMove = .2f;
        public MapManager mapManager;

        protected bool m_InPause = false;
        protected bool m_InPausingProcess = false;
        protected bool isMoving = false;
        protected Vector2 origPosition, targetPosition;

        void Awake ()
        {
            s_Instance = this;
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if(s_Instance != this)
                throw new UnityException("There cannot be more than one PlayerCharacter script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }

        void Update()
        {
            if (PlayerInput.Instance.Pause.Down)
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
            else if (!isMoving && !m_InPause)
            {
                ProcessInput();
            }
        }

        void ProcessInput()
        {
            ProcessQueueEditorOpened();
            ProcessPlayerMovementInput();
        }

        void ProcessQueueEditorOpened()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                mapManager.QueueLoaded();
            }
        }

         void ProcessPlayerMovementInput() {
            Vector2 direction = Vector2.zero;

            if (PlayerInput.Instance.Vertical.Value > 0f)
            {
                direction = Vector2.up;
            }
            else if (PlayerInput.Instance.Vertical.Value < 0f)
            {
                direction = Vector2.down;
            }
            else if (PlayerInput.Instance.Horizontal.Value < 0f)
            {
                direction = Vector2.left;
            }
            else if (PlayerInput.Instance.Horizontal.Value > 0f)
            {
                direction = Vector2.right;
            }
            else
            {
                return;
            }

            StartCoroutine(MovePlayer(direction));
        }

        public void Pause()
        {
            if (!m_InPause)
            {
                PlayerInput.Instance.ReleaseControl(false);
                PlayerInput.Instance.Pause.GainControl();
                m_InPause = true;

                // Hack for the input from the 2D Game Kit tutorial:
                //  If the time scale is not zeroed here, the input component will register
                //  twice the key downs.
                Time.timeScale = 0;

                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("UIMenus", UnityEngine.SceneManagement.LoadSceneMode.Additive);
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
            PlayerInput.Instance.GainControl();
            //we have to wait for a fixed update so the pause button state change, otherwise we can get in case were the update
            //of this script happen BEFORE the input is updated, leading to setting the game in pause once again
            yield return new WaitForFixedUpdate();
            yield return new WaitForEndOfFrame();
            m_InPause = false;
        }

        // fancy coroutine
        protected IEnumerator MovePlayer(Vector2 direction)
        {
            origPosition = transform.position;
            targetPosition = origPosition + direction;

            if (mapManager.CanMove((int)targetPosition.x, (int)targetPosition.y))
            {
                isMoving = true;
                float ellapsedTime = 0;
                while (ellapsedTime < timeToMove)
                {
                    transform.position = Vector2.Lerp(origPosition, targetPosition, (ellapsedTime / timeToMove));
                    ellapsedTime += Time.deltaTime;
                    yield return null;
                }

                // make sure there is no small jitter from lerp on final position
                transform.position = targetPosition;

                isMoving = false;
                mapManager.UpdateWorld();
                mapManager.NewPlayerPosition((int)targetPosition.x, (int)targetPosition.y);
            }
        }
    }
}
