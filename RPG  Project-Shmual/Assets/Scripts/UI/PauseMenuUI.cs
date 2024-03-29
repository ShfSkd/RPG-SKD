﻿using RPG.Control;
using RPG.SceneManagment;
using System.Collections;
using UnityEngine;

namespace RPG.UI
{
	public class PauseMenuUI : MonoBehaviour
	{
		PlayerController playerController;
		private void Awake()
		{
			playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		}
		private void OnEnable()
		{
			if (playerController == null) return;
			Time.timeScale = 0;
			playerController.enabled = false;
		}
		private void OnDisable()
		{
			if (playerController == null) return;
			Time.timeScale = 3;
			playerController.enabled = true;
		}
		public void Save()
		{
			SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
			savingWrapper.Save();
		}
		public void SaveAndQuit()
		{
			SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
			savingWrapper.Save();
			savingWrapper.LoadMenu();
		}
	}
}