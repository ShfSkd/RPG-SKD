using RPG.Quests;
using System.Collections;
using UnityEngine;
using TMPro;
using System;

namespace RPG.UI.Quests
{
	public class QuestItemUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI title;
		[SerializeField] TextMeshProUGUI progress;

		QuestStatus status;
		public void Setup(QuestStatus status)
		{
			this.status = status;
			title.text = status.GetQuest().GetTitle();
			progress.text = status.GetCompleteobjectives() + "/" + status.GetQuest().GetObjectivesCount();
		}
		public QuestStatus GetQuestStatus()
		{
			return status;
		}

	}
}