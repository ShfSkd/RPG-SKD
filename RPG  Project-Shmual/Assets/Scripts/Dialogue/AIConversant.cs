using RPG.Attributes;
using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Dialogue
{
	public class AIConversant : MonoBehaviour, IRaycastable
	{
		[SerializeField] Dialogue dialogue = null;
		[SerializeField] string aIName;

		public string GetAIName()
		{
			return aIName;
		}
		public CursorType GetCoursorType()
		{
			return CursorType.Dialogue;
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (dialogue == null) return false;

			Health health = GetComponent<Health>();
			if (health && health.IsDead()) return false;

			if (Input.GetMouseButtonDown(0))
			{
				callingController.GetComponent<PlayerConversant>().StartDialogue(this, dialogue);
			}
			return true;
		}
	}
}