using System;
using UnityEngine;

namespace GOD
{
    public class SceneMgrWrapper : MonoBehaviour
    {
        public void RestartScene()
        {
            SceneMgr.RestartScene();
        }

        public void TransitionToScene(string sceneName)
        {
            SceneMgr.TransitionToScene(sceneName);
        }

        public void RestartSceneWithDelay(float delay)
        {
            SceneMgr.RestartSceneWithDelay(delay);
        }
    }
}