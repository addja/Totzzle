using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

namespace GOD
{
    public class Tile : MonoBehaviour
    {
        public enum TileType
        {
            countdown,
            origin,
            trigger,
        };

        protected Animator m_animator;
        protected TMP_Text m_text;
        public TileType m_type;
        protected int m_tileAnimationCode;

        protected virtual void Awake()
        {
            var canvas = GetComponentInChildren<Canvas>();
            Assert.IsTrue(canvas);
            m_text = canvas.GetComponentInChildren<TMP_Text>();
            Assert.IsTrue(m_text);

            m_animator = GetComponent<Animator>();
            Assert.IsTrue(m_animator);
            AnimateTile(m_tileAnimationCode);
        }

        protected void AnimateTile(int m_typeAnimator)
        {
            m_animator.SetInteger("tileTypeAnimator", m_typeAnimator);
        }

        public virtual void UpdateTile() {}

        public virtual void StartCountdown() {}
    }
}