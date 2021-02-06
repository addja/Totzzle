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
            if (!m_InPause && !isMoving)
            {
                ProcessPlayerMovementInput();
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

        // fancy coroutine
        protected IEnumerator MovePlayer(Vector2 direction)
        {
            origPosition = transform.position;
            targetPosition = origPosition + direction;

            if (MapMgr.Instance.CanMove((int)targetPosition.x, (int)targetPosition.y))
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
                MapMgr.Instance.UpdateWorld();
                MapMgr.Instance.NewPlayerPosition((int)targetPosition.x, (int)targetPosition.y);
            }
        }
    }
}
