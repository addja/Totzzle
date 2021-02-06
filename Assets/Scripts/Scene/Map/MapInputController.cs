using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class MapInputController : MonoBehaviour
    {
        static protected MapInputController s_Instance;
        static public MapInputController Instance { get { return s_Instance; } }

        public float timeToMove = .2f;

        protected bool m_InPause = false;
        protected bool m_InPausingProcess = false;
        protected bool isMoving = false;
        protected Vector2 origPosition, targetPosition;

        void Awake()
        {
            s_Instance = this;
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one MapInputController script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }
    }
}
