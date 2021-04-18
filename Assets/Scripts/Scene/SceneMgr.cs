using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GOD
{
    /// <summary>
    /// This class is used to transition between scenes. This includes triggering all the things that need to happen on transition such as data persistence.
    /// </summary>
    public class SceneMgr : MonoBehaviour
    {
        public static SceneMgr Instance
        {
            get
            {
                if (instance != null)
                    return instance;

                instance = FindObjectOfType<SceneMgr>();

                if (instance != null)
                    return instance;

                Create ();

                return instance;
            }
        }

        public static bool Transitioning
        {
            get { return Instance.m_Transitioning; }
        }

        protected static SceneMgr instance;

        public static SceneMgr Create ()
        {
            GameObject SceneMgrGameObject = new GameObject("SceneMgr");
            instance = SceneMgrGameObject.AddComponent<SceneMgr>();

            return instance;
        }

        protected Scene m_CurrentScene;
        protected PlayerInput m_PlayerInput;
        protected bool m_Transitioning;

        void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            m_PlayerInput = FindObjectOfType<PlayerInput>();
            m_CurrentScene = SceneManager.GetActiveScene();
        }

        public static void RestartScene()
        {
            Instance.StartCoroutine(Instance.Transition(Instance.m_CurrentScene.name));
        }

        public static void RestartSceneWithDelay(float delay)
        {
            Instance.StartCoroutine(CallWithDelay(delay, RestartScene));
        }

        public static void TransitionToScene(string sceneName)
        {
            Instance.StartCoroutine(Instance.Transition(sceneName));
        }

        protected IEnumerator Transition(string newSceneName)
        {
            bool resetInputValues = true;

            m_Transitioning = true;

            if (m_PlayerInput == null)
                m_PlayerInput = FindObjectOfType<PlayerInput>();
            m_PlayerInput.ReleaseControl(resetInputValues);
            //yield return StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.Loading));
            yield return SceneManager.LoadSceneAsync(newSceneName);
            m_PlayerInput = FindObjectOfType<PlayerInput>();
            m_PlayerInput.ReleaseControl(resetInputValues);
            m_CurrentScene = SceneManager.GetSceneByName(newSceneName);
            //yield return StartCoroutine(ScreenFader.FadeSceneIn());
            m_PlayerInput.GainControl();

            m_Transitioning = false;
        }

        static IEnumerator CallWithDelay(float delay, Action call)
        {
            yield return new WaitForSeconds(delay);
            call();
        }
    }
}