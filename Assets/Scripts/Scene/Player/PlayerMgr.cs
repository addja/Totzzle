using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GOD
{
    public class PlayerMgr : MonoBehaviour
    {
        // BEGIN Singleton stuff
        static protected PlayerMgr s_Instance;
        static public PlayerMgr Instance { get { return s_Instance; } }

        private void SingletonAwake ()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one PlayerMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        private void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if(s_Instance != this)
                throw new UnityException("There cannot be more than one PlayerMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        private void OnDisable()
        {
            s_Instance = null;
        }
        // END Singleton stuff

        public float timeToMove = .2f;

        protected bool m_isMoving = false;
        protected Animator m_animator;

        protected bool m_IsInputDisabled = false;
        protected Vector2 origPosition, targetPosition;

        public GameObject playerDisablePanel;

        private void Awake()
        {
            SingletonAwake();

            m_animator = GetComponentInChildren<Animator>();
            Assert.IsNotNull(m_animator);
            AnimatePlayer();
        }

        protected void AnimatePlayer() {
            m_animator.SetBool("isMoving", m_isMoving);
        }

        public void DisablePlayer() {
            m_IsInputDisabled = true;
            playerDisablePanel.SetActive(true);
        }

        public void EnablePlayer() {
            m_IsInputDisabled = false;
            playerDisablePanel.SetActive(false);
        }

        public void DisableInput() {
            m_IsInputDisabled = true;
        }

        public void EnableInput() {
            m_IsInputDisabled = false;
        }

        void Update()
        {
            if (!m_isMoving && !m_IsInputDisabled)
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
            targetPosition = origPosition + direction.normalized * GridMgr.Instance.transform.localScale;

            if (GridMgr.Instance.CanMove(targetPosition.x, targetPosition.y))
            {
                m_isMoving = true;
                AnimatePlayer();
                float ellapsedTime = 0;

                AudioMgr.Instance.Play("Step");

                while (ellapsedTime < timeToMove)
                {
                    transform.position = Vector2.Lerp(origPosition, targetPosition, (ellapsedTime / timeToMove));
                    ellapsedTime += Time.deltaTime;
                    yield return null;
                }

                // make sure there is no small jitter from lerp on final position
                transform.position = targetPosition;

                m_isMoving = false;
                AnimatePlayer();
                GridMgr.Instance.UpdateWorld();
            }
        }
    }
}
