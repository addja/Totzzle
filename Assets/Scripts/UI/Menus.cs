﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class Menus : MonoBehaviour
    {
        public void ExitPause()
        {
            PuzzleMgr.Instance.Unpause();
        }

        public void RestartLevel()
        {
            PuzzleMgr.Instance.Reset();
        }
    }
}