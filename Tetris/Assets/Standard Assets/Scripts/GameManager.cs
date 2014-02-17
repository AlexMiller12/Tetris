using UnityEngine;
using System.Collections;

/*
 
 TODO:
 	-Texture Atlasing
 
 
 */

public class GameManager : Singleton<GameManager> {
	
//---------------------------------------------------------------------------FIELDS:
	
	const float INTRO_DISPLAY_TIME = 2.0f;
	private GameObject highScores;
	
//---------------------------------------------------------------------MONO METHODS:

	// Use this for initialization
	void Start () {
		// Disable scripts
		Game.Instance.enabled = false;
		LevelSelector.Instance.enabled = false;
		// Get a reference to HighScores so we can reactivate it later
		highScores = HighScores.Instance.gameObject;
		// Set high scores inactive so they can't be seen
		highScores.SetActive(false);
		// Switch to level select
		StartCoroutine( FlashIntro() );
	}
	
//-----------------------------------------------------------------------MY METHODS:
	
	IEnumerator FlashIntro()
	{
		// Display intro for a second or two
		yield return new WaitForSeconds(INTRO_DISPLAY_TIME);
		// Then move on
		goToLevelSelect();
	}
	
	public void goToLevelSelect()
	{
		// Set high scores inactive (in case we're switching from high scores)
		highScores.SetActive(false);
		// Switch Background
		Background.Instance.levelSelect();
		// Enable Level Selector
		LevelSelector.Instance.enabled = true;
	}
	
	/*
	 * Considers player's score for high score list and switches scenes to High_Score
	 */
	public void goToHighScores(int score)
	{
		// Show High Scores (must activate to find Instance)
		highScores.SetActive(true);
		
		HighScores.Instance.showScores();
		// Switch Background
		Background.Instance.highScores();
	}
	
}
