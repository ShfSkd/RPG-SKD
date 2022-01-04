using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RPG.Stats;

namespace RPG.UI
{
	public class QualityRowUI : MonoBehaviour
	{
		[SerializeField] Quality quality;  
		[SerializeField] TextMeshProUGUI valueText;
		[SerializeField] Button minusButton;
		[SerializeField] Button plusButton;

		QualityStore playerQualityStore = null;

		private void Start()
		{
			playerQualityStore = GameObject.FindGameObjectWithTag("Player").GetComponent<QualityStore>();

			minusButton.onClick.AddListener(() => Allocate(-1));
			plusButton.onClick.AddListener(() => Allocate(1));
		}

		private void Update()
		{
			minusButton.interactable = playerQualityStore.CanAssignPoints(quality, -1);
			plusButton.interactable = playerQualityStore.CanAssignPoints(quality, 1);
			valueText.text = playerQualityStore.GetProposedPoints(quality).ToString();
		}

		public void Allocate(int points)
		{
			playerQualityStore.AssignPoints(quality, points);
		}
	}
}