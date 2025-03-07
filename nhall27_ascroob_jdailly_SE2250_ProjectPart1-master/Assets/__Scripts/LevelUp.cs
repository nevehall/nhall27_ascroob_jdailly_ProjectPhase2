﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelUp : MonoBehaviour {

	//variables
	public int level;
	private float experience;
	private float experienceRequired=1500;

	//hp of the player, only for testing purposes
	public float hp;

	public Text levelUp;

	static public Scores s;




	//methods
	void Start()
	{
		level = 1;
		
	}

	void Update()
	{
		Exp();

	}

	void RankUp()
	{
		level += 1;

		switch (level) 
		{
			case 1:
				break;
			case 2:
				print ("Congratulations! You have hit Level 2 on your character!");
				experienceRequired = 3000;
				levelUp.text = "Level 2" ;
				break;
			case 3:
				print ("Congratulations! You have hit Level 3 on your character!");
				levelUp.text = "Level 3";
				break;
				
		}
	}

	void Exp()
	{
		if (Scores.score >= experienceRequired) {
			RankUp ();
		}
	}
}
