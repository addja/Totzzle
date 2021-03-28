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
            switch (m_state)
            {
                case TriggerState.enabled:
                    if (PlayerMgr.Instance.transform.position == transform.position)

                    {
                        m_state = TriggerState.triggered;
                        PuzzleMgr.Instance.StartCountdown();
                        m_particleSystem.Stop();
                    }
                    break;
                case TriggerState.triggered:
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
