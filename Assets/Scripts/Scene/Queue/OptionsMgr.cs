using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace GOD
{
	public class OptionsMgr : SlotsMgr<OptionsMgr>
	{
		public OptionSlot GetActiveOption()
		{
			return (OptionSlot)m_activeSlot;
		}

		public List<OptionSlot> GetOptions()
		{
			return m_Slots.ConvertAll<OptionSlot>((Slot slot) => (OptionSlot)slot);
		}

		public bool HasNoContainerOptions()
		{
			bool				noContainerOptions	= false;
			List<OptionSlot>	options				= GetOptions();

			foreach (OptionSlot option in options)
			{
				if (option.HasContainer())
				{
					noContainerOptions = true;
					break;
				}
			}

			return noContainerOptions;
		}

		public OptionSlot GetNoContainerOption()
		{
			OptionSlot noContainerOption = GetActiveOption();

			if (noContainerOption.HasContainer())
			{
				List<OptionSlot>	options		= GetOptions();
				List<OptionSlot>	backside	= new List<OptionSlot>();
				List<OptionSlot>	frontside	= new List<OptionSlot>();
				bool				back		= true;

				foreach (OptionSlot option in options)
				{
					if (noContainerOption == option)
					{
						back = false;
					}
					else if (back)
					{
						backside.Add(option);
					}
					else
					{
						frontside.Add(option);
					}
				}

				noContainerOption = null;

				foreach (OptionSlot option in frontside)
				{
					if (!option.HasContainer())
					{
						noContainerOption = option;
						break;
					}
				}

				if (noContainerOption == null)
				{
					foreach (OptionSlot option in backside)
					{
						if (!option.HasContainer())
						{
							noContainerOption = option;
							break;
						}
					}
				}
			}

			return noContainerOption;
		}
	}
}