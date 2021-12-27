using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effect
{
	[CreateAssetMenu(fileName = "Trigger Animation Effect", menuName = "Abilities/Effects/Trigger Animation", order = 0)]
	public class TriggerAnimationEffect : EffectStragtegy
	{
		[SerializeField] string animationTrigger;

		public override void StartEffect(AbilityData data, Action finished)
		{
			Animator animator = data.GetUser().GetComponent<Animator>();
			animator.SetTrigger(animationTrigger);
			finished();
		}
	}
}