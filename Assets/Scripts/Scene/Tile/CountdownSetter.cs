using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;

namespace GOD
{
    [ExecuteInEditMode]
    public class CountdownSetter : MonoBehaviour
    {
        protected TMP_Text m_text;
        protected CountdownTile m_countdownTile;

        private void Awake()
        {
            var canvas = GetComponentInChildren<Canvas>();
            Assert.IsTrue(canvas);
            m_text = canvas.GetComponentInChildren<TMP_Text>();
            Assert.IsTrue(m_text);

            m_countdownTile = GetComponent<CountdownTile>();
            Assert.IsTrue(m_countdownTile);
        }

        private void Update()
        {
            m_text.text = m_countdownTile.m_counter.ToString();
        }
    }

}