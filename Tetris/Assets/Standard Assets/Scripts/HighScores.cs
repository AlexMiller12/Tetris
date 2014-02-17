using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO dialog asking for user's name when they make the list

public class HighScores : Singleton<HighScores> {
	
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
	
	
	public GUISkin guiSkin;
	
	private List<Score> highScores;
		
	

//---------------------------------------------------------------------MONO METHODS:
	
	// Use this for initialization
	void Start () 
	{	
		highScores = new List<Score>();		
	}

	void OnGUI ()
	{
		Debug.Log("Updating!");	
		GUI.skin = guiSkin;
		
		Vector3 tl = MyCamera.Instance.camera.WorldToScreenPoint(continueTL);
		Vector3 br = MyCamera.Instance.camera.WorldToScreenPoint(continueBR);
		float width = br.x - tl.x;
		float height = tl.y - br.y;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), continue_button))
		{
			// Show clicked texture
			continue_button = continue_clicked;
			// Then continue
			StartCoroutine( Continue() ) ;
		}	
	}
		
//-----------------------------------------------------------------------MY METHODS:
	
	IEnumerator Continue()
	{
		// Wait a sec so we can see the button click
		yield return new WaitForSeconds(0.3f);
		// Disable high scores 
		GameManager.Instance.goToLevelSelect();
	}
	
	
	/*
	 * Reads the high scores from disk. Creates new file if it doesn't exist
	 */
	private void loadHighScores()
	{
		for (int i = 0; i < 10; i++) //TODO try/catch?
		{
			Score thisScore = new Score();
			thisScore.name = PlayerPrefs.GetString("name" + i, highScores[i].name);
			thisScore.score = PlayerPrefs.GetInt("score" + i, highScores[i].score);
			thisScore.level = PlayerPrefs.GetInt("level" + i, highScores[i].level);
			highScores.Add(thisScore);
		}
	}
	
	/*
	 * Returns the rank score's rank if it makes the high score list, otherwise
	 * it returns -1
	 */
	public int makesList(int score)
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
		for (int i = 0; i < 10; i++)//TODO try/catch?
		{
			PlayerPrefs.SetString("name" + i, highScores[i].name);
			PlayerPrefs.SetInt("score" + i, highScores[i].score);
			PlayerPrefs.SetInt("level" + i, highScores[i].level);
		}
	}
	
	
	public void showScores()
	{
		continue_button = continue_unclicked;
		GUI.enabled = true;
	}
}
