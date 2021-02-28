using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Autoselection : MonoBehaviour
{
    public GameObject Object;
    private GameObject LastSelectedObject;

    void OnEnable()
    {
        Button Button = Object.GetComponent<Button>();

        if (Button == null)
        {
            Button = Object.GetComponentInChildren<Button>();
        }

        Button.Select();
        Button.OnSelect(null);

        LastSelectedObject = Object;
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
