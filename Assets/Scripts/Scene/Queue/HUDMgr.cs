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
				SetState(current);
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

		public void SetState(State state)
		{
			if (m_state != state)
			{
				switch (m_state)
				{
					case State.idle:
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
						}
					}
					break;

					case State.queue:
					{
						switch (state)
						{
							case State.idle:
							{
								ContainerMgr.Instance.Exit();
							}
							break;

							case State.options:
							{
								ContainerMgr.Instance.Exit();
								OptionsMgr.Instance.Enter();
							}
							break;
						}
					}
					break;

					case State.options:
					{
						switch (state)
						{
							case State.idle:
							{
								OptionsMgr.Instance.Exit();
							}
							break;

							case State.queue:
							{
								OptionsMgr.Instance.Exit();
								ContainerMgr.Instance.Enter();
							}
							break;
						}
					}
					break;
				}

				m_state = state;
			}
		}

		public void EnableInput()
		{
			switch (m_state)
			{
				case State.queue:
					ContainerMgr.Instance.EnableInput();
					break;
				case State.options:
					OptionsMgr.Instance.EnableInput();
					break;
			}
		}

		public void DisableInput()
		{
			switch (m_state)
			{
				case State.queue:
					ContainerMgr.Instance.DisableInput();
					break;
				case State.options:
					OptionsMgr.Instance.DisableInput();
					break;
			}
		}
	}
}