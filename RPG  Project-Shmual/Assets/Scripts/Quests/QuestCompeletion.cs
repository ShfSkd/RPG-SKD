using System.Collections;
using UnityEngine;

namespace RPG.Quests
{
	public class QuestCompeletion : MonoBehaviour
	{
		[SerializeField] Quest quest;
		[SerializeField] string objective;

		public void CompleteObjective()
		{
			QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
			questList.CompleteObjective(quest,objective);

		}
	}
}