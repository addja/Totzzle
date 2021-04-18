using UnityEngine;

/// <summary>
/// Enables the attached GameObject only in the Editor.
/// </summary>
[ExecuteInEditMode]
public class EnableInEditor : MonoBehaviour
{
	private void Awake()
	{
		Update();
	}

	private void Start()
	{
		Update();
	}

	private void Update()
	{
#if UNITY_EDITOR
		this.gameObject.SetActive(true);
#else
		this.gameObject.SetActive(false);
#endif
	}
}