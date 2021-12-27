using System.Collections;
using TMPro;
using UnityEngine;

namespace RPG.Attributes
{
	public class ManaDisplay : MonoBehaviour
	{
		Mana mana;
		TextMeshProUGUI manaText;

		private void Awake()
		{
			mana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
			manaText = GetComponent<TextMeshProUGUI>();
		}
		private void Update()
		{
			if (manaText != null)
			{
				manaText.text = string.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
			}
		}
	}
}