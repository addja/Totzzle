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

		public bool Contains(OptionSlot container)
		{
			return GetOptions().Contains(container);
		}

		public bool TrueForAll(System.Predicate<OptionSlot> match)
		{
			return GetOptions().TrueForAll(match);
		}

		public bool HasNoContainerOptions()
		{
			return !TrueForAll((OptionSlot option) => option.HasContainer());
		}

		public OptionSlot GetNoContainerOption()
		{
			OptionSlot noContainerOption = GetActiveOption();

			while (noContainerOption.HasContainer())
			{
				noContainerOption = (OptionSlot)GetNextSlot(noContainerOption);

				if (noContainerOption == GetActiveOption())
				{
					noContainerOption = null;
					break;
				}
			}

			return noContainerOption;
		}
	}
}