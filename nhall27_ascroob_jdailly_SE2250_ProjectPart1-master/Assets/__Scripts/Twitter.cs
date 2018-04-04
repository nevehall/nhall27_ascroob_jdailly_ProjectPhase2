using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twitter: MonoBehaviour {


	private string twitterNameParameter = "Check out my high score on the game Space Shooter!";
	private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
	private const string TWITTER_LANGUAGE = "en";

	public void BtnPressed(){
		Application.OpenURL(TWITTER_ADDRESS+"?text="+WWW.EscapeURL(twitterNameParameter+" " +Scores.highscore));

	}
}