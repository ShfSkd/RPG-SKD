using System.Collections;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
	public class DialogueUI : MonoBehaviour
	{
		PlayerConversant playerConversant;
		[SerializeField] TextMeshProUGUI AIText;
		[SerializeField] Button nextButton;
		[SerializeField] GameObject aIRespawn;
		[SerializeField] Transform choiceRoot;
		[SerializeField] GameObject choisePrefab;
		[SerializeField] Button quitButton;
		[SerializeField] TextMeshProUGUI conversantName;


		private void Start()
		{
			playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
			playerConversant.onConversationUpdate += UpdateUI;
			nextButton.onClick.AddListener(() => playerConversant.Next());
			quitButton.onClick.AddListener(() => playerConversant.Quit());

			UpdateUI();
		}
		void UpdateUI()
		{
			gameObject.SetActive(playerConversant.IsActive());
			if (!playerConversant.IsActive()) return;

			conversantName.text = playerConversant.GetCurrentConversantName();
			aIRespawn.SetActive(!playerConversant.IsChoosing());
			choiceRoot.gameObject.SetActive(playerConversant.IsChoosing());
			if (playerConversant.IsChoosing())
			{
				BuildChoiceList();
			}
			else
			{
				AIText.text = playerConversant.GetText();
				nextButton.gameObject.SetActive(playerConversant.HasNext());
			}
		}

		private void BuildChoiceList()
		{
			foreach (Transform child in choiceRoot.transform)
			{
				Destroy(child.gameObject);
			}
			foreach (DialogueNode choise in playerConversant.GetChoices())
			{
				GameObject choiseInstance = Instantiate(choisePrefab, choiceRoot);
				var textComp = choiseInstance.GetComponentInChildren<TextMeshProUGUI>();
				textComp.text = choise.GetText();
				Button button = choiseInstance.GetComponentInChildren<Button>();
				button.onClick.AddListener(()=> 
				{
					playerConversant.SelectChoice(choise);
				});
			}
		}
	}
}