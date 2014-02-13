using UnityEngine;
using System.Collections;

/*
 
 TODO:
 	-Texture Atlasing
 
 
 */

public class GameManager : MonoBehaviour {
	
//---------------------------------------------------------------------------FIELDS:
	
	const float INTRO_DISPLAY_TIME = 2.0f;
	
//---------------------------------------------------------------------MONO METHODS:

	// Use this for initialization
	void Start () {
		
		//-----TEMP
		//Background.Instance.levelSelect();
		//-----
		
		// Disable scripts
		Game.Instance.enabled = false;
		LevelSelector.Instance.enabled = false;
		
		// Switch to level select
		StartCoroutine( GoToLevelSelect() );
		
	}
	
	// Update is called once per frame 
	void Update () {
	
	}	
	
//-----------------------------------------------------------------------MY METHODS:
	
	public void startNewGame(int level)
	{
		
	}
	
	IEnumerator GoToLevelSelect()
	{
		// Display intro for a second or two
		yield return new WaitForSeconds(INTRO_DISPLAY_TIME);
		// Switch Background
		Background.Instance.levelSelect();
		// Enable Level Selector
		LevelSelector.Instance.enabled = true;
	}
	
	/*
	 * Considers player's score for high score list and switches scenes to High_Score
	 */
	public void endCurrentGame(int score)
	{
		
	}
	
}
