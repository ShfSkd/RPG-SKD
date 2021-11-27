using RPG.Quests;
using System.Collections;
using UnityEngine;
using TMPro;
using System;

namespace RPG.UI.Quests
{
	public class QuestTooltipUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI title;
		[SerializeField] Transform objectiveContainer;
		[SerializeField] GameObject objectivePrefab;
		[SerializeField] GameObject objectiveIncompletePrefab;
		[SerializeField] TextMeshProUGUI rewardText;
		public void Setup(QuestStatus status)
		{
			Quest quest = status.GetQuest();
			title.text = quest.GetTitle();
			foreach (Transform child in objectiveContainer)
			{
				Destroy(child.gameObject);
			}
			foreach (var objective in quest.GetObjectives())
			{
				GameObject prefab = objectiveIncompletePrefab;
				if (status.IsObjectiveComplete(objective.reference))
				{
					prefab = objectivePrefab; ;
				}
				GameObject objecteInstance= Instantiate(prefab, objectiveContainer);
				TextMeshProUGUI objectiveText = objecteInstance.GetComponentInChildren<TextMeshProUGUI>();
				objectiveText.text = objective.descreptions;
			}
			rewardText.text = GetRewardText(quest);
		}

		private string GetRewardText(Quest quest)
		{
			string rewardText = "";

			foreach (var reward in quest.GetReward())
			{
				if (rewardText != "")
				{
					rewardText += ", ";
				}
				if (reward.number > 1)
				{
					rewardText += reward.number + "";
				}
				rewardText += reward.item.GetDisplayName();
			}
			if (rewardText == "")
			{
				rewardText = "No Reward ";
			}
			rewardText += ".";
			return rewardText;
		}
	}
}