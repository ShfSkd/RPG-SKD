using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
	public class PlayerConversant : MonoBehaviour
	{
		[SerializeField] string playerName;
		Dialogue currentDialogue;
		DialogueNode currentNode = null;
		AIConversant currentConversant = null;
		bool isChoosing;

		public event Action onConversationUpdate;

		public void StartDialogue(AIConversant newConversant,Dialogue newDialogue)
		{
			currentConversant = newConversant;
			currentDialogue = newDialogue;
			currentNode = currentDialogue.GetRootNode();
			TriggerEnterAction();
			onConversationUpdate();
		}
		public void Quit()
		{
			currentDialogue = null;
			TriggerExitAction();
			currentNode = null;
			isChoosing = false;
			currentConversant = null;

			onConversationUpdate();
		}
		public bool IsActive()
		{
			return currentDialogue != null;
		}
		public bool IsChoosing()
		{
			return isChoosing;
		}
		public string GetText()
		{
			if (currentNode == null) return "";


			return currentNode.GetText();
		}

		public string GetCurrentConversantName()
		{
			if (isChoosing)
			{
				return playerName;
			}
			else
			{
				return currentConversant.GetAIName();
			}
		}

		public IEnumerable<DialogueNode> GetChoices()
		{
			return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));

		}
		public void SelectChoice(DialogueNode chosenNode)
		{
			currentNode = chosenNode;
			TriggerEnterAction();
			isChoosing = false;
			Next();
		}
		public void Next()
		{
			int numPlayerResponses= FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
			if (numPlayerResponses > 0)
			{
				isChoosing = true;
				TriggerExitAction();
				onConversationUpdate();
				return;
			}
			DialogueNode[]children= FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
			int randomIndex = UnityEngine.Random.Range(0, children.Count());
			TriggerExitAction();
			currentNode = children[randomIndex];
			TriggerEnterAction();
			onConversationUpdate();
		}
		public bool HasNext() 
		{
			return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
		}
		IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
		{
			foreach (var node in inputNode)
			{
				if (node.CheckCondition(GetEvaluators()))
				{
					yield return node;
				}
			}
		}

		IEnumerable<IPredicateEvaluator> GetEvaluators()
		{
			return GetComponents<IPredicateEvaluator>();
		}

		void TriggerEnterAction()
		{
			if (currentNode != null)
			{
				TriggerAction(currentNode.GetOnEnterAction());
			}
		}
		void TriggerExitAction()
		{
			if (currentNode != null )
			{
				TriggerAction(currentNode.GetOnExitAction());
			}
		}
		void TriggerAction(string action)
		{
			if (action == "") return;

			foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
			{
				trigger.Trigger(action);
			}
		}
	}
}