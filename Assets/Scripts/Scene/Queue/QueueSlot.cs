using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOD
{
    public class QueueSlot : MonoBehaviour
    {
        private Image m_image;

        private void Start()
        {
            m_image = GetComponent<Image>();
        }

        public void Activate()
        {
            m_image.color = new Color(1, 0, 0);
        }

        public void Deactivate()
        {
            m_image.color = new Color(0, 0, 0);
        }
    }
}