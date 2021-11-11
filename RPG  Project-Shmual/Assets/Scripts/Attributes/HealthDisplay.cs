using System.Collections;
using UnityEngine;
using TMPro;

namespace RPG.Attributes
{
	public class HealthDisplay : MonoBehaviour
	{
		Health health;
		TextMeshProUGUI healthText;

		private void Awake()
		{
			health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
			healthText = GetComponent<TextMeshProUGUI>();
		}
		private void Update()
		{
			if (healthText != null)
			{
				healthText.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
			}
		}
	}
}