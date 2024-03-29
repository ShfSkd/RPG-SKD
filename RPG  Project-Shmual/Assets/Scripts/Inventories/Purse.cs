﻿using RPG.UI;
using System;
using System.Collections;
using UnityEngine;
using RPG.Saving;
using GameDevTV.Inventories;

namespace RPG.Inventories
{
	public class Purse : MonoBehaviour,ISaveable,IItemStore
	{
		[SerializeField] float startingBalance = 400f;

		public event Action onChange;
		float balance = 0;
		private void Awake()
		{
			balance = startingBalance;
		}	
		public float GetBalance()
		{
			return balance;
		}
		public void UpdateBalance(float amount)
		{
			balance += amount;
			if (onChange != null)
				onChange();
		}

		public object CaptureState()
		{
			return balance;
		}

		public void RestoreState(object state)
		{
			balance = (float)state;
		}

		public int AddItems(InventoryItem item, int number)
		{
			if (item is CurrencyItem)
			{
				UpdateBalance(item.GetPrice() * number);
				return number;
			}
			return 0;
		}
	}
}