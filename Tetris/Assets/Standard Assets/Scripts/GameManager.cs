using UnityEngine;
using System.Collections;

/*
 
 TODO:
 	-Texture Atlasing
 
 
 */

public class GameManager : Singleton<GameManager> {
	
//-------------------------------------------------------------CONSTANTS AND FIELDS:
	
	const float INTRO_DISPLAY_TIME = 3.5f;
	
	private GameObject highScores, game;
	
//---------------------------------------------------------------------MONO METHODS:

	void Start () {
		// Play intro music
		AudioManager.Instance.menuMusic();
		// Disable unused scripts		
		LevelSelector.Instance.enabled = false;
		// Get a reference to HighScores and Game so we can reactivate them later
		highScores = HighScores.Instance.gameObject;
		game = Game.Instance.gameObject;
		// Set high scores and game inactive so they can't be seen
		highScores.SetActive(false);
		game.SetActive(false);
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
	
	public void goToGame(int startLevel, bool advancedMode)
	{
		game.SetActive(true);
		Game.Instance.startNewGame(startLevel, advancedMode);
	}
	
	/*
	 * Considers player's score for high score list and switches scenes to High_Score
	 */
	public void goToHighScores(int score, int level)
	{
		AudioManager.Instance.menuMusic();
		// Hide Game
		game.SetActive(false);
		// Show High Scores (must activate to find Instance)
		highScores.SetActive(true);
		
		HighScores.Instance.init(score, level);
		// Switch Background
		Background.Instance.highScores();
	}
	
}
