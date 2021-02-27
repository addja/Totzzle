using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Autoselection : MonoBehaviour
{
    public GameObject Object;

    void OnEnable()
    {
        Button Button = Object.GetComponent<Button>();

        if (Button == null)
        {
            Button = Object.GetComponentInChildren<Button>();
        }

        Button.Select();
        Button.OnSelect(null);
    }
}
