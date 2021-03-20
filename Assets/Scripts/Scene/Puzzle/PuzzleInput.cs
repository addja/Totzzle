using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOD
{
    public class PuzzleInput : InputComponent
    {
        // BEGIN Singleton stuff
        public static PuzzleInput Instance
        {
            get { return s_Instance; }
        }

        protected static PuzzleInput s_Instance;

        void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one PuzzleInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if (s_Instance != this)
                throw new UnityException("There cannot be more than one PuzzleInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }
        // END Singleton stuff

        public bool HaveControl { get { return m_HaveControl; } }

        public InputButton Pause = new InputButton(KeyCode.Escape, XboxControllerButtons.Menu);
        public InputButton QueueEditor = new InputButton(KeyCode.Tab, XboxControllerButtons.View);
        [HideInInspector]

        protected bool m_HaveControl = true;
        protected bool m_DebugMenuIsOpen = false;

        protected override void GetInputs(bool fixedUpdateHappened)
        {
            Pause.Get(fixedUpdateHappened, inputType);
            QueueEditor.Get(fixedUpdateHappened, inputType);

            if (Input.GetKeyDown(KeyCode.F12))
            {
                m_DebugMenuIsOpen = !m_DebugMenuIsOpen;
            }
        }

        public override void GainControl()
        {
            m_HaveControl = true;

            GainControl(Pause);
            GainControl(QueueEditor);
        }

        public override void ReleaseControl(bool resetValues = true)
        {
            m_HaveControl = false;

            ReleaseControl(Pause, resetValues);
            ReleaseControl(QueueEditor, resetValues);
        }

        void OnGUI()
        {
            if (m_DebugMenuIsOpen)
            {
                const float height = 100;

                GUILayout.BeginArea(new Rect(30, Screen.height - height, 200, height));

                GUILayout.BeginVertical("box");
                GUILayout.Label("Press F12 to close");

                SetEnabled(Pause, GUILayout.Toggle(Pause.Enabled, "Enable pause"));
                SetEnabled(QueueEditor, GUILayout.Toggle(QueueEditor.Enabled, "Enable queue editor"));

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        void SetEnabled(InputButton input, bool enabled)
        {
            if (input.Enabled != enabled)
            {
                if (enabled)
                {
                    input.Enable();
                }
                else
                {
                    input.Disable();
                }
            }
        }

        void SetEnabled(InputAxis input, bool enabled)
        {
            if (input.Enabled != enabled)
            {
                if (enabled)
                {
                    input.Enable();
                }
                else
                {
                    input.Disable();
                }
            }
        }
    }
}