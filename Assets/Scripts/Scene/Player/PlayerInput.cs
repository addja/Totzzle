using UnityEngine;

namespace GOD
{
    public class PlayerInput : InputComponent
    {
        public static PlayerInput Instance
        {
            get { return s_Instance; }
        }

        protected static PlayerInput s_Instance;

        public bool HaveControl { get { return m_HaveControl; } }

        public InputAxis Horizontal = new InputAxis(KeyCode.D, KeyCode.A, XboxControllerAxes.LeftstickHorizontal);
        public InputAxis Vertical = new InputAxis(KeyCode.W, KeyCode.S, XboxControllerAxes.LeftstickVertical);
        [HideInInspector]

        protected bool m_HaveControl = true;

        protected bool m_DebugMenuIsOpen = false;

        void Awake ()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnEnable()
        {
            if (s_Instance == null)
                s_Instance = this;
            else if(s_Instance != this)
                throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
        }

        void OnDisable()
        {
            s_Instance = null;
        }

        protected override void GetInputs(bool fixedUpdateHappened)
        {
            Horizontal.Get(inputType);
            Vertical.Get(inputType);

            if (Input.GetKeyDown(KeyCode.F12))
            {
                m_DebugMenuIsOpen = !m_DebugMenuIsOpen;
            }
        }

        public override void GainControl()
        {
            m_HaveControl = true;

            GainControl(Horizontal);
            GainControl(Vertical);
        }

        public override void ReleaseControl(bool resetValues = true)
        {
            m_HaveControl = false;

            ReleaseControl(Horizontal, resetValues);
            ReleaseControl(Vertical, resetValues);
        }

        void OnGUI()
        {
            if (m_DebugMenuIsOpen)
            {
                const float height = 100;

                GUILayout.BeginArea(new Rect(30, Screen.height - height, 200, height));

                GUILayout.BeginVertical("box");
                GUILayout.Label("Press F12 to close");

                SetEnabled(Horizontal, GUILayout.Toggle(Horizontal.Enabled, "Enable horizontal movement"));
                SetEnabled(Vertical, GUILayout.Toggle(Vertical.Enabled, "Enable vertical movement"));

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
