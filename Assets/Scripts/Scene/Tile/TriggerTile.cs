using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GOD
{
    public class TriggerTile : Tile
    {
        private enum TriggerState
        {
            disabled,
            enabled,
            triggered,
        };
        private TriggerState m_state;
        private ParticleSystem m_particleSystem;

        protected override void Awake() {
            m_particleSystem = GetComponentInChildren<ParticleSystem>();
            Assert.IsTrue(m_particleSystem);

            m_tileAnimationCode = -2; // Used by parent Awake
            base.Awake();
            m_state = TriggerState.disabled;
            m_type = TileType.trigger;
            m_text.text = "B";
        }

        public void EnableTrigger() {
            Assert.IsFalse(m_state == TriggerState.triggered); // Queue cannot be loaded after triggered
            m_state = TriggerState.enabled;
            m_particleSystem.Play();

            UpdateTile();
        }

        public void DisableTrigger() {
            Assert.IsFalse(m_state == TriggerState.triggered); // Queue cannot be unloaded after triggered
            m_state = TriggerState.disabled;
            m_particleSystem.Stop();

            UpdateTile();
        }

        public override void UpdateTile()
        {
            if (PlayerMgr.Instance.transform.position == transform.position)
            {
                if (m_state == TriggerState.enabled)
                {
                    m_state = TriggerState.triggered;
                    PuzzleMgr.Instance.StartCountdown();
                    m_particleSystem.Stop();
                    AudioMgr.Instance.Play("Queue loaded");
                }
            }
        }
    }
}
