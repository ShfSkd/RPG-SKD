using RPG.Shops;
using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI.Shops
{
	public class ShopUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI shopName;
		[SerializeField] Transform listRoot;
		[SerializeField] RowUI rowPrefab;
		[SerializeField] TextMeshProUGUI totalField;
		[SerializeField] Button confirmButton;

		Shopper shooper = null;
		Shop currentShop = null;

		Color originalToatalTextColor;
		private void Start()
		{
			originalToatalTextColor = totalField.color;
			shooper = GameObject.FindGameObjectWithTag("Player").GetComponent<Shopper>();
			if (shooper == null) return;

			shooper.activeShopChange += ShopChanged;
			confirmButton.onClick.AddListener(ConfirmTransaction);
			ShopChanged();
		}

		private void ShopChanged()
		{
			if (currentShop != null)
			{
				currentShop.onChange -= RefreshUI;
			}
			currentShop = shooper.GetActiveShop();
			gameObject.SetActive(currentShop != null);

			if (currentShop == null) return;
			shopName.text = currentShop.GetShopName();

			currentShop.onChange += RefreshUI; 


			RefreshUI();
		}

		private void RefreshUI()
		{
			foreach (Transform child in listRoot)
			{
				Destroy(child.gameObject);
			}
			foreach (ShopItem item in currentShop.GetFilterdItem())
			{
				RowUI row= Instantiate(rowPrefab,listRoot);
				row.Setup(currentShop,item);
			}

			totalField.text = $"Total:${currentShop.TotalAmount():N2}";
			totalField.color = currentShop.HasEnoughCoins() ? originalToatalTextColor : Color.red;
			confirmButton.interactable = currentShop.CanTransact();
		}

		public void Close()
		{
			shooper.SetActiveShop(null);
		}
		public void ConfirmTransaction()
		{
			currentShop.ConfirmTransaction();
		}
	}
}