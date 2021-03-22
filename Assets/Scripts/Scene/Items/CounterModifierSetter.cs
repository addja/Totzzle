using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

namespace GOD
{
    [ExecuteInEditMode]
    public class CounterModifierSetter : MonoBehaviour
    {
        protected TMP_Text m_text;
        protected CounterModifier m_counterModifier;

        private void Awake()
        {
            var canvas = GetComponentInChildren<Canvas>();
            Assert.IsTrue(canvas);
            m_text = canvas.GetComponentInChildren<TMP_Text>();
            Assert.IsTrue(m_text);

            m_counterModifier = GetComponent<CounterModifier>();
            Assert.IsTrue(m_counterModifier);
        }

        private void Update()
        {
            m_text.text = "+" + m_counterModifier.m_value.ToString();
        }
    }

}