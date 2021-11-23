using RPG.Quests;
using System.Collections;
using UnityEngine;
using TMPro;

namespace RPG.UI.Quests
{
	public class QuestTooltipUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI title;
		[SerializeField] Transform objectiveContainer;
		[SerializeField] GameObject objectivePrefab;
		public void Setup(Quest quest)
		{
			title.text = quest.GetTitle();
			objectiveContainer.DetachChildren();
			foreach (string objective in quest.GetObjectives())
			{
				GameObject objecteInstance= Instantiate(objectivePrefab, objectiveContainer);
				TextMeshProUGUI objectiveText = objecteInstance.GetComponentInChildren<TextMeshProUGUI>();
				objectiveText.text = objective;
			}
		}
		
	}
}