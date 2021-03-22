using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace GOD
{
	public class OptionSlot : Slot
	{
		protected ContainerSlot m_container;

		protected override void Awake()
		{
			base.Awake();

			m_container = null;
		}

		public ContainerSlot GetContainer()
		{
			return m_container;
		}

		public void SetContainer(ContainerSlot container)
		{
			if (m_container != container)
			{
				m_container = container;
			}
		}

		public bool HasContainer()
		{
			return (GetContainer() != null);
		}

		protected override void OnClick()
		{
			ContainerMgr	containerMgr	= ContainerMgr.Instance;
			ContainerSlot	container		= containerMgr.GetActiveContainer();
			OptionSlot		option			= container.GetOption();

			if (option != null)
			{
				option.SetContainer(null);
				option.Enable();
			}

			Disable();
			SetContainer(container);
			container.SetOption(this);
			containerMgr.SetActive(container);
			HUDMgr.Instance.SetState(HUDMgr.State.queue);
		}
	}
}