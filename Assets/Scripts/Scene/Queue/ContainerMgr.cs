using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace GOD
{
	public class ContainerMgr : SlotsMgr<ContainerMgr>
	{
		public Image enabledBorder;
		public Image disabledBorder;
		public UnityEvent m_containersFilledEvent;
		public UnityEvent m_containersUnfilledEvent;
		private bool m_containersFilled;

		protected override void Awake()
		{
			base.Awake();

			if (m_containersFilledEvent == null)
			{
				m_containersFilledEvent = new UnityEvent();
			}

			if (m_containersUnfilledEvent == null)
			{
				m_containersUnfilledEvent = new UnityEvent();
			}
		}

		public ContainerSlot GetActiveContainer()
		{
			return (ContainerSlot)m_activeSlot;
		}

		public List<ContainerSlot> GetContainers()
		{
			return m_Slots.ConvertAll<ContainerSlot>(
				delegate(Slot slot)
				{
					return (ContainerSlot)slot;
				}
			);
		}

		public bool AreContainersFilled()
		{
			return m_containersFilled;
		}

		public override void Enter()
		{
			base.Enter();

			enabledBorder.enabled	= true;
			disabledBorder.enabled	= false;
		}

		public override void Update()
		{
			base.Update();
			CheckContainers();
		}

		public override void Exit()
		{
			base.Exit();

			enabledBorder.enabled	= false;
			disabledBorder.enabled	= true;
		}

		private void CheckContainers()
		{
			SetContainerFilled(
				GetContainers().TrueForAll(
					delegate(ContainerSlot container)
					{
						return container.HasOption();
					}
				)
			);
		}

		private void SetContainerFilled(bool containerFilled)
		{
			if (m_containersFilled != containerFilled)
			{
				m_containersFilled = containerFilled;

				if (containerFilled)
				{
					m_containersFilledEvent.Invoke();
				}
				else
				{
					m_containersUnfilledEvent.Invoke();
				}
			}
		}
	}
}