using GameDevTV.Inventories;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Shops
{
	public class ShopItem 
	{
		InventoryItem item;
		int availabilty;
		float price;
		int quantityInTransaction;

		public ShopItem(InventoryItem item,int availabilty, float price, int quantityInTransaction)
		{
			this.item = item;
			this.availabilty = availabilty;
			this.price = price;
			this.quantityInTransaction = quantityInTransaction;

		}

		public int GetAvailability()
		{
			return availabilty;
		}

		public float GetPrice()
		{
			return price;
		}

		public Sprite GetIcon()
		{
			return item.GetIcon();
		}

		public string GetName()
		{
			return item.GetDisplayName();
		}

		public InventoryItem GetInventortItem()
		{
			return item;
		}
		public int GetQuantityInTransaction()
		{
			return quantityInTransaction;
		}
	}
}