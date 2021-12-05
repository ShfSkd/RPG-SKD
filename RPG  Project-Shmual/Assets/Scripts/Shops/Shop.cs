using GameDevTV.Inventories;
using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Inventories;

namespace RPG.Shops
{
	public class Shop : MonoBehaviour,IRaycastable
	{
		[SerializeField] string shopName;

		[SerializeField] StockItemConfig[] stockConfig;
		[Serializable]
		private class StockItemConfig
		{
			public InventoryItem item;
			public int initialStock;
			[Range(0,100)]
			public float buyingDiscountPercent;
		}
		Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
		Dictionary<InventoryItem, int> stock = new Dictionary<InventoryItem, int>();
		Shopper curentShopper = null;

		public event Action onChange;
		private void Awake()
		{
			foreach (StockItemConfig config in stockConfig)
			{
				stock[config.item] = config.initialStock;
			}
		}
		public void SetShopeer(Shopper shopper)
		{
			curentShopper = shopper;
		}
		public IEnumerable<ShopItem> GetFilterdItem()
		{
			return GetAllItems();
		}
		public IEnumerable<ShopItem> GetAllItems()
		{
			foreach (StockItemConfig config in stockConfig)
			{
				float price = config.item.GetPrice() * (1 - config.buyingDiscountPercent / 100);
				int quantityInTransaction;
				transaction.TryGetValue(config.item, out quantityInTransaction);
				int currentStock = stock[config.item];
				yield return new ShopItem(config.item, currentStock, price, quantityInTransaction);
			}
		}
		public void SelectFilter(ItemCategory category) { }
		public ItemCategory GetFilter() { return ItemCategory.None; }
		public void SelectedMode(bool isBuying) { }
		public bool IsBuyingMode() { return true; }
		public bool CanTransact() 
		{
			if (IsTranactionEmpty()) return false;
			if (!HasEnoughCoins()) return false;
			return true; 
		}
		public void ConfirmTransaction()
		{
			Inventory shopperInventory = curentShopper.GetComponent<Inventory>();
			Purse shopperPurse = curentShopper.GetComponent<Purse>();
			if (shopperInventory == null || shopperPurse == null) return;

			foreach (ShopItem shopItem in GetAllItems())
			{
				InventoryItem item = shopItem.GetInventortItem();
				int quantity = shopItem.GetQuantityInTransaction();
				float price = shopItem.GetPrice();
				for (int i = 0; i < quantity; i++)
				{
					if (shopperPurse.GetBalance() < price) break;
					
					bool sucsses= shopperInventory.AddToFirstEmptySlot(item, 1);
					if (sucsses)
					{
						AddToTransaction(item, -1);
						stock[item]--;
						shopperPurse.UpdateBalance(-price);
					}
				}
			}
			if (onChange != null)
				onChange();
		}
		public float TotalAmount() 
		{
			float total = 0;
			foreach (ShopItem item in GetAllItems())
			{
				total += item.GetPrice() * item.GetQuantityInTransaction() ;
			}
			return total;
		}
		public void AddToTransaction(InventoryItem item,int quantinty)
		{
			if (!transaction.ContainsKey(item))
			{
				transaction[item] = 0;
			}

			if (transaction[item] + quantinty > stock[item])
			{
				transaction[item] = stock[item];
			}
			else
				transaction[item] += quantinty;

			if (transaction[item] <= 0)
			{
				transaction.Remove(item);
			}
			if (onChange != null)
			{
				onChange();
			}
		}
		public string GetShopName()
		{
			return shopName;
		}
		public CursorType GetCoursorType()
		{
			return CursorType.Shop;
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (Input.GetMouseButtonDown(0))
			{
				callingController.GetComponent<Shopper>().SetActiveShop(this);
			}
			return true;
		}

		public bool HasEnoughCoins()
		{
			Purse purse= curentShopper.GetComponent<Purse>();
			if (purse == null) return false;

			return purse.GetBalance() >= TotalAmount();
		}

		private bool IsTranactionEmpty()
		{
			return transaction.Count == 0;
		}

	}

}