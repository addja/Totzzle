using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace GOD
{
	[ExecuteInEditMode]
	public class Slot : MonoBehaviour
	{
		protected TMPro.TMP_Text m_text;
		protected Button m_button;
		protected bool m_enabled;
		protected bool m_inputEnabled;
		static protected Autoselection m_autoselection;

		public int m_value;

		protected virtual void Awake()
		{
			m_text = GetComponentInChildren<TMPro.TMP_Text>();
			Assert.IsTrue(m_text);
			m_button = GetComponent<Button>();

			if (m_button == null)
			{
				m_button = GetComponentInChildren<Button>();
			}

			Assert.IsTrue(m_button);
			m_button.onClick.AddListener(OnClick);
			m_enabled		= true;
			m_inputEnabled	= false;
			UpdateInteractable();
		}

		public virtual int GetValue()
		{
			return m_value;
		}

		public void Enable()
		{
			m_enabled = true;
			UpdateInteractable();
		}

		protected virtual void Update()
		{
			UpdateInteractable();
			SetText(GetValue().ToString());
		}

		public void Disable()
		{
			m_enabled = false;
			UpdateInteractable();
		}

		public bool IsEnabled()
		{
			return m_enabled;
		}

		public void Select()
		{
			if (m_autoselection != null)
			{
				Destroy(m_autoselection);
			}

			m_autoselection = this.gameObject.AddComponent<Autoselection>();
		}

		public void Unselect()
		{
			if (IsSelected())
			{
				Destroy(m_autoselection);
			}
		}

		public bool IsSelected()
		{
			return (m_autoselection != null && m_autoselection.gameObject == this.gameObject);
		}

		public bool IsInteractable()
		{
			return m_button.interactable;
		}

		public void EnableInput()
		{
			m_inputEnabled = true;
			UpdateInteractable();
		}

		public void DisableInput()
		{
			m_inputEnabled = false;
			UpdateInteractable();
		}

		protected void SetText(string text)
		{
			m_text.text = text;
		}

		protected void UpdateInteractable()
		{
			bool Interactable = ((m_enabled && m_inputEnabled) || IsSelected());

			if (m_button.interactable != Interactable)
			{
				m_button.interactable = Interactable;

				if (Interactable)
				{
					if (IsSelected())
					{
						Select();
					}
					else
					{
						Unselect();
					}
				}
			}
		}

		protected virtual void OnClick()
		{
		}
	}
}