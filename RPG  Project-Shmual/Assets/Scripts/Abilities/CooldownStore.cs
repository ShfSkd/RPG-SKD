﻿using GameDevTV.Inventories;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
	public class CooldownStore : MonoBehaviour
	{
		Dictionary<InventoryItem, float> cooldownTimers = new Dictionary<InventoryItem, float>();
		Dictionary<InventoryItem, float> initialCooldownTimes = new Dictionary<InventoryItem, float>();

		private void Update()
		{
			var keys = new List<InventoryItem>(cooldownTimers.Keys);
			foreach (InventoryItem ability in keys)
			{
				cooldownTimers[ability] -= Time.deltaTime;

				if (cooldownTimers[ability] < 0)
				{
					cooldownTimers.Remove(ability);
					initialCooldownTimes.Remove(ability);
				}
			}
		}
		public void StartCooldown(InventoryItem ability,float cooldownTime)
		{
			cooldownTimers[ability] = cooldownTime;
			initialCooldownTimes[ability] = cooldownTime;
		}
		public float GetCooldownTimeRemaining(InventoryItem ability)
		{
			if (!cooldownTimers.ContainsKey(ability)) return 0;

			return cooldownTimers[ability];
		}

		public float GetFractionRemaining(InventoryItem ability)
		{
			if (ability == null) return 0;

			if (!cooldownTimers.ContainsKey(ability)) return 0;

			return cooldownTimers[ability] / initialCooldownTimes[ability];
		}
	}
}