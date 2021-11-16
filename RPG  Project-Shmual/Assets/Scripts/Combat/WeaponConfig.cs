using GameDevTV.Inventories;
using RPG.Attributes;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
	public class WeaponConfig : EquipableItem,IModifierProvider
	{
		[SerializeField] AnimatorOverrideController animatorOverrride;
		[SerializeField] Weapon equippedPrefab = null;
		[SerializeField] float weaponDamage = 10f;
		[SerializeField] float weaponPercentageBonus = 0f;
		[SerializeField] float weaponRange = 2f;
		[SerializeField] bool isRightHanded = true;
		[SerializeField] Projectile projectile = null;
		const string weaponName = "Weapon";

		public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
		{
			DeastroyOldWeapon(rightHand, leftHand);
			Weapon weapon = null;

			if (equippedPrefab != null)
			{
				Transform handTransform = GetTransform(rightHand, leftHand);

				weapon = Instantiate(equippedPrefab, handTransform);
				weapon.gameObject.name = weaponName;
			}
			var overrrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

			if (animatorOverrride != null)
				animator.runtimeAnimatorController = animatorOverrride;
			else if (overrrideController != null)
				animator.runtimeAnimatorController = overrrideController.runtimeAnimatorController;

			return weapon;
		}

		private void DeastroyOldWeapon(Transform rightHand, Transform leftHand)
		{
			Transform oldWeapon = rightHand.Find(weaponName);
			if (oldWeapon == null)
			{
				oldWeapon = leftHand.Find(weaponName);
			}
			if (oldWeapon == null) return;

			oldWeapon.name = "DESTROYING";
			Destroy(oldWeapon.gameObject);
		}

		private Transform GetTransform(Transform rightHand, Transform leftHand)
		{
			Transform handTransform;
			if (isRightHanded)
				handTransform = rightHand;
			else
				handTransform = leftHand;
			return handTransform;
		}

		public bool HasProjectile()
		{
			return projectile != null;
		}
		public void LaunchProjectile(Transform rightHand, Transform leftHand,Health target,GameObject instigator,float calclateDamage)
		{
			Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
			projectileInstance.SetTarget(target,instigator,calclateDamage);
		}
		public float GetWeaponRange()
		{
			return weaponRange;
		}
		public float GetPercentageBonus()
		{
			return weaponPercentageBonus;
		}
		public float GetWeaponDamage()
		{
			return weaponDamage;
		}

		public IEnumerable<float> GetAdditiveModifiers(Stat stat)
		{
			if (stat == Stat.Damage)
				yield return weaponDamage;
		}

		public IEnumerable<float> GetPercentageModifiers(Stat stat)
		{
			if (stat == Stat.Damage)
				yield return weaponPercentageBonus;
		}
	}
}