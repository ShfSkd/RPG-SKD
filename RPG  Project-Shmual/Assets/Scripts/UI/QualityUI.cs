using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using RPG.Stats;

namespace RPG.UI
{
	public class QualityUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI unassignesPointsText;
		[SerializeField] Button commitButton;

		QualityStore playerQualityStore = null;

		private void Start()
		{
			playerQualityStore = GameObject.FindGameObjectWithTag("Player").GetComponent<QualityStore>();
		}
		private void Update()
		{
			unassignesPointsText.text = playerQualityStore.GetUnassignedPoints().ToString();
			commitButton.onClick.AddListener(playerQualityStore.Commit);
		}
	}
}