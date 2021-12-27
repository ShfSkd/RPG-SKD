﻿using GameDevTV.Utils;
using RPG.Stats;
using System.Collections;
using UnityEngine;

namespace RPG.Attributes
{
	public class Mana : MonoBehaviour
	{
		

		LazyValue<float> mana;

		private void Awake()
		{
			mana = new LazyValue<float>(GetMaxMana);
		}
		private void Update()
		{
			if (mana.value < GetMaxMana())
			{
				mana.value += GetManaRegenRate() * Time.deltaTime;
				if (mana.value > GetMaxMana())
					mana.value = GetMaxMana();
			}
		}
		public float GetMana()
		{
			return mana.value;
		}
		public float GetManaRegenRate()
		{
			return GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate);
		}
		public float GetMaxMana()
		{
			return GetComponent<BaseStats>().GetStat(Stat.Mana);
		}
		public bool UseMana(float manaToUse)
		{
			if (manaToUse > mana.value) return false;

			mana.value -= manaToUse;
			return true;
		}
	}
}