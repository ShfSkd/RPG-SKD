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
		[SerializeField] GameObject objectiveIncompletePrefab;
		public void Setup(QuestStatus status)
		{
			Quest quest = status.GetQuest();
			title.text = quest.GetTitle();
			objectiveContainer.DetachChildren();
			foreach (string objective in quest.GetObjectives())
			{
				GameObject prefab = objectiveIncompletePrefab;
				if (status.IsObjectiveComplete(objective))
				{
					prefab = objectivePrefab; ;
				}
				GameObject objecteInstance= Instantiate(prefab, objectiveContainer);
				TextMeshProUGUI objectiveText = objecteInstance.GetComponentInChildren<TextMeshProUGUI>();
				objectiveText.text = objective;
			}
		}
		
	}
}