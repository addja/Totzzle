using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class QueueSlot : KeyboardButton
    {
        private OptionSlot m_cachedOptionSlot;
        public bool m_loaded = false;

        public void SetSlotValue(int value, OptionSlot optionSlot)
        {
            m_value = value;
            m_text.text = m_value.ToString();
            if (m_cachedOptionSlot)
            {
                m_cachedOptionSlot.Enable();
            }
            m_cachedOptionSlot = optionSlot;
            m_loaded = true;
            QueueSlotsMgr.Instance.CheckQueueLoaded();
        }
    }
}