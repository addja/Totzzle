using UnityEngine;

/// <summary>
/// Inherit from this base class to create a persistentsingleton.
/// E.g.: public class MyClassName : Singleton<MyClassName> {}
/// This script will not prevent non singleton constructors
/// from being used in your derived classes. To prevent this,
/// add a protected constructor to each derived class.
/// </summary>
public class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
			DontDestroyOnLoad(m_Instance.gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}