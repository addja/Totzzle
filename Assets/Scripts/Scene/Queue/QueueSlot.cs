using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class QueueSlot : KeyboardButton
    {
        private int m_value;
        private OptionSlot m_cachedOptionSlot;

        public void SetSlotValue(int value, OptionSlot optionSlot)
        {
            m_value = value;
            m_text.text = m_value.ToString();
            if (m_cachedOptionSlot)
            {
                m_cachedOptionSlot.Activate();
            }
            m_cachedOptionSlot = optionSlot;
        }
    }

}