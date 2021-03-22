using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GOD
{
	// @todo: There's a need for a QueueMgr to delegate much of this logic
	//	and also research a better practice for a state manager.
	public class HUDMgr : Singleton<HUDMgr>
	{
		public enum State
		{
			idle,
			queue,
			options,
			countdown
		}
		public State m_state;
		public UnityEvent m_queueLoadedEvent;
		public UnityEvent m_queueUnloadedEvent;

		protected override void Awake()
		{
			base.Awake();

			State current = m_state;

			if (current != State.idle)
			{
				m_state = State.idle;

				if (current != State.countdown)
				{
					SetState(current);
				}
			}

			if (m_queueLoadedEvent == null)
			{
				m_queueLoadedEvent = new UnityEvent();
			}

			if (m_queueUnloadedEvent == null)
			{
				m_queueUnloadedEvent = new UnityEvent();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			ContainerMgr containerMgr = ContainerMgr.Instance;
			containerMgr.m_containersFilledEvent.AddListener(() => m_queueLoadedEvent.Invoke());
			containerMgr.m_containersUnfilledEvent.AddListener(() => m_queueUnloadedEvent.Invoke());
		}

		public void UpdateWorld()
		{
			switch (m_state)
			{
				case State.countdown:
				{
					ContainerMgr.Instance.NextActive();
					ContainerMgr.Instance.GetActiveContainer().Select();
					AudioMgr.Instance.Play("Count down");
				}
				break;
			}
		}

		public int GetQueueValue()
		{
			return ContainerMgr.Instance.GetActiveContainer().GetValue();
		}

		public void SetState(State state)
		{
			if (m_state != state)
			{
				ExitState(m_state);
				EnterState(state);

				m_state = state;
			}
		}

		public void EnableInput()
		{
			switch (m_state)
			{
				case State.queue:
				{
					ContainerMgr.Instance.EnableInput();
				}
				break;

				case State.options:
				{
					OptionsMgr.Instance.EnableInput();
				}
				break;
			}
		}

		public void DisableInput()
		{
			switch (m_state)
			{
				case State.queue:
				{
					ContainerMgr.Instance.DisableInput();
				}
				break;

				case State.options:
				{
					OptionsMgr.Instance.DisableInput();
				}
				break;
			}
		}

		private void EnterState(State state)
		{
			switch (state)
			{
				case State.queue:
				{
					ContainerMgr.Instance.Enter();
				}
				break;

				case State.options:
				{
					OptionsMgr.Instance.Enter();
				}
				break;

				case State.countdown:
				{
					OptionsMgr.Instance.SetActive(false);
					ContainerMgr.Instance.ResetActive();
					ContainerMgr.Instance.GetActiveContainer().Select();
				}
				break;
			}
		}

		private void ExitState(State state)
		{
			switch (state)
			{
				case State.queue:
				{
					ContainerMgr.Instance.Exit();
				}
				break;

				case State.options:
				{
					OptionsMgr.Instance.Exit();
				}
				break;

				case State.countdown:
				{
					OptionsMgr.Instance.SetActive(true);
					ContainerMgr.Instance.ResetActive();
				}
				break;
			}
		}
	}
}