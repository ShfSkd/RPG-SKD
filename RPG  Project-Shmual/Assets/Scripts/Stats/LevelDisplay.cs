using System.Collections;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
	public class LevelDisplay : MonoBehaviour
	{
		BaseStats baseStats;
		TextMeshProUGUI experienceText;

		private void Awake()
		{
			baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
			experienceText = GetComponent<TextMeshProUGUI>();
		}
		private void Update()
		{
			if (experienceText != null)
			{
				experienceText.text = string.Format("{0:0}", baseStats.GetLevel());
			}
		}
	}
}