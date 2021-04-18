using UnityEngine;
 
/// <summary>
/// Inherit from this base class to create a singleton.
/// E.g.: public class MyClassName : Singleton<MyClassName> {}
/// This script will not prevent non singleton constructors
/// from being used in your derived classes. To prevent this,
/// add a protected constructor to each derived class.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static object m_Lock = new object();
	private static T m_Instance;

	/// <summary>
	/// Access singleton instance through this property.
	/// </summary>
	public static T Instance
	{
		get
		{
			lock (m_Lock)
			{
				return (T)FindObjectOfType(typeof(T));
			}
		}
	}

	protected virtual void Awake()
	{
		if (m_Instance == null)
		{
			// Search for existing instance.
			m_Instance = (T)FindObjectOfType(typeof(T));
		}
		else
		{
			throw new UnityException("There cannot be more than one '" + typeof(T) +
				"' script.  The instances are " + m_Instance.name + " and " + name + ".");
		}
	}

	protected virtual void OnEnable()
	{
		if (m_Instance == null)
		{
			// Search for existing instance.
			m_Instance = (T)FindObjectOfType(typeof(T));
		}
		else if (m_Instance != this)
		{
			throw new UnityException("There cannot be more than one '" + typeof(T) +
				"' script.  The instances are " + m_Instance.name + " and " + name + ".");
		}
	}

	protected virtual void OnDisable()
	{
		m_Instance = null;
	}
}