﻿using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
	[CreateAssetMenu(fileName ="Type Of Ability",menuName ="Abilities/Ability",order =0)]
	public class Ability : ActionItem
	{
		[SerializeField] TargetingStrategy targetingStrategy;
		[SerializeField] FilterStrategy[] filterStrategies;
		[SerializeField] EffectStragtegy[] effectStragtegies;
		[SerializeField] float cooldownTime = 0f;
		[SerializeField] float manaCost = 0;

		public override void Use(GameObject user)
		{
			Mana mana = user.GetComponent<Mana>();
			if (mana.GetMana() < manaCost) return;

			CooldownStore cooldownStore = user.GetComponent<CooldownStore>();

			if (cooldownStore.GetCooldownTimeRemaining(this) > 0) return;

			AbilityData data = new AbilityData(user);

			ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
			actionScheduler.StartAction(data); 

			targetingStrategy.StartTargeting(data, () =>
			{
				TargetAquierd(data);
			});
		}
		void TargetAquierd(AbilityData data)
		{
			if (data.IsCancelled()) return;

			Mana mana = data.GetUser().GetComponent<Mana>();
			if (!mana.UseMana(manaCost)) return;

			CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
			cooldownStore.StartCooldown(this, cooldownTime);

			foreach (var filterStrategy in filterStrategies)
			{
				data.SetTargets(filterStrategy.Filter(data.GetTargets()));
			}
			foreach (var effect in effectStragtegies)
			{
				effect.StartEffect(data, EffectFinished);
			}
		}
		void EffectFinished()
		{

		}
	}
}