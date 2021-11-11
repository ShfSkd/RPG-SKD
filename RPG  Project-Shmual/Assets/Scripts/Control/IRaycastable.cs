using System.Collections;
using UnityEngine;

namespace RPG.Control
{
	public interface IRaycastable 
	{
		CursorType GetCoursorType();
		bool HandleRaycast(PlayerController callingController);
	}
}