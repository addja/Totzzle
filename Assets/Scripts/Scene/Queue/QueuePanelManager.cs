using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePanelManager : MonoBehaviour
{
    // Manages which queue panel is displayed (compressed or expanded)
    public GameObject queueCompressedPanel;
    public GameObject queueExpandedPanel;

    private bool queueExpanded = false;

    public void ToggleQueueExpanded()
    {
        queueExpanded = !queueExpanded;
        if (queueExpanded)
        {
            queueExpandedPanel.SetActive(false);
            queueCompressedPanel.SetActive(true);
        }
        else
        {
            queueCompressedPanel.SetActive(false);
            queueExpandedPanel.SetActive(true);

        }
    }
}
