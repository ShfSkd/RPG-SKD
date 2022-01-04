using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effect
{
	[CreateAssetMenu(fileName = "Delay Composite Effect", menuName = "Abilities/Effects/Delay Composite", order = 0)]
	public class DelayCompositeEffect : EffectStragtegy
	{
		[SerializeField] float delay = 0;
		[SerializeField] EffectStragtegy[] delayedEffects;
		[SerializeField] bool abortIfCancelled;
		public override void StartEffect(AbilityData data, Action finished)
		{
			data.StartCoroutine(DelayedEffect(data, finished));
		}

		private IEnumerator DelayedEffect(AbilityData data, Action finished)
		{
			yield return new WaitForSeconds(delay);
			if (abortIfCancelled && data.IsCancelled()) yield break;
			foreach (var effect in delayedEffects)
			{
				effect.StartEffect(data, finished);
			}
		}
	}
}