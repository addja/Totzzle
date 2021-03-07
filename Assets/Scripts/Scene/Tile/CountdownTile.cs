using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class CountdownTile : Tile
    {
        public int m_counter = 0;
        protected bool m_countDownEnabled = false;

        protected override void Awake() {
            m_tileAnimationCode = 1; // Used by parent Awake
            base.Awake();
            m_type = TileType.countdown;
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
                        GridMgr.Instance.Pause(GridMgr.PauseType.lose);
                    }
                    Destroy(gameObject);
                }
                m_text.text = m_counter.ToString();
            }
        }
    
        public override void StartCountdown()
        {
            Debug.Log("Starting counting down");
            m_countDownEnabled = true;
        }
    }
}
