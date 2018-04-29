using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugMenuController : MonoBehaviour {

	private Action SaveReq;
	private Action LoadReq;
	private Action ResetReq;
	private Action RandReq;

	private Action HungerToggleReq;
	private Action AddScrapReq;

	private Action ChangeAsteriodReq;
	private Action GoToHubReq;
	private Action GoToGravFragReq;


	// Use this for initialization
	void Start () {
		if (SaveReq == null) SaveReq = () => { GameState.SaveGame(); };
		if (LoadReq == null) LoadReq = () => { GameState.LoadGame(); };
		if (ResetReq == null) ResetReq = () => { GameState.ResetGame(); };
		if (RandReq == null) RandReq = () => { GameState.RandomizeStats(); };
		if (AddScrapReq == null) AddScrapReq = () => { GameState.AddScrap(); };

		if (HungerToggleReq == null) HungerToggleReq = () => {
			GameObject GO = GameObject.FindWithTag("Player");
			if (GO != null) GO.GetComponent<Hunger>().debugDontLoseHunger = !(GO.GetComponent<Hunger>().debugDontLoseHunger);
		};

		if (ChangeAsteriodReq == null) ChangeAsteriodReq = () => 
		{
			GameObject GO = GameObject.FindWithTag("Player");
			if (GO != null) GO.GetComponent<Movement>().ChangeAsteroid();
		};
		if (GoToHubReq == null) GoToHubReq = () => 
		{
			GameObject GO = GameObject.FindWithTag("Player");
			if (GO != null) GO.GetComponent<Movement>().GoToHUB();
		};
		if (GoToGravFragReq == null) GoToGravFragReq = () => 
		{
			GameObject GO = GameObject.FindWithTag("Player");
			if (GO != null) GO.GetComponent<Movement>().GoToGravityFragment();
		};
	}
	
	public void SaveButton()
	{
		if(SaveReq != null) SaveReq();
	}
	
	public void LoadButton()
	{
		if(LoadReq != null) LoadReq();
	}
	
	public void ResetButton()
	{
		if(ResetReq != null) ResetReq();
	}
	
	public void RandButton()
	{
		if(RandReq != null) RandReq();
	}

	public void HungerToggleButton()
	{
		if(HungerToggleReq != null) HungerToggleReq();
	}

	public void AddScrapButton()
	{
		if(AddScrapReq != null) AddScrapReq();
	}

	public void SwitchButton()
	{
		if(ChangeAsteriodReq != null) ChangeAsteriodReq();
	}

	public void GoToHUBButton()
	{
		if(GoToHubReq != null) GoToHubReq();
	}

	public void GoToGravFragButton()
	{
		if(GoToGravFragReq != null) GoToGravFragReq();
	}
}
