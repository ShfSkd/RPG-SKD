﻿using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
	public class WeaponPickup : MonoBehaviour,IRaycastable
	{
		[SerializeField] WeaponConfig weapon;
		[SerializeField] float healthToRestore = 0;
		[SerializeField] float respawnTime = 10f;
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				Pickup(other.gameObject);
			}
		}

		private void Pickup(GameObject subject)
		{
			if (weapon != null)
				subject.GetComponent<Fighter>().EquipWeapon(weapon);

			if (healthToRestore > 0)
				subject.GetComponent<Health>().Heal(healthToRestore);

			StartCoroutine(HideForSeconds(respawnTime));
		}

		IEnumerator HideForSeconds(float seconds)
		{
			ShowPickup(false);
			yield return new WaitForSeconds(seconds);
			ShowPickup(true);
		}

		
		private void ShowPickup(bool shouldShow)
		{
			transform.GetComponent<Collider>().enabled = shouldShow;
			foreach (Transform child in transform)
			{
				child.gameObject.SetActive(shouldShow);
			}
		}

		public bool HandleRaycast(PlayerController callingController)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Pickup(callingController.gameObject);
			}
			return true;
		}

		public CursorType GetCoursorType()
		{
			return CursorType.Pickup;
		}
	}
}