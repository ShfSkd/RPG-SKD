using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
	public class PlayerController : MonoBehaviour
	{
		Health health;
		[Serializable]
		struct CursorMapping
		{
			public CursorType type;
			public Texture2D texture;
			public Vector2 hotspot;
		}
		[SerializeField] CursorMapping[] cursorMppaings = null;
		[SerializeField] float maxNavMeshProjectionDistance = 1f;
		[SerializeField] float raycastRaduis = 1f;

		bool isDraggingUI;

		private void Awake()
		{
			health = GetComponent<Health>();
		}
		private void Update()
		{
			if (InteractWithUI()) return;
			if (health.IsDead()) 
			{
				SetCursor(CursorType.None);
				return;
			}
			if (InteractWithComponent()) return;
			if (InteractWithMovement()) return;

			SetCursor(CursorType.None);
			print("nothing to do");
		}


		private bool InteractWithUI()
		{
			if (Input.GetMouseButtonUp(0))
				isDraggingUI = false;

			if (EventSystem.current.IsPointerOverGameObject())
			{
				if (Input.GetMouseButtonDown(0))
					isDraggingUI = true;

				SetCursor(CursorType.UI);
				return true;
			}
			if (isDraggingUI)
				return true;

			return false;
		}
		private bool InteractWithComponent()
		{
			RaycastHit[] hits = RaycastAllSorted();
			foreach (RaycastHit hit in hits)
			{
				IRaycastable[] raycastables= hit.transform.GetComponents<IRaycastable>();
				foreach (IRaycastable raycastable in raycastables)
				{
					if (raycastable.HandleRaycast(this))
					{
						SetCursor(raycastable.GetCoursorType());
						return true;
					}
				}
			}
			return false;
		}
		RaycastHit[] RaycastAllSorted()
		{
			RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRaduis);
			float[] distances = new float[hits.Length];
			for (int i = 0; i < hits.Length; i++)
			{
				distances[i] = hits[i].distance;
			}
			Array.Sort(distances,hits);
			return hits;
		}
		private bool InteractWithMovement()
		{
			/*RaycastHit hit;
			bool hasHit = Physics.Raycast(GetMouseRay(), out hit);*/
			Vector3 target;
			bool hasHit = RaycastNavMesh(out target);
			if (hasHit)
			{
				if (!GetComponent<Mover>().CanMoveTo(target)) return false;

				if (Input.GetMouseButton(0))
				{
					GetComponent<Mover>().StartMoveAction(target,1f);
				}
				SetCursor(CursorType.Movement);
				return true;
			}
			return false;
		}
		bool RaycastNavMesh(out Vector3 target)
		{
			target = new Vector3();
			RaycastHit hit;
			bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
			if (!hasHit) return false;

			NavMeshHit navMeshHit;
			bool hasCastToNavMesh= NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance,NavMesh.AllAreas);
			if (!hasCastToNavMesh) return false;

			target = navMeshHit.position;

			return true;
		}
		private void SetCursor(CursorType type)
		{
			CursorMapping mapping = GetCursorMapping(type);
			Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
		}
		CursorMapping GetCursorMapping(CursorType type)
		{
			foreach (CursorMapping mapping in cursorMppaings)
			{
				if (mapping.type == type)
				{
					return mapping;
				}
			}
			return cursorMppaings[0];
		}
		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
}