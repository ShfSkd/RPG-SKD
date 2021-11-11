using System.Collections;
using UnityEngine;

namespace RPG.Attributes
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] Health healthComponent = null;
		[SerializeField] RectTransform forground = null;
		[SerializeField] Canvas canvasRoot = null;
		private void Update()
		{
			if (Mathf.Approximately(healthComponent.GetFraction(), 0)|| Mathf.Approximately(healthComponent.GetFraction(), 1))
			{
				canvasRoot.enabled = false;
				return;
			}
			canvasRoot.enabled = true;
			forground.localScale = new Vector3(healthComponent.GetFraction(), 1, 1);

		}

	}
}