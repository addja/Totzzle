using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class Menus : MonoBehaviour
    {
        public void ExitPause()
        {
            MapMgr.Instance.Unpause();
        }

        public void RestartLevel()
        {
            ExitPause();
            SceneMgr.RestartScene();
        }
    }
}