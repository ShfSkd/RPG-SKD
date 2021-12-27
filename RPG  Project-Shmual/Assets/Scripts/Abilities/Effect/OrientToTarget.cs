using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effect
{
	[CreateAssetMenu(fileName = "Orient To Target Effect", menuName = "Abilities/Effects/Orient To Target", order = 0)]
	public class OrientToTarget : EffectStragtegy
	{
		public override void StartEffect(AbilityData data, Action finished)
		{
			Transform playerTransform = data.GetUser().transform;
			playerTransform.LookAt(data.GetTargetedPoint());
			finished();
		}
	}
}