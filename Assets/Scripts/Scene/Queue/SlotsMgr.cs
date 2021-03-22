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
		public enum Direction
		{
			right,
			left
		}

		public Direction m_direction = Direction.right;
		protected List<Slot> m_Slots;
		protected Slot m_activeSlot;

		protected override void Awake()
		{
			base.Awake();

			m_Slots = new List<Slot>(GetComponentsInChildren<Slot>());
			Assert.IsTrue(m_Slots.Count > 0);
			ResetActive();
		}

		public List<Slot> Get()
		{
			return m_Slots;
		}

		public bool Contains(Slot slot)
		{
			return m_Slots.Contains(slot);
		}

		public bool TrueForAll(System.Predicate<Slot> match)
		{
			return m_Slots.TrueForAll(match);
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

		public void ResetActive()
		{
			SetActive(m_Slots[0]);
		}

		public void PreviousActive()
		{
			SetActive(GetPreviousSlot(m_activeSlot));
		}

		public void NextActive()
		{
			SetActive(GetNextSlot(m_activeSlot));
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

			m_activeSlot.Unselect();
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

		protected Direction Inverse(Direction direction)
		{
			switch (direction)
			{
				case Direction.right:
				{
					direction = Direction.left;
				}
				break;

				case Direction.left:
				{
					direction = Direction.right;
				}
				break;
			}

			return direction;
		}

		protected Slot GetNextSlot(Slot slot)
		{
			return GetSlot(slot, m_direction);
		}

		protected Slot GetPreviousSlot(Slot slot)
		{
			return GetSlot(slot, Inverse(m_direction));
		}

		protected Slot GetSlot(Slot start, Direction direction)
		{
			Slot		candidate	= start;
			List<Slot>	backside	= new List<Slot>();
			List<Slot>	frontside	= new List<Slot>();
			bool		back		= true;

			foreach (Slot slot in m_Slots)
			{
				if (candidate == slot)
				{
					back = false;
				}
				else if (back)
				{
					backside.Add(slot);
				}
				else
				{
					frontside.Add(slot);
				}
			}

			switch (m_direction)
			{
				case Direction.left:
				{
					frontside.Reverse();
					backside.Reverse();

					List<Slot> temporal = new List<Slot>(frontside);
					frontside = backside;
					backside = temporal;
				}
				break;
			}

			if (frontside.Count > 0)
			{
				candidate = frontside[0];
			}
			else if (backside.Count > 0)
			{
				candidate = backside[0];
			}

			return candidate;
		}
	}
}