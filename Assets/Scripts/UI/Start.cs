using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GOD
{
    public class Start : MonoBehaviour {

        public void Quit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
		    Application.Quit();
#endif
        }
    }
}
