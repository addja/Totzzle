using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class QueueSlotsMgr : MonoBehaviour
    {
        public static QueueSlotsMgr Instance
        {
            get { return s_Instance; }
        }

        protected static QueueSlotsMgr s_Instance;

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one QueueSlotsMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
    
            m_queueSlots = GetComponentsInChildren<QueueSlot>();
            foreach (QueueSlot queueSlot in m_queueSlots)
            {
                queueSlot.Deactivate();
            }
            m_activeQueueSlotIndex = 0;
            m_activeQueueSlot = m_queueSlots[m_activeQueueSlotIndex];
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one QueueSlotsMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }

        private QueueSlot[] m_queueSlots;
        private QueueSlot m_activeQueueSlot;
        private int m_activeQueueSlotIndex;

        public void SelectSlot()
        {
            m_activeQueueSlot.Activate();
        }

        public void DeselectSlot()
        {
            m_activeQueueSlot.Deactivate();
        }

        public void SelectLeftSlot()
        {
            m_activeQueueSlot.Deactivate();

            if (m_activeQueueSlotIndex == 0)
            {
                m_activeQueueSlotIndex = m_queueSlots.Length-1;
            } else {
                m_activeQueueSlotIndex--;
            }

            m_activeQueueSlot = m_queueSlots[m_activeQueueSlotIndex];
            m_activeQueueSlot.Activate();
        }

        public void SelectRightSlot()
        {
            m_activeQueueSlot.Deactivate();

            if (m_activeQueueSlotIndex == m_queueSlots.Length-1)
            {
                m_activeQueueSlotIndex = 0;
            } else {
                m_activeQueueSlotIndex++;
            }

            m_activeQueueSlot = m_queueSlots[m_activeQueueSlotIndex];
            m_activeQueueSlot.Activate();
        }

        public void OptionsSlotsSelected()
        {
            m_activeQueueSlot.Select();
        }

        public void SetSlotValue(int value, OptionSlot optionSlot)
        {
            m_activeQueueSlot.SetSlotValue(value, optionSlot);
        }
    }
}