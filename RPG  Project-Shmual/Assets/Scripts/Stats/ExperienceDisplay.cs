using System.Collections;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
	public class ExperienceDisplay : MonoBehaviour
	{
		Experience experience;
		TextMeshProUGUI experienceText;
		
		private void Awake()
		{
			experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
			experienceText = GetComponent<TextMeshProUGUI>();
		}
		private void Update()
		{
			if (experienceText != null)
			{
				experienceText.text = string.Format("{0:0}", experience.GetPoints());
			}
		}
	}
}