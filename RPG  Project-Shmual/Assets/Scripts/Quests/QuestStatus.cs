using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
	public class QuestStatus 
	{
		 Quest quest;
		List<string> completedObjectives = new List<string>();

		[Serializable]
		class QuestsStatusRecord
		{
			public string questName;
			public List<string> completedObjective;
		}
		public QuestStatus(Quest quest)
		{
			this.quest = quest;
		}

		public Quest GetQuest()
		{
			return quest;
		}
		public int GetCompleteobjectives()
		{
			return completedObjectives.Count;
		}
		public bool IsObjectiveComplete(string objective)
		{
			return completedObjectives.Contains(objective);
		}

		public void CompleteObjective(string objective)
		{
			if (quest.HasObjective(objective))
				completedObjectives.Add(objective);
		}

		public object CaptureState()
		{
			QuestsStatusRecord state = new QuestsStatusRecord();
			state.questName = quest.name;
			state.completedObjective = completedObjectives;

			return state;
		}
		public QuestStatus(object objectState)
		{
			QuestsStatusRecord state = objectState as QuestsStatusRecord;
			quest = Quest.GetByName(state.questName);
			completedObjectives=state.completedObjective;
		}
	}
}