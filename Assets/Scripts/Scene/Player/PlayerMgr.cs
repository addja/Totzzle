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

        public float m_timeToMove = .2f;
        public float m_timeInvalidAnim = .2f;

        protected bool m_isBusy = false;
        protected Animator m_animator;

        protected bool m_IsInputDisabled = false;
        protected Vector2 origPosition, targetPosition;

        private void Awake()
        {
            SingletonAwake();

            m_animator = GetComponentInChildren<Animator>();
            Assert.IsNotNull(m_animator);
            AnimatePlayer(PlayerAnimation.idle);
        }

        protected enum PlayerAnimation
        {
            idle,
            move,
            invalid,
        };

        protected void AnimatePlayer(PlayerAnimation playerAnimation) {
            switch(playerAnimation){
                case PlayerAnimation.idle:
                    m_animator.SetInteger("animation", 0);
                    break;
                case PlayerAnimation.move:
                    m_animator.SetInteger("animation", 1);
                    break;
                case PlayerAnimation.invalid:
                    m_animator.SetInteger("animation", 2);
                    break;
                default:
                    Assert.IsTrue(false);
                    break;
            }
        }

        public void DisablePlayer() {
            m_IsInputDisabled = true;
        }

        public void EnablePlayer() {
            m_IsInputDisabled = false;
        }

        public void DisableInput() {
            m_IsInputDisabled = true;
        }

        public void EnableInput() {
            m_IsInputDisabled = false;
        }

        void Update()
        {
            if (!m_isBusy && !m_IsInputDisabled)
            {
                ProcessPlayerMovementInput();
            }
        }

        void ProcessPlayerMovementInput() {
            Vector2 direction = Vector2.zero;

            // Had to change this to keydown to not call a thousand times the invalid movement anim
            // if (PlayerInput.Instance.Vertical.Value > 0f)
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = Vector2.up;
            }
            // else if (PlayerInput.Instance.Vertical.Value < 0f)
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector2.down;
            }
            // else if (PlayerInput.Instance.Horizontal.Value < 0f)
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direction = Vector2.left;
            }
            // else if (PlayerInput.Instance.Horizontal.Value > 0f)
            else if (Input.GetKeyDown(KeyCode.D))
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
            targetPosition = origPosition + direction.normalized * PuzzleMgr.Instance.transform.localScale;

            if (PuzzleMgr.Instance.CanMove(targetPosition.x, targetPosition.y))
            {
                m_isBusy = true;
                AnimatePlayer(PlayerAnimation.move);
                float ellapsedTime = 0;

                AudioMgr.Instance.Play("Step");

                while (ellapsedTime < m_timeToMove)
                {
                    transform.position = Vector2.Lerp(origPosition, targetPosition, (ellapsedTime / m_timeToMove));
                    ellapsedTime += Time.deltaTime;
                    yield return null;
                }

                // make sure there is no small jitter from lerp on final position
                transform.position = targetPosition;

                m_isBusy = false;
                AnimatePlayer(PlayerAnimation.idle);
                PuzzleMgr.Instance.UpdateWorld();
            } else {
                AnimatePlayer(PlayerAnimation.invalid);
                float ellapsedTime = 0;
                m_isBusy = true;

                AudioMgr.Instance.Play("InvalidMove");

                while (ellapsedTime < m_timeInvalidAnim)
                {
                    ellapsedTime += Time.deltaTime;
                    yield return null;
                }
                m_isBusy = false;
                AnimatePlayer(PlayerAnimation.idle);
            }
        }
    }
}
