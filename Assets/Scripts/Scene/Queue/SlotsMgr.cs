using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace GOD
{
	public class SlotsMgr<T> : Singleton<T> where T : MonoBehaviour
	{
		protected List<Slot> m_Slots;
		protected Slot m_activeSlot;

		protected override void Awake()
		{
			base.Awake();

			m_Slots = new List<Slot>(GetComponentsInChildren<Slot>());
			Assert.IsTrue(m_Slots.Count > 0);
			m_activeSlot = m_Slots[0];
		}

		public bool Contains(Slot slot)
		{
			return m_Slots.Contains(slot);
		}

		public List<Slot> Get()
		{
			return m_Slots;
		}

		public Slot GetActive()
		{
			return m_activeSlot;
		}

		public void SetActive(Slot slot)
		{
			if (m_activeSlot != slot && Contains(slot))
			{
				m_activeSlot = slot;
			}
		}

		public virtual void Enter()
		{
			EnableInput();

			m_activeSlot.Enable();
			m_activeSlot.Select();
		}

		public virtual void Update()
		{
			EventSystem current = UnityEngine.EventSystems.EventSystem.current;

			foreach (Slot slot in m_Slots)
			{
				if (current.currentSelectedGameObject == slot.gameObject)
				{
					if (m_activeSlot != slot)
					{
						m_activeSlot = slot;
						m_activeSlot.Select();
					}
					break;
				}
			}
		}

		public virtual void Exit()
		{
			DisableInput();
		}

		public void SetActive(bool active)
		{
			this.gameObject.SetActive(active);
		}

		public void EnableInput()
		{
			m_Slots.ForEach((Slot slot) => slot.EnableInput());
		}

		public void DisableInput()
		{
			m_Slots.ForEach((Slot slot) => slot.DisableInput());
		}
	}
}