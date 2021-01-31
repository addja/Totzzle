using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class MenusUI : MonoBehaviour
    {
        public void ExitPause()
        {
            PlayerCharacter.Instance.Unpause();
        }

        public void RestartLevel()
        {
            ExitPause();
            SceneController.RestartScene();
        }
    }
}