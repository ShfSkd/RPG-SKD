using RPG.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Effect
{
	[CreateAssetMenu(fileName = "Health Effect", menuName = "Abilities/Effects/Health", order = 0)]
	public class HealthEffect : EffectStragtegy
	{
		[SerializeField] float healthChange;
		public override void StartEffect(AbilityData data, Action finished)
		{
			foreach (var target in data.GetTargets())
			{
				Health health = target.GetComponent<Health>();

				if (health)
				{
					if (healthChange < 0)
						health.TakeDamage(data.GetUser(), -healthChange);
					else
						health.Heal(healthChange);

				}
			}
			finished();
		}
	}
}