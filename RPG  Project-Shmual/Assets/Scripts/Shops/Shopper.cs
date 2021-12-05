using System;
using System.Collections;
using UnityEngine;

namespace RPG.Shops
{
	public class Shopper : MonoBehaviour
	{
		Shop activeShop = null;

		public event Action activeShopChange;
		public void SetActiveShop(Shop shop)
		{
			if (activeShop != null)
				activeShop.SetShopeer(null);
			
			activeShop = shop;

			if (activeShop != null)
				activeShop.SetShopeer(this);

			if (activeShopChange != null)
				activeShopChange();
		}

		public Shop GetActiveShop()
		{
			return activeShop;
		}
	}
}