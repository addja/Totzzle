using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace GOD
{
    public class QueueSlotsMgr : MonoBehaviour
    {
        // BEGIN Singleton stuff
        public static QueueSlotsMgr Instance
        {
            get { return s_Instance; }
        }

        protected static QueueSlotsMgr s_Instance;

        void SingletonAwake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one QueueSlotsMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
    
            m_queueSlots = GetComponentsInChildren<QueueSlot>();
            
            Assert.IsTrue(m_queueSlots.Length > 0);
            foreach (QueueSlot queueSlot in m_queueSlots)
            {
                queueSlot.Enable();
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
        // END Singleton stuff

        private QueueSlot[] m_queueSlots;
        private QueueSlot m_activeQueueSlot;
        private int m_activeQueueSlotIndex;
        private bool m_queueLoaded = false;

        void Awake()
        {
            SingletonAwake();

            m_queueSlots = GetComponentsInChildren<QueueSlot>();
            
            Assert.IsTrue(m_queueSlots.Length > 0);
            foreach (QueueSlot queueSlot in m_queueSlots)
            {
                queueSlot.Enable();
            }
            m_activeQueueSlotIndex = 0;
            m_activeQueueSlot = m_queueSlots[m_activeQueueSlotIndex];
        }

        public void HighlightSlot()
        {
            m_activeQueueSlot.Highlight();
        }

        public void DeselectSlot()
        {
            m_activeQueueSlot.Disable();
        }

        public void SelectLeftSlot()
        {
            m_activeQueueSlot.Disable();

            if (m_activeQueueSlotIndex == 0)
            {
                m_activeQueueSlotIndex = m_queueSlots.Length-1;
            } else {
                m_activeQueueSlotIndex--;
            }

            m_activeQueueSlot = m_queueSlots[m_activeQueueSlotIndex];
            m_activeQueueSlot.Highlight();
        }

        public void SelectRightSlot()
        {
            m_activeQueueSlot.Disable();

            if (m_activeQueueSlotIndex == m_queueSlots.Length-1)
            {
                m_activeQueueSlotIndex = 0;
            } else {
                m_activeQueueSlotIndex++;
            }

            m_activeQueueSlot = m_queueSlots[m_activeQueueSlotIndex];
            m_activeQueueSlot.Highlight();
        }

        public void OptionsSlotsSelected()
        {
            m_activeQueueSlot.Select();
        }

        public void SetSlotValue(int value, OptionSlot optionSlot)
        {
            m_activeQueueSlot.SetSlotValue(value, optionSlot);
        }

        public void CheckQueueLoaded() {
            if (m_queueLoaded) {
                return;
            }
            foreach (QueueSlot queueSlot in m_queueSlots)
            {
               if (!queueSlot.m_loaded) {
                    return;
                } 
            }

            Debug.Log("queue loaded");
            m_queueLoaded = true;
            PuzzleMgr.Instance.QueueLoaded();
        }
    }
}