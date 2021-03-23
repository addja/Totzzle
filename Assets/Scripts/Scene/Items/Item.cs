using UnityEngine;

namespace GOD
{
    public abstract class Item : MonoBehaviour
    {
        protected bool m_isMovable = false;

        public abstract void StartCountdown();
        public abstract bool IsIdle();

        public bool IsMovable()
        {
            return m_isMovable;
        }
    }
}