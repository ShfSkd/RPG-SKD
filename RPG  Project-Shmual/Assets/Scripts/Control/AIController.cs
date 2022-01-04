using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
	public class AIController : MonoBehaviour
	{
		[SerializeField] float chaseDistance = 5f;
		[SerializeField] float suspicionTime = 2f;
		[SerializeField] float aggroColdownTime = 5f;
		[SerializeField] float waypointDwellTime = 2f;
		[SerializeField] PatrolPath patrolPath;
		[SerializeField] float waypointTolerance = 1f;
		[Range(0, 1)]
		[SerializeField] float patrolSpeedFraction = 0.2f;
		[SerializeField] float shotDistance = 5f;

		Fighter fighter;
		Health health;
		GameObject player;
		Mover mover;

		LazyValue<Vector3> guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeSinceAggrevat = Mathf.Infinity;
		float timeSinceArrivedAtWaypoint = Mathf.Infinity;
		int currentWaypointIndex = 0;

		private void Awake()
		{
			fighter = GetComponent<Fighter>();
			health = GetComponent<Health>();
			player = GameObject.FindGameObjectWithTag("Player");
			mover = GetComponent<Mover>();

			guardPosition = new LazyValue<Vector3>(GetInitialPosition);
			guardPosition.ForceInit();
		}
		public void Reset()
		{
			NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
			navMeshAgent.Warp(guardPosition.value);
			timeSinceLastSawPlayer = Mathf.Infinity;
			timeSinceAggrevat = Mathf.Infinity;
			timeSinceArrivedAtWaypoint = Mathf.Infinity;
			currentWaypointIndex = 0;
		}
		Vector3 GetInitialPosition()
		{
			return transform.position;
		}
		private void Update()
		{
			if (health.IsDead()) return;

			if (IsAggrevated() && fighter.CanAttack(player))
			{
				AttackBehavior();
			}
			else if (timeSinceLastSawPlayer < suspicionTime)
			{
				SuspicionBehaviour();
			}
			else
			{
				PatrolBehavior();
			}
			UpdateTimers();
		}
		public void Aggrevate()
		{
			timeSinceAggrevat = 0;
		}
		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeSinceArrivedAtWaypoint += Time.deltaTime;
			timeSinceAggrevat += Time.deltaTime;
		}

		private void PatrolBehavior()
		{
			Vector3 nextPoistion = guardPosition.value;
			if (patrolPath != null)
			{
				if (AtWaypoint())
				{
					timeSinceArrivedAtWaypoint = 0;
					CycleWaypoint();
				}
				nextPoistion = GetCurrentWaypoint();
			}
			if (timeSinceArrivedAtWaypoint > waypointDwellTime)
			{
				mover.StartMoveAction(nextPoistion,patrolSpeedFraction);
			}
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance;
		}
		private void CycleWaypoint()
		{
			currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWaypoint(currentWaypointIndex);
		}


		private void SuspicionBehaviour()
		{
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void AttackBehavior()
		{
			timeSinceLastSawPlayer = 0;
			fighter.Attack(player);

			AggrevatNerarbyEnemies();
		}

		private void AggrevatNerarbyEnemies()
		{
			RaycastHit []hits= Physics.SphereCastAll(transform.position, shotDistance, Vector3.up, 0);
			foreach (RaycastHit hit in hits)
			{
				AIController aI = hit.collider.GetComponent<AIController>();
				if (aI == null) continue;

				aI.Aggrevate();
				
			}
		}

		private bool IsAggrevated()
		{
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			return distanceToPlayer < chaseDistance || timeSinceAggrevat < aggroColdownTime;
		}
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}