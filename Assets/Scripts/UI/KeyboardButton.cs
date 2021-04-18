using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GOD
{
    // Base class for keyboard buttons.
    // Provides all the necessary APIs to be implemented by descendants

    public class KeyboardButton : MonoBehaviour
    {
        public enum KeyboardButtonState
        {
            enabled,
            disabled,
            selected,
            highlighted,
        }

        protected KeyboardButtonState m_state = KeyboardButtonState.disabled;
        protected Animator m_animator;
        private Dictionary<KeyboardButtonState, int> m_animatorValue = new Dictionary<KeyboardButtonState, int> {
            {KeyboardButtonState.disabled, 0 },
            {KeyboardButtonState.enabled, 1 },
            {KeyboardButtonState.selected, 2 },
            {KeyboardButtonState.highlighted, 3 },
        };
        protected TMPro.TMP_Text m_text;
        public int m_value;
       
        private void Awake() {
            m_animator = GetComponent<Animator>();
            Assert.IsTrue(m_animator);

            m_text = GetComponentInChildren<TMPro.TMP_Text>();
            Assert.IsTrue(m_text);
        }

        private void UpdateAnimator() {
            m_animator.SetInteger("state", m_animatorValue[m_state]);
        }

        public void Enable() {
            m_state = KeyboardButtonState.enabled;
            UpdateAnimator();
        }

        public void Disable() {
            m_state = KeyboardButtonState.disabled;
            UpdateAnimator();
        }

        public void Select()
        {
            m_state = KeyboardButtonState.selected;
            UpdateAnimator();
        }

        public void Highlight() {
            m_state = KeyboardButtonState.highlighted;
            UpdateAnimator();
        }

        public void SetText(string text)
        {
            m_text.text = text;
        }
    }
}