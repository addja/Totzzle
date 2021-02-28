using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class CountdownTile : Tile
    {
        public uint m_counter = 3;
        protected bool m_countDownEnabled = false;

        protected override void Awake() {
            m_tileAnimationCode = 1; // Used by parent Awake
            base.Awake();
            m_type = TileType.countdown;
            m_text.text = m_counter.ToString();
        }

        public override void UpdateTile()
        {
            if (m_countDownEnabled && m_counter != 0)
            {
                // TODO reactor to have more than minus one step
                m_counter--;
                if (m_counter == 0)
                {
                    if (PlayerMgr.Instance.transform.position == transform.position)
                    {
                        Debug.Log("YOU LOOSE");
                        // TODO play you loose animation
                    }
                    Destroy(gameObject);
                }
                m_text.text = m_counter.ToString();
            }
        }
    
        public override void StartCountdown()
        {
            m_countDownEnabled = true;
        }
    }
}
