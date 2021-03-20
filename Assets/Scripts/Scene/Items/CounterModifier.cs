using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterModifier : Item
{
    public override void StartCountdown()
    {
        Destroy(gameObject);
    }
}
