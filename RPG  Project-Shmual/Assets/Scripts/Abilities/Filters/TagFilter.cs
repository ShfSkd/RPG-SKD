using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Filters
{
	[CreateAssetMenu(fileName = "Tag Filter", menuName = "Abilities/Filters/Tag", order = 0)]
	public class TagFilter : FilterStrategy
	{
		[SerializeField] string tagToFilter = "";
		public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectToFilter)
		{
			foreach (var gameObject in objectToFilter)
			{
				if (gameObject.CompareTag(tagToFilter))
					yield return gameObject;
			}
		}
	}
}