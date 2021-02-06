using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class QueuePanelMgr : MonoBehaviour
    {
        public static QueuePanelMgr Instance
        {
            get { return s_Instance; }
        }

        protected static QueuePanelMgr s_Instance;

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one QueuePanelMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one QueuePanelMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }
        // Manages which queue panel is displayed (compressed or expanded)
        public GameObject queueCompressedPanel;
        public GameObject queueExpandedPanel;

        protected bool m_IsInputDisabled = true;
        private bool queueExpanded = false;

        public void ToggleQueueExpanded()
        {
            m_IsInputDisabled = !m_IsInputDisabled;
            queueExpanded = !queueExpanded;
            if (queueExpanded)
            {
                queueCompressedPanel.SetActive(false);
                queueExpandedPanel.SetActive(true);
            }
            else
            {
                queueExpandedPanel.SetActive(false);
                queueCompressedPanel.SetActive(true);

            }
        }

        public void DisableInput() {
            m_IsInputDisabled = true;
        }

        public void EnableInput() {
            m_IsInputDisabled = false;
        }


        private void Update()
        {
            if (!m_IsInputDisabled) {
                // processInput
            }
        }
    }
}