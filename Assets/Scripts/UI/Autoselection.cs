using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class Autoselection : MonoBehaviour
{
	public GameObject Object;
	private GameObject LastSelectedObject;

	void Awake()
	{
		if (Object == null)
		{
			Object = gameObject;
		}

		OnEnable();
	}

	void OnEnable()
	{
		Button Button = Object.GetComponent<Button>();

		if (Button == null)
		{
			Button = Object.GetComponentInChildren<Button>();
		}

		Assert.IsTrue(Button);
		Button.Select();
		Button.OnSelect(null);

		LastSelectedObject = Object;
	}

	void OnDisable()
	{
		if (Object != null)
		{
			Button Button = Object.GetComponent<Button>();

			if (Button == null)
			{
				Button = Object.GetComponentInChildren<Button>();
			}

			Assert.IsTrue(Button);
			Button.OnDeselect(null);

			Object = null;
		}

		LastSelectedObject	= null;
	}

	void Update ()
	{
		EventSystem Current = UnityEngine.EventSystems.EventSystem.current;

		if (Current.currentSelectedGameObject == null)
		{
			Current.SetSelectedGameObject(LastSelectedObject);
		}
		else
		{
			LastSelectedObject = Current.currentSelectedGameObject;
		}
	}
}
