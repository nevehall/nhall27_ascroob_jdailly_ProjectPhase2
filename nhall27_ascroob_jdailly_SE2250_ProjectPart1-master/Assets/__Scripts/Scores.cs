using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scores : MonoBehaviour {

	public Text current;
	public Text high;

	public static int score;

	public static int highscore;

	static public Scores s;


	void Start()
	{
		if (s == null) {                                        // c
			s = this;
		}
		score = 0;
		highscore = PlayerPrefs.GetInt ("highscore", highscore);

		high.text = "HighScore: " + highscore.ToString();
		current.text = "Score: "+ score.ToString ();
	}

	void Update()
	{
		current.text = "Score: " + score.ToString();

		if (score > highscore)
		{
			highscore = score;
			high.text = "HighScore: " + highscore.ToString();
		}

		PlayerPrefs.SetInt ("highscore", highscore);
	}


	public static void AddPoints (int pointsToAdd)
	{
		s.AddPOINTS(pointsToAdd);
		//pointsGained = 2*pointsToAdd;
	}

	public  void AddPOINTS(int pointsToAdd)
	{
		score += pointsToAdd;
		//pointsGained = 2*pointsToAdd;
	}

	public static void Reset()
	{
		score = 0;
	}

	public static void ResetHighScore (){
		highscore = 0;
	}

}
