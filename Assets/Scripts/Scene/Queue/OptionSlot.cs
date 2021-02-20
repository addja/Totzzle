using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class OptionSlot : MonoBehaviour
    {
        private enum OptionSlotState {
            enabled,
            disabled,
            selected,
        }
        private OptionSlotState m_optionSlotState;
        private Image m_image; // TODO use animations instead

        private void Start()
        {
            m_optionSlotState = OptionSlotState.enabled;
            m_image = GetComponent<Image>();
        }

        public void Activate()
        {
            m_optionSlotState = OptionSlotState.selected;
            m_image.color = new Color(1, 0, 0);
        }

        public void Deactivate()
        {
            m_optionSlotState = OptionSlotState.enabled;
            m_image.color = new Color(1, 1, 1);
        }

        public void LockInQueue()
        {
            m_optionSlotState = OptionSlotState.disabled;
            m_image.color = new Color(.5f, .5f, .5f);
        }

        public bool IsNotDisabled() {
            return m_optionSlotState == OptionSlotState.disabled;
        }
    }
}