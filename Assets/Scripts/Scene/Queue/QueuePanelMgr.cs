using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public Image panelImage;
        public Image queueBorder;

        protected bool m_IsInputDisabled = true;

        public void EnableQueue() {
            m_IsInputDisabled = true;
            panelImage.color = new Color(1, 1, 1);
            queueBorder.color = new Color(1, 0, 0);
        }

        public void DisableQueue() {
            m_IsInputDisabled = false;
            panelImage.color = new Color(0, 0, 0);
            queueBorder.color = new Color(.5f, .5f, .5f);
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