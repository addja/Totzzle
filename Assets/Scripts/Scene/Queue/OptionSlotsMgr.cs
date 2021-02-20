using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class OptionSlotsMgr : MonoBehaviour
    {
        public static OptionSlotsMgr Instance
        {
            get { return s_Instance; }
        }

        protected static OptionSlotsMgr s_Instance;

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one OptionSlotsMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one OptionSlotsMgr script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }

        private OptionSlot[] m_optionSlots;
        private OptionSlot m_activeOptionSlot;
        private int m_activeOptionSlotIndex;

        void Start()
        {
            m_optionSlots = GetComponentsInChildren<OptionSlot>();
            foreach (OptionSlot optionSlot in m_optionSlots)
            {
                optionSlot.Deactivate();
            }
            m_activeOptionSlotIndex = 0;
            m_activeOptionSlot = m_optionSlots[m_activeOptionSlotIndex];
        }

        public void SelectSlot()
        {
            Debug.Log("Select slot");
            m_activeOptionSlot.Activate();
        }

        public void DeselectSlot()
        {
            Debug.Log("DeselectSlot slot");
            m_activeOptionSlot.Deactivate();
        }

        public void SelectLeftSlot()
        {
            Debug.Log("SelectLeftSlot");
            m_activeOptionSlot.Deactivate();
            MoveSelectedLeft();
        }

        public void SelectRightSlot()
        {
            Debug.Log("SelectRightSlot");
            m_activeOptionSlot.Deactivate();
            MoveSelectedRight();
        }

        public void LockInQueue()
        {
            m_activeOptionSlot.LockInQueue();
            MoveSelectedRight();
        }

        private void MoveSelectedRight() {
            int securityCounter = 0;
            do
            {
                if (m_activeOptionSlotIndex == m_optionSlots.Length - 1)
                {
                    m_activeOptionSlotIndex = 0;
                }
                else
                {
                    m_activeOptionSlotIndex++;
                }
                m_activeOptionSlot = m_optionSlots[m_activeOptionSlotIndex];

                // We should never require this. But for security this would avoid getting OOM
                ++securityCounter;
                if (securityCounter == 100) {
                    break;
                }
            } while (m_activeOptionSlot.IsNotDisabled());

            m_activeOptionSlot.Activate();
        }

        private void MoveSelectedLeft() {
            int securityCounter = 0;
            do
            {
                if (m_activeOptionSlotIndex == 0)
                {
                    m_activeOptionSlotIndex = m_optionSlots.Length - 1;
                }
                else
                {
                    m_activeOptionSlotIndex--;
                }
                m_activeOptionSlot = m_optionSlots[m_activeOptionSlotIndex];

                // We should never require this. But for security this would avoid getting OOM
                ++securityCounter;
                if (securityCounter == 100) {
                    break;
                }
            } while (m_activeOptionSlot.IsNotDisabled());

            m_activeOptionSlot.Activate();
        }
    }
}