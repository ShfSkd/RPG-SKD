using GameDevTV.Inventories;
using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
	public class Fighter : MonoBehaviour,IAction/*,ISaveable*/
	{
		[SerializeField] float timeBetweenAttacks = 1f;
		[SerializeField] Transform rightHandTransform=null, leftHandTransform = null;
		[SerializeField] WeaponConfig deafultWeapon = null;
		[SerializeField] float autoAttackRange = 4f;

		Health target;
		Equipment equipment;
		float timeSinceLastAttack = Mathf.Infinity;
		WeaponConfig currentWeaponConfig;
		LazyValue<Weapon> currentWeapon;

		private void Awake()
		{
			currentWeaponConfig = deafultWeapon;
			currentWeapon = new LazyValue<Weapon>(SetupDefualtWeapon);
			equipment = GetComponent<Equipment>();
			if (equipment)
			{
				equipment.equipmentUpdated += UpdateWeapon;
			}
		}
		private Weapon SetupDefualtWeapon()
		{
			return AttachWeapon(deafultWeapon);
		}

		private void Start()
		{
			currentWeapon.ForceInit();
		}
		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;

			if (target == null) return;
			if (target.IsDead())
			{
				target = FindNewTargetInRange();
				if (target == null) return;
			}

			if (!GetIsInRange(target.transform))
			{
				GetComponent<Mover>().MoveTo(target.transform.position,1f);
			}
			else
			{
				GetComponent<Mover>().Cancel();
				AttackBehavior();
			}
		}

		public void EquipWeapon(WeaponConfig weapon)
		{
			currentWeaponConfig = weapon;
			currentWeapon.value= AttachWeapon(weapon);
		}
		private void UpdateWeapon()
		{
			var weapon= equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
			if (weapon == null)
			{
				EquipWeapon(deafultWeapon);
			}
			else
			EquipWeapon(weapon);
		}
		private Weapon AttachWeapon(WeaponConfig weapon)
		{
			Animator animator = GetComponent<Animator>();
			return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
			
		}

		public Health GetTarget()
		{
			return target;
		}
		public Transform GetHandTransform(bool isRightHand)
		{
			if (isRightHand) 
				return rightHandTransform;
			else
				return leftHandTransform;
		}
		private void AttackBehavior()
		{
			transform.LookAt(target.transform);
			if (timeSinceLastAttack > timeBetweenAttacks)
			{
				// This will trigger the hit event
				TriggerAttack();
				timeSinceLastAttack = 0;

			}
		}
		private Health FindNewTargetInRange()
		{
			Health posibleBestEnemy = null;
			float theBestEnemyDistance = Mathf.Infinity;
			foreach (var enemy in FindAllTargetsInRange())
			{
				float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);

				if (enemyDistance < theBestEnemyDistance)
				{
					posibleBestEnemy = enemy;
					theBestEnemyDistance = enemyDistance;
				}
			}
			return posibleBestEnemy;
		}
		IEnumerable<Health> FindAllTargetsInRange()
		{
			RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, autoAttackRange, Vector3.up);
			foreach (RaycastHit hit in raycastHits)
			{
				Health health = hit.transform.GetComponent<Health>();
				if (health == null) continue;
				if (health.IsDead()) continue;
				if (health.gameObject == gameObject) continue;
				if (!health.GetComponent<CombatTarget>()) continue;

				yield return health;
				
			}

		}

		private void TriggerAttack()
		{
			GetComponent<Animator>().ResetTrigger("stopAttack");
			GetComponent<Animator>().SetTrigger("attack");
		}
		private void StopAttack()
		{
			GetComponent<Animator>().ResetTrigger("attack");
			GetComponent<Animator>().SetTrigger("stopAttack");
		}
		// Animation Event
		void Hit()
		{
			if (target == null) return;
			float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
			BaseStats targetBaseStats = target.GetComponent<BaseStats>();
			if (targetBaseStats != null)
			{
				float defence = targetBaseStats.GetStat(Stat.Defence);
				damage /= 1 + defence / damage;
			}
			if (currentWeapon.value != null)
			{
				currentWeapon.value.OnHit();
			}

			if (currentWeaponConfig.HasProjectile())
			{
				currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
			}
			else
			{

				target.TakeDamage(gameObject, damage);
			}
		}
		void Shoot()
		{
			Hit();
		}
		private bool GetIsInRange(Transform targetTranform)
		{
			return Vector3.Distance(transform.position, targetTranform.position) < currentWeaponConfig.GetWeaponRange();
		}
		public bool CanAttack(GameObject target)
		{
			if (target == null) return false;
			if (!GetComponent<Mover>().CanMoveTo(target.transform.position)&&!GetIsInRange(target.transform))
			{
				return false;
			}
			Health targetToTest = target.GetComponent<Health>();
			return targetToTest != null && !targetToTest.IsDead();
		}


		public void Attack(GameObject combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			target = combatTarget.GetComponent<Health>();
		}
		public void Cancel()
		{
			StopAttack();
			target = null;
			GetComponent<Mover>().Cancel();
		}

	/*	public object CaptureState()
		{
			return currentWeaponConfig.name;
		}

		public void RestoreState(object state)
		{
			string weaponName = (string)state;
			WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
			EquipWeapon(weapon);
		}*/
	}
}