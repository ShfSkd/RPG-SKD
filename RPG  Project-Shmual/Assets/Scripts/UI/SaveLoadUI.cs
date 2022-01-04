using RPG.SceneManagment;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
	public class SaveLoadUI : MonoBehaviour
	{
		[SerializeField] Transform contactRoot;
		[SerializeField] GameObject buttonPrefab;

		private void OnEnable()
		{
			SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
			foreach (Transform child in contactRoot)
			{
				Destroy(child.gameObject);
			}
			if (savingWrapper == null) return;
			foreach (string save in savingWrapper.ListSaves())
			{
				GameObject buttonInstance = Instantiate(buttonPrefab, contactRoot);
				TMP_Text textComp = buttonInstance.GetComponentInChildren<TMP_Text>();
				textComp.text = save;
				Button button = buttonInstance.GetComponentInChildren<Button>();
				button.onClick.AddListener(() =>
				{
					savingWrapper.LoadGame(save);
				});
			}
		}



	}
}