using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
	public void ButtonSelected()
	{
		Debug.Log("Selected!");
		AudioMgr.Instance.Play("ButtonSelected");
	}

	public void ButtonPressed()
	{
		Debug.Log("Pressed!");
		AudioMgr.Instance.Play("ButtonPressed");
	}
}
