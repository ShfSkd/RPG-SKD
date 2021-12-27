using GameDevTV.Inventories;
using System;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Inventories;
using RPG.Stats;
using RPG.Saving;

namespace RPG.Shops
{
	public class Shop : MonoBehaviour,IRaycastable,ISaveable
	{
		[SerializeField] string shopName;
		[SerializeField] [Range(0, 100)] float sellingPercventage = 80f;

		[SerializeField] StockItemConfig[] stockConfig;
		[Serializable]
		private class StockItemConfig
		{
			public InventoryItem item;
			public int initialStock;
			[Range(0,100)]
			public float buyingDiscountPercent;
			public int levelToUnlock = 0;
		}
		Dictionary<InventoryItem, int> transaction = new Dictionary<InventoryItem, int>();
		Dictionary<InventoryItem, int> stockSold = new Dictionary<InventoryItem, int>();
		Shopper curentShopper = null;
		bool isBuyingMode = true;
		ItemCategory filter = ItemCategory.None;
		private Dictionary<InventoryItem, float> gello;

		public event Action onChange;
		public void SetShopeer(Shopper shopper)
		{
			curentShopper = shopper;
		}
		public IEnumerable<ShopItem> GetFilterdItem()
		{
			foreach (ShopItem shopItem in GetAllItems())
			{
				InventoryItem item = shopItem.GetInventortItem();
				if (filter == ItemCategory.None || item.GetCategory() == filter)
				{
					yield return shopItem;
				}
			}
		}
		public IEnumerable<ShopItem> GetAllItems()
		{
			Dictionary<InventoryItem, float> prices = GetPrices();
			Dictionary<InventoryItem, int> availbilties = GetAvailabilites();
			foreach (InventoryItem item in availbilties.Keys)
			{
				if (availbilties[item] <= 0) continue;

				float price = prices[item];
				int quantityInTransaction;
				transaction.TryGetValue(item, out quantityInTransaction);
				int availability = availbilties[item];
				yield return new ShopItem(item, availability, price, quantityInTransaction);
			}
		}
		public void SelectFilter(ItemCategory category)
		{
			filter = category;

			if (onChange != null)
			{
				onChange();
			}

		}
		public ItemCategory GetFilter() 
		{
			return filter;
		}
		public void SelectedMode(bool isBuying)
		{
			isBuyingMode = isBuying;
			if (onChange != null)
			{
				onChange();
			}
		}
		public bool IsBuyingMode() 
		{
			return isBuyingMode; 
		}
		public bool CanTransact() 
		{
			if (IsTranactionEmpty()) return false;
			if (!HasEnoughCoins()) return false;
			if (!HasInventorySpace()) return false;

			return true; 
		}
		public bool HasInventorySpace()
		{
			if (!isBuyingMode) return true;

			Inventory shopperInventory = curentShopper.GetComponent<Inventory>();
			if (shopperInventory == null) return false;
			List<InventoryItem> flatItems = new List<InventoryItem>();
			foreach (ShopItem shopItem in GetAllItems())
			{
				InventoryItem item = shopItem.GetInventortItem();
				int quantity = shopItem.GetQuantityInTransaction();

				for (int i = 0; i < quantity; i++)
				{
					flatItems.Add(item);
				}
			}
			return shopperInventory.HasSpaceFor(flatItems);
		}
		public bool HasEnoughCoins()
		{
			if (!isBuyingMode) return true;

			Purse purse = curentShopper.GetComponent<Purse>();
			if (purse == null) return false;

			return purse.GetBalance() >= TotalAmount();
		}

		public bool IsTranactionEmpty()
		{
			return transaction.Count == 0;
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
					if (isBuyingMode)
					{
						BuyItem(shopperInventory, shopperPurse, item, price);
					}
					else
					{
						SellItem(shopperInventory, shopperPurse, item, price);
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

			var availabilties = GetAvailabilites();
			int availabilty = availabilties[item];
			if (transaction[item] + quantinty > availabilty)
			{
				transaction[item] = availabilty;
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
		private int CountItemInInventory(InventoryItem item)
		{
			Inventory inventory = curentShopper.GetComponent<Inventory>();
			if (inventory == null) return 0;
			int total = 0;
			for (int i = 0; i < inventory.GetSize(); i++)
			{
				if (inventory.GetItemInSlot(i)==item)
				{
					total += inventory.GetNumberInSlot(i);
				}
			}
			return total;
		}
		private Dictionary<InventoryItem, int> GetAvailabilites()
		{
			Dictionary<InventoryItem, int> availabilities = new Dictionary<InventoryItem, int>();
			foreach (var config in GetAvailableConfig())
			{
				if (isBuyingMode)
				{
					if (!availabilities.ContainsKey(config.item))
					{
						int sold = 0;
						stockSold.TryGetValue(config.item, out sold);
						availabilities[config.item] = -sold;
						//	availabilities[config.item] = -stockSold[config.item];
					}

					availabilities[config.item] += config.initialStock;
				}
				else
				{
					availabilities[config.item] = CountItemInInventory(config.item);
				}
			}
			return availabilities;
		}

		private Dictionary<InventoryItem, float> GetPrices()
		{
			Dictionary<InventoryItem, float> prices = new Dictionary<InventoryItem, float>();
			foreach (var config in GetAvailableConfig())
			{
				if (isBuyingMode)
				{
					if (!prices.ContainsKey(config.item))
						prices[config.item] = config.item.GetPrice();

					prices[config.item] *= (1 - config.buyingDiscountPercent / 100);
				}
				else
				{
					prices[config.item] = config.item.GetPrice() * (sellingPercventage / 100);
				}
			}
			return prices;
		}
		IEnumerable<StockItemConfig> GetAvailableConfig()
		{
			int shopperLevel = GetShopperLevel();
			foreach (var config in stockConfig)
			{
				if (config.levelToUnlock > shopperLevel) continue;

				yield return config;
			}
		}
		private void SellItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
		{
			int slot = FindFirstItemInSlot(shopperInventory, item);
			if (slot == -1) return;

			AddToTransaction(item, -1);
			shopperInventory.RemoveFromSlot(slot, 1);

			if (!stockSold.ContainsKey(item))
				stockSold[item] = 0;

			stockSold[item]--;
			shopperPurse.UpdateBalance(price);
		}


		private void BuyItem(Inventory shopperInventory, Purse shopperPurse, InventoryItem item, float price)
		{
			if (shopperPurse.GetBalance() < price) return;

			bool sucsses = shopperInventory.AddToFirstEmptySlot(item, 1);
			if (sucsses)
			{
				AddToTransaction(item, -1);

				if (!stockSold.ContainsKey(item))
					stockSold[item] = 0;

				stockSold[item]++;
				shopperPurse.UpdateBalance(-price);
			}
		}
		private int FindFirstItemInSlot(Inventory shopperInventory, InventoryItem item)
		{
			for (int i = 0; i < shopperInventory.GetSize(); i++)
			{
				if (shopperInventory.GetItemInSlot(i) == item) return i;
			}
			return -1;
		}
		int GetShopperLevel()
		{
			BaseStats stats = curentShopper.GetComponent<BaseStats>();
			if (stats == null) return 0;

			return stats.GetLevel();
		}

		public object CaptureState()
		{
			Dictionary<string, int> saveObject = new Dictionary<string, int>();
			foreach (var pair in stockSold)
			{
				saveObject[pair.Key.GetItemID()] = pair.Value;
			}
			return saveObject;
		}

		public void RestoreState(object state)
		{
			Dictionary<string, int> saveObject = (Dictionary<string, int>)state;
			stockSold.Clear();
			foreach (var pair in saveObject)
			{
				stockSold[InventoryItem.GetFromID(pair.Key)] = pair.Value;
			}
		}
	}

}