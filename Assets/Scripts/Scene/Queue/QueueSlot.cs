using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class QueueSlot : MonoBehaviour
    {
        private enum QueueSlotState {
            enabled,
            disabled,
            selected,
        }
        private QueueSlotState m_queueSlotState = QueueSlotState.disabled;
        private Image m_image; // TODO use animations instead
        private int m_value;
        private TMPro.TMP_Text m_text;
        private OptionSlot m_cachedOptionSlot;

        private void Start()
        {
            m_image = GetComponent<Image>();
            m_text = GetComponentInChildren<TMPro.TMP_Text>();
        }

        public void Activate()
        {
            m_queueSlotState = QueueSlotState.enabled;
            m_image.color = new Color(.5f, .5f, 0);
        }

        public void Deactivate()
        {
            m_queueSlotState = QueueSlotState.disabled;
            if (!m_image) { // BUG? Will go away anyway with the animation behaviour
                m_image = GetComponent<Image>();
            }
            m_image.color = new Color(0, 0, 0);
        }

        public void Select()
        {
            m_queueSlotState = QueueSlotState.selected;
        }

        public void SetSlotValue(int value, OptionSlot optionSlot)
        {
            m_value = value;
            m_text.text = m_value.ToString();
            if(m_cachedOptionSlot) {
                m_cachedOptionSlot.Activate();
            }
            m_cachedOptionSlot = optionSlot;
        }
    }
}