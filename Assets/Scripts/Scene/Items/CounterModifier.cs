using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterModifier : Item
{

    // To control movement coroutine
    private Vector2 m_originalPosition;
    private Vector2 m_targetPosition;
    public float m_timeToMove = 0.1f;

    public override void StartCountdown()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        m_isMovable = true;
    }

    public void Move(Vector2 direction) {
        m_originalPosition = transform.position;
        m_targetPosition = m_originalPosition + direction;
        StartCoroutine(MoveCounterModifier());
    }

    private IEnumerator MoveCounterModifier()
    {
        // AnimatePlayer(PlayerAnimation.move);
        float ellapsedTime = 0;

        // AudioMgr.Instance.Play("Step");

        while (ellapsedTime < m_timeToMove)
        {
            transform.position = Vector2.Lerp(
                m_originalPosition, m_targetPosition, (ellapsedTime / m_timeToMove));
            ellapsedTime += Time.deltaTime;
            yield return null;
        }

        // make sure there is no small jitter from lerp on final position
        transform.position = m_targetPosition;

        // AnimatePlayer(PlayerAnimation.idle);
    }
}
