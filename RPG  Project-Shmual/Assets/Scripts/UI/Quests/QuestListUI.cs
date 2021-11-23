using RPG.Quests;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.UI.Quests
{
	public class QuestListUI : MonoBehaviour
	{
		[SerializeField] Quest[] tempQuests;
		[SerializeField] QuestItemUI questPrefab;

		private void Start()
		{
			transform.DetachChildren();
			foreach (Quest quest in tempQuests)
			{
				QuestItemUI itemUI= Instantiate(questPrefab,transform);
				itemUI.Setup(quest);
			}
		}
	}
}