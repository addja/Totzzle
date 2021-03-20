using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected bool m_isMovable = false;

    public abstract void StartCountdown();

    public bool IsMovable()
    {
        return m_isMovable;
    }
}
