using System.Collections;
using UnityEngine;
using TMPro;
namespace RPG.UI.DamageText
{
	public class DamageTextSpawner : MonoBehaviour
	{
		[SerializeField] DamageText textDamagePrefab = null;

		public void Spawn(float damageAmount)
		{
			DamageText instance= Instantiate(textDamagePrefab, transform);
			instance.SetValue(damageAmount);
		}
	}
}