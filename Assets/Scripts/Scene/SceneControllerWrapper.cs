using System;
using UnityEngine;

namespace GOD
{
    public class SceneControllerWrapper : MonoBehaviour
    {
        public void RestartScene()
        {
            SceneController.RestartScene();
        }

        public void TransitionToScene(string sceneName)
        {
            SceneController.TransitionToScene(sceneName);
        }

        public void RestartSceneWithDelay(float delay)
        {
            SceneController.RestartSceneWithDelay(delay);
        }
    }
}