using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterModifier : Item
{
    public override void StartCountdown()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        m_isMovable = true;
    }
}
