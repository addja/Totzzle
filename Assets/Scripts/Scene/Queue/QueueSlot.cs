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

        private void Start()
        {
            m_image = GetComponent<Image>();
        }

        public void Activate()
        {
            m_queueSlotState = QueueSlotState.enabled;
            m_image.color = new Color(1, 1, 0);
        }

        public void Deactivate()
        {
            m_queueSlotState = QueueSlotState.disabled;
            if (!m_image) {
                m_image = GetComponent<Image>();
            }
            m_image.color = new Color(0, 0, 0);
        }

        public void Select()
        {
            m_queueSlotState = QueueSlotState.selected;
            m_image.color = new Color(.5f, .25f, .25f);
        }
    }
}