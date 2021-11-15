﻿using GameDevTV.Inventories;
using System.Collections;
using UnityEngine;

namespace RPG.Control
{
	public class ClickablePickup : MonoBehaviour,IRaycastable
	{
		Pickup pickup;

		private void Awake()
		{
			pickup = GetComponent<Pickup>();
		}

		public CursorType GetCoursorType()
		{
			if (pickup.CanBePickedUp())
				return CursorType.Pickup;
			else
				return CursorType.FullPickup;
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (Input.GetMouseButtonDown(0))
				pickup.PickupItem();

			return true;
		}

	
	}
}