using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class OptionSlot : KeyboardButton
    {
        public void LockInQueue()
        {
            base.Disable();
            QueueSlotsMgr.Instance.SetSlotValue(m_value, this);
        }

        public bool IsDisabled()
        {
            return m_state == KeyboardButtonState.disabled;
        }

        [ExecuteInEditMode] // This seems to only execute in game
        private void Update() {
            m_text.text = m_value.ToString();
        }

    }
}