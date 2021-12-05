using RPG.Shops;
using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
	public class RowUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI nameField;
		[SerializeField] Image iconField;
		[SerializeField] TextMeshProUGUI availabilityField;
		[SerializeField] TextMeshProUGUI priceField;
		[SerializeField] TextMeshProUGUI quentityField;

		Shop currentShop=null;
		ShopItem item = null;

		public void Setup(Shop currentShop, ShopItem item)
		{
			this.currentShop = currentShop;
			this.item = item;
			nameField.text = item.GetName();
			iconField.sprite = item.GetIcon();
			availabilityField.text =  $"{ item.GetAvailability()}";
			priceField.text = $"${item.GetPrice():N2}";
			quentityField.text =$"{ item.GetQuantityInTransaction()}";
		}
		public void Add()
		{
			currentShop.AddToTransaction(item.GetInventortItem(),1);
		}
		public void Remove()
		{
			currentShop.AddToTransaction(item.GetInventortItem(),-1);

		}
	}
}