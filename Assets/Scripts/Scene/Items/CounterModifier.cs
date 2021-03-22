using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace GOD
{
    public class CounterModifier : Item
    {

        // To control movement coroutine
        private Vector2 m_originalPosition;
        private Vector2 m_targetPosition;
        private Vector2 m_direction;
        private bool m_moving = false;
        public float m_timeToMove = 0.15f; // Needs to be smaller than player m_timeToMove to avoid bugs
        public int m_value = 0;

        public override void StartCountdown()
        {
            CountdownTile countdownTile = PuzzleMgr.Instance.GetCountdownTile(
                (int)transform.position.x,(int)transform.position.y);
            if (countdownTile != null)
            {
                countdownTile.m_counter += m_value;
            }
            Destroy(gameObject);
        }

        private void Awake()
        {
            m_isMovable = true;
        }

        public void Move(Vector2 direction)
        {
            if (!m_moving)
            {
                m_direction = direction;
                m_originalPosition = transform.position;
                m_targetPosition = m_originalPosition + m_direction;
                StartCoroutine(MoveCounterModifier());
            }
        }

        private IEnumerator MoveCounterModifier()
        {
            // AnimatePlayer(PlayerAnimation.move);
            float ellapsedTime = 0;
            m_moving = true;

            // AudioMgr.Instance.Play("CounterModifierMovement");

            while (ellapsedTime < m_timeToMove)
            {
                transform.position = Vector2.Lerp(
                    m_originalPosition, m_targetPosition, (ellapsedTime / m_timeToMove));
                ellapsedTime += Time.deltaTime;
                yield return null;
            }

            // make sure there is no small jitter from lerp on final position
            transform.position = m_targetPosition;
            m_moving = false;

            // AudioMgr.Instance.Stop("CounterModifierMovement");
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // If more objects to be entering this if, use tags
            if (other.collider.name == "CounterModifier")
            {
                var counterModifier = other.collider.gameObject.GetComponent<CounterModifier>();
                Assert.IsNotNull(counterModifier);
                // Assumption: We can only collide if we are moving the player, who checks if the object
                // can be moved. We can extend this to bounch back in the future
                counterModifier.Move(m_direction);
            }
        }
    }
}