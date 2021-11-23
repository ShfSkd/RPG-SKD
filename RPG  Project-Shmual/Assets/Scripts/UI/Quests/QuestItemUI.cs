using RPG.Quests;
using System.Collections;
using UnityEngine;
using TMPro;

namespace RPG.UI.Quests
{
	public class QuestItemUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI title;
		[SerializeField] TextMeshProUGUI progress;

		Quest quest;
		public void Setup(Quest quest)
		{
			this.quest = quest;
			title.text = quest.GetTitle();
			progress.text = "0/" + quest.GetObjectivesCount();
		}
		public Quest GetQuest()
		{
			return quest;
		}
	}
}