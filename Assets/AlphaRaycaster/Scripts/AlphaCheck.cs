using UnityEngine;

[AddComponentMenu("Event/Alpha Check"), ExecuteInEditMode]
public class AlphaCheck : MonoBehaviour 
{
	[Range(0, 1), Tooltip("Below that value of alpha this component won't react to raycast.")]
	public float AlphaThreshold = .9f;
	[Tooltip("Include material tint color when checking alpha.")]
	public bool IncludeMaterialAlpha;

	private void OnEnable ()
	{
		if (!FindObjectOfType<AlphaRaycaster>())
		{
			var canvas = FindObjectOfType<Canvas>();
			if (!canvas) return;
			var alphaCaster = canvas.gameObject.AddComponent<AlphaRaycaster>();
			alphaCaster.SelectiveMode = true;
		}
	}
}
