using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace GOD
{
	public class ContainerSlot : Slot
	{
		protected OptionSlot m_option;

		protected override void Awake()
		{
			base.Awake();

			m_option = null;
		}

		protected override void Update()
		{
			base.Update();

			SetText(HasOption() ? FormatValue(m_option.GetValue()) : "");
		}

		public override int GetValue()
		{
			return HasOption() ? m_option.GetValue() : m_value;
		}

		public OptionSlot GetOption()
		{
			return m_option;
		}

		public void SetOption(OptionSlot option)
		{
			if (m_option != option)
			{
				m_option = option;
			}
		}

		public bool HasOption()
		{
			return (GetOption() != null);
		}

		protected override void OnClick()
		{
			OptionsMgr optionsMgr = OptionsMgr.Instance;

			if (optionsMgr != null)
			{
				OptionSlot option = optionsMgr.GetActiveOption();

				if (m_option == null)
				{
					if (option.HasContainer() && optionsMgr.HasNoContainerOptions())
					{
						OptionSlot noContainerOption = optionsMgr.GetNoContainerOption();

						if (noContainerOption != null)
						{
							option = noContainerOption;
						}
					}

					if (option.HasContainer())
					{
						option.GetContainer().SetOption(null);
					}
				}
				else
				{
					option = m_option;
				}

				optionsMgr.SetActive(option);
				HUDMgr.Instance.SetState(HUDMgr.State.options);
			}
		}
	}
}