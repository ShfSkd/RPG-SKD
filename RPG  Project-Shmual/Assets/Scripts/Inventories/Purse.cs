using RPG.UI;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Inventories
{
	public class Purse : MonoBehaviour
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
	}
}