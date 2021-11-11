using System.Collections;
using UnityEngine;
using TMPro;
using RPG.Attributes;

namespace RPG.Combat
{
	public class EnemyHealthDisplay : MonoBehaviour
	{
		
		Fighter fighter;
		TextMeshProUGUI enemyHealthText;

		private void Awake()
		{
			fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
			enemyHealthText = GetComponent<TextMeshProUGUI>();
		}
		private void Update()
		{
			if (fighter.GetTarget() == null)
			{
				enemyHealthText.text = "N/A";
				return;
			}
			if (enemyHealthText != null)
			{
				Health health = fighter.GetTarget();
				enemyHealthText.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
			}
		}
	}
}