using UnityEngine;
using System.Collections;

public class LevelSelector : Singleton<LevelSelector> {
		
//------------------------------------------------------------------------CONSTANTS:
	
	private const int BUTTON_WIDTH = 30, BUTTON_HEIGHT = 30;
	
//---------------------------------------------------------------------------FIELDS:
		
	public GUISkin guiSkin;
	
	// The textures used for clicked state for each button
	public Texture2D start_clicked, lvl0_clicked, lvl1_clicked, lvl2_clicked, 
					 lvl3_clicked, lvl4_clicked, lvl5_clicked, lvl6_clicked, 
					 lvl7_clicked, lvl8_clicked, lvl9_clicked;
	
	// The textures used for unclicked state for each button
	public Texture2D start_unclicked, lvl0_unclicked, lvl1_unclicked, lvl2_unclicked, 
					 lvl3_unclicked, lvl4_unclicked, lvl5_unclicked, lvl6_unclicked, 
					 lvl7_unclicked, lvl8_unclicked, lvl9_unclicked;
	
	// The textures that are currently being displayed for each button
	private Texture2D lvl0, lvl1, lvl2, lvl3, lvl4, lvl5, lvl6, lvl7, lvl8, lvl9;
	
	private int startLevel = 0;
	
//---------------------------------------------------------------------MONO METHODS:
	
	void Start()
	{
		unclickAllButtons();	
		GUI.enabled = true;
	}
	
	void OnGUI()
	{		
		GUI.skin = guiSkin;
		
		int topX = 121;
		int topY = 93;
		
		if (GUI.Button (buttonRect(topX, topY), lvl0))
		{
			Debug.Log("LevelSelector --- CLICKED!");
			unclickAllButtons();
			lvl0 = lvl0_clicked;
			// Screen.width, Screen.height
			startLevel = 0;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl1))
		{
			unclickAllButtons();	
			lvl1 = lvl1_clicked;
			startLevel = 1;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl2))
		{
			unclickAllButtons();	
			lvl2 = lvl2_clicked;
			startLevel = 2;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl3))
		{
			unclickAllButtons();	
			lvl3 = lvl3_clicked;
			startLevel = 3;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl4))
		{
			unclickAllButtons();	
			lvl4 = lvl4_clicked;
			startLevel = 4;
		}
		topX -= BUTTON_WIDTH * 4; 
		topY += BUTTON_HEIGHT;
		
		if (GUI.Button (buttonRect(topX, topY), lvl5))
		{
			unclickAllButtons();	
			lvl5 = lvl5_clicked;
			startLevel = 5;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl6))
		{
			unclickAllButtons();	
			lvl6 = lvl6_clicked;
			startLevel = 6;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl7))
		{
			unclickAllButtons();	
			lvl7 = lvl7_clicked;
			startLevel = 7;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl8))
		{
			unclickAllButtons();	
			lvl8 = lvl8_clicked;
			startLevel = 8;
		}
		topX += BUTTON_WIDTH; 
		
		if (GUI.Button (buttonRect(topX, topY), lvl9))
		{
			unclickAllButtons();	
			lvl9 = lvl9_clicked;
			startLevel = 9;
		}	 	
		
		if (GUI.Button (new Rect(200, 210, 120, 65), start_unclicked))
		{
			start_unclicked = start_clicked;
			StartCoroutine( StartGame() ) ;
			Debug.Log("LevelSelector --- START CLICKED!");
		}

	}
	
//-----------------------------------------------------------------------MY METHODS:
	
	/*
	 * Returns a Rect BUTTON_WIDTH tall and wide at topLeftX, topLeftY
	 */
	private Rect buttonRect(int topLeftX, int topLeftY)
	{
		return new Rect(topLeftX, topLeftY, BUTTON_HEIGHT, BUTTON_WIDTH);
	}
	
	/*
	 * Sets all levels to unclicked states
	 */
	private void unclickAllButtons()
	{
		lvl0 = lvl0_unclicked;
		lvl1 = lvl1_unclicked;
		lvl2 = lvl2_unclicked;
		lvl3 = lvl3_unclicked;
		lvl4 = lvl4_unclicked;
		lvl5 = lvl5_unclicked;
		lvl6 = lvl6_unclicked;
		lvl7 = lvl7_unclicked;
		lvl8 = lvl8_unclicked;
		lvl9 = lvl9_unclicked;
	}

	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(0.3f);
		Game.Instance.startNewGame(startLevel);
		enabled = false;
	}
}
