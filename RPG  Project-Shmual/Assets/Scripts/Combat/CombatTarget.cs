using RPG.Attributes;
using RPG.Control;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
	[RequireComponent(typeof(Health))]
	public class CombatTarget : MonoBehaviour, IRaycastable
	{
		public CursorType GetCoursorType()
		{
			return CursorType.Combat;
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) return false;

			if (Input.GetMouseButton(0))
			{
				callingController.GetComponent<Fighter>().Attack(gameObject);
			}
			return true;
		}
	}
}