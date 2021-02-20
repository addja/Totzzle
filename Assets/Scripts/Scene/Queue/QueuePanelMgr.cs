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

        protected bool m_InputEnabled = false;

        public void EnableQueue() {
            Debug.Log("Queue enabled");
            m_InputEnabled = true;
            panelImage.color = new Color(.4f, .4f, .4f);
            queueBorder.color = new Color(1, 0, 0);
            QueueSlotsMgr.Instance.SelectSlot();
        }

        public void DisableQueue() {
            Debug.Log("Queue disabled");
            m_InputEnabled = false;
            panelImage.color = new Color(0, 0, 0);
            queueBorder.color = new Color(.5f, .5f, .5f);
            QueueSlotsMgr.Instance.DeselectSlot();
        }

        public void DisableInput() {
            m_InputEnabled = false;
        }

        public void EnableInput() {
            m_InputEnabled = true;
        }


        private void Update()
        {
            if (m_InputEnabled) {

                // input is very broken bruh
                // if (QueuePanelInput.Instance.Horizontal.Value < 0f)
                if (Input.GetKeyDown(KeyCode.D))
                {
                    QueueSlotsMgr.Instance.SelectRightSlot();
                }
                // else if (QueuePanelInput.Instance.Horizontal.Value > 0f)
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    QueueSlotsMgr.Instance.SelectLeftSlot();
                }
            }
        }
    }
}