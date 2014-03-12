using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO dialog asking for user's name when they make the list (GUI)
// TODO use .insert() to put in new score; it will shift the lesser scores down


public class HighScores : Singleton<HighScores> {
	
//----------------------------------------------------------------INNER CLASS SCORE:
	
	private class Score 
	{
		// Constructor
		public Score () {}
		// Fields
		public string name;
		public int score;
		public int level;
	}
	
//---------------------------------------------------------------------------FIELDS:
	 
	public Texture2D continue_unclicked, continue_clicked;
	// Current texture used for continue button
	private Texture2D continue_button; 
	// The world-space coordinates at which the continue button belongs
	private Vector3 continueTL = new Vector3(0.5f, -4.9f, 20.7f);
	private Vector3 continueBR = new Vector3(8.24f, -4.9f, 16.9f);
	
	//private bool playerMadeList;
	
	private const string DEFAULT_NAME = "Some Player";
	private const int DEFAULT_SCORE = 10000;
	private const int DEFAULT_LEVEL = 9;
		
	public GUISkin guiSkin;

	private List<Score> highScores;
	// The rank of the score of the game we just finished
	private int lastGamesRank;
	private bool needPlayersName;
	private string playerName;
	private Rect bgBox, textFieldBox, inputButtonBox;
	
//---------------------------------------------------------------------MONO METHODS:
	
	void OnGUI ()
	{
		GUI.skin = guiSkin;
		
		Vector3 tl = OnlyCamera.Instance.camera.WorldToScreenPoint(continueTL);
		Vector3 br = OnlyCamera.Instance.camera.WorldToScreenPoint(continueBR);
		float width = br.x - tl.x;
		float height = tl.y - br.y;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), continue_button))
		{
			// Show clicked texture
			continue_button = continue_clicked;
			// Then continue
			StartCoroutine( Continue() ) ;
		}	
		// We need to get the player's name to record their score		
		if (needPlayersName)
		{
			GUI.Box (bgBox, "Congratulations, Comrade!");
			
			playerName = GUI.TextField(textFieldBox, playerName);
			
			if (GUI.Button(inputButtonBox, "Enter"))
			{
				setComradeName();
				saveHighScores();
			}			
		}
	}
		
//-----------------------------------------------------------------------MY METHODS:

	/*
	 * Returns to the level select scene
	 */
	IEnumerator Continue()
	{
		// Wait a sec so we can see the button click
		yield return new WaitForSeconds(0.3f);
		// Disable high scores 
		GameManager.Instance.goToLevelSelect();
	}
		
	/*
	 * Calculates and sets the sizes of the Rects used to get player's name
	 */
	private void setGUIBoxes()
	{
		float left = Screen.width / 3;
		float top = Screen.height / 3;
		float width = left;
		float height = top / 2;
		
		bgBox = new Rect(left, top, width, height);
		
		textFieldBox = new Rect(left, top + height / 4, width, height / 4);
		
		inputButtonBox = new Rect(left, top + height / 2, width, height / 2);
	}
	
	/*
	 * Reads previous high scores into highScores
	 */
	private void loadHighScores()
	{

		for (int i = 0; i < 10; i++) 
		{
			Score thisScore = new Score();
			//PlayerPrefs.DeleteAll();
			thisScore.name = PlayerPrefs.GetString("name" + i, DEFAULT_NAME);
			
			if (thisScore.name.Equals(""))
			{
				thisScore.name = "?????";	
			}

			thisScore.score = PlayerPrefs.GetInt("score" + i, DEFAULT_SCORE);
			thisScore.level = PlayerPrefs.GetInt("level" + i, DEFAULT_LEVEL);
			
			highScores.Add(thisScore);
		}
	}
	
	/*
	 * Returns the rank score's rank if it makes the high score list, otherwise
	 * it returns -1
	 */
	private int rankScore(int score)
	{
		for (int i = 0; i < 10; i++)
		{
			if (score > highScores[i].score)
			{
				return i;
			}
		}
		return -1;
	}
	
	/*
	 * Reads the high scores from disk. Creates new file if it doesn't exist
	 */
	private void saveHighScores()
	{
		Debug.Log("Save scores");
		for (int i = 0; i < 10; i++) //TODO try/catch?
		{
			PlayerPrefs.SetString("name" + i, highScores[i].name);
			PlayerPrefs.SetInt("score" + i, highScores[i].score);
			PlayerPrefs.SetInt("level" + i, highScores[i].level);
		}
	}
			
	private void setComradeName()
	{
		if (needPlayersName)
		{
			needPlayersName = false;
			if (playerName == "")
			{
				playerName = "?????";
			}
			highScores[lastGamesRank].name = playerName;
			updateText();
		}		
	}
	
	/*
	 * Sets up the HighScores to show the high scores
	 */
	public void init(int playerScore, int playerLevel)
	{
		
		highScores = new List<Score>();	
		loadHighScores();
		continue_button = continue_unclicked;
		needPlayersName = false;
		// See if player's score ranks among the greats
		lastGamesRank = rankScore(playerScore);
		
		if (lastGamesRank > -1)
		{
			playerName = "";
			needPlayersName = true;
			setGUIBoxes();
			Score newScore = new Score();
			newScore.name = "";
			newScore.score = playerScore;
			newScore.level = playerLevel;
			highScores.Insert(lastGamesRank, newScore);
			saveHighScores();
		}
		updateText();
		GUI.enabled = true;
	}
	
	/*
	 * Updates the 3D text showing the high scores
	 */
	private void updateText() 
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			string name = highScores[i].name;
			int level = highScores[i].level;
			int score = highScores[i].score;
			// Scores are labeled 1-10
			GameObject entry = GameObject.Find(i + 1 + "");
			entry.GetComponent<TextMesh>().text = name;
			entry.transform.GetChild(0).GetComponent<TextMesh>().text = "" + level;
			entry.transform.GetChild(1).GetComponent<TextMesh>().text = "" + score;

		}
	}
}
