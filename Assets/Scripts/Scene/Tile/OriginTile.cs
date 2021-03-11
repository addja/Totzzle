using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class OriginTile : Tile
    {
        protected override void Awake() {
            m_tileAnimationCode = -1; // Used by parent Awake
            base.Awake();
            m_type = TileType.origin;
            m_text.text = "A";
        }
        protected bool m_countDownEnabled = false;

        public override void UpdateTile()
        {
            if (PlayerMgr.Instance.transform.position == transform.position)
            {
                if (m_countDownEnabled)
                {
                    AudioMgr.Instance.Play("Win");
                    GridMgr.Instance.Pause(GridMgr.PauseType.win);
                }
            }
        }

        public override void StartCountdown()
        {
            m_countDownEnabled = true;
        }
    }
}
