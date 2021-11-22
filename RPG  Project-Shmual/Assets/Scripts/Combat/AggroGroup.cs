using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
	public class AggroGroup : MonoBehaviour
	{
		[SerializeField] Fighter[] figthers;
		[SerializeField] bool activateOnStart;
		private void Start()
		{
			Activate(activateOnStart);
		}
		public void Activate(bool shouldActivate)
		{
			foreach (Fighter fighter in figthers)
			{
				CombatTarget combatTarget = fighter.GetComponent<CombatTarget>();
				if (combatTarget != null)
				{
					combatTarget.enabled = shouldActivate;
				}
				fighter.enabled = shouldActivate;
			}
		}
	}
}