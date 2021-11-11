using System.Collections;
using TMPro;
using UnityEngine;

namespace RPG.UI.DamageText
{
	public class DamageText : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI damgeTextAmount = null;
		public void DestroyText()
		{
			Destroy(gameObject);
		}
		public void SetValue(float amount)
		{
			damgeTextAmount.text = string.Format("{0:0}", amount);
		}
	}
}