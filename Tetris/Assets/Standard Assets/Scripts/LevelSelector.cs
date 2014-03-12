using UnityEngine;
using System.Collections;

public class LevelSelector : Singleton<LevelSelector> {

//---------------------------------------------------------------------------FIELDS:
	
	public GUISkin guiSkin;
	
	// The world-space coordinates at which the start and level buttons belong
	private Vector3 startTL = new Vector3(-4.17f, -4.9f, 11.04f);
	private Vector3 startBR = new Vector3(8.13f, -4.9f, 5.03f);
	private Vector3 levelTL = new Vector3(-12.2f, -4.9f, 0.2f);
	private Vector3 levelBR = new Vector3(-9.2f, -4.9f, -2.6f);
	private Vector3 advancedTL = new Vector3(5.63f, -4.9f, 2.2f);
	private Vector3 advancedBR = new Vector3(8.43f, -4.9f, 0f);
	
	// The textures used for clicked state for each button
	public Texture2D start_clicked, lvl0_clicked, lvl1_clicked, lvl2_clicked, 
					 lvl3_clicked, lvl4_clicked, lvl5_clicked, lvl6_clicked, 
					 lvl7_clicked, lvl8_clicked, lvl9_clicked, advanced_clicked;
	
	// The textures used for unclicked state for each button
	public Texture2D start_unclicked, lvl0_unclicked, lvl1_unclicked, lvl2_unclicked, 
					 lvl3_unclicked, lvl4_unclicked, lvl5_unclicked, lvl6_unclicked, 
					 lvl7_unclicked, lvl8_unclicked, lvl9_unclicked, advanced_unclicked;
	
	// The textures that are currently being displayed for each button
	private Texture2D lvl0, lvl1, lvl2, lvl3, lvl4, lvl5, lvl6, lvl7, lvl8, lvl9, 
					  start, advanced;
	
	private int startLevel;
	private bool advancedMode;
	
//---------------------------------------------------------------------MONO METHODS:
	
	void Start()
	{
		startLevel = 0;
		advancedMode = false;
		unclickAllButtons();	
		advanced = advanced_unclicked;
		GUI.enabled = true;
	}
	
	void OnGUI()
	{	
		GUI.skin = guiSkin;
				
		Vector3 tl = OnlyCamera.Instance.camera.WorldToScreenPoint(levelTL);
		Vector3 br = OnlyCamera.Instance.camera.WorldToScreenPoint(levelBR);
		float width = br.x - tl.x;
		float height = tl.y - br.y;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl0))
		{
			unclickAllButtons();
			lvl0 = lvl0_clicked;
			startLevel = 0;
		}
		tl.x += width;
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl1))
		{
			unclickAllButtons();	
			lvl1 = lvl1_clicked;
			startLevel = 1;
		}
		tl.x += width;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl2))
		{
			unclickAllButtons();	
			lvl2 = lvl2_clicked;
			startLevel = 2;
		}
		tl.x += width;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl3))
		{
			unclickAllButtons();	
			lvl3 = lvl3_clicked;
			startLevel = 3;
		}
		tl.x += width;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl4))
		{
			unclickAllButtons();	
			lvl4 = lvl4_clicked;
			startLevel = 4;
		}
		tl.x -= width * 4; 
		tl.y += height;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl5))
		{
			unclickAllButtons();	
			lvl5 = lvl5_clicked;
			startLevel = 5;
		}
		tl.x += width;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl6))
		{
			unclickAllButtons();	
			lvl6 = lvl6_clicked;
			startLevel = 6;
		}
		tl.x += width;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl7))
		{
			unclickAllButtons();	
			lvl7 = lvl7_clicked;
			startLevel = 7;
		}
		tl.x += width;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl8))
		{
			unclickAllButtons();	
			lvl8 = lvl8_clicked;
			startLevel = 8;
		}
		tl.x += width;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), lvl9))
		{
			unclickAllButtons();	
			lvl9 = lvl9_clicked;
			startLevel = 9;
		}	 	

		tl = OnlyCamera.Instance.camera.WorldToScreenPoint(startTL);
		br = OnlyCamera.Instance.camera.WorldToScreenPoint(startBR);
		width = br.x - tl.x;
		height = tl.y - br.y;

		if (GUI.Button (new Rect(tl.x, tl.y, width, height), start))
		{
			start = start_clicked;
			StartCoroutine( StartGame() ) ;
		}

		tl = OnlyCamera.Instance.camera.WorldToScreenPoint(advancedTL);
		br = OnlyCamera.Instance.camera.WorldToScreenPoint(advancedBR);
		width = br.x - tl.x;
		height = tl.y - br.y;
		
		if (GUI.Button (new Rect(tl.x, tl.y, width, height), advanced))
		{
			toggleAdvancedMode();
		}

	}
	
//-----------------------------------------------------------------------MY METHODS:
	
	IEnumerator StartGame()
	{
		// Wait a sec so we can see the start button clicked
		yield return new WaitForSeconds(0.3f);
		// Start a new game
		GameManager.Instance.goToGame(startLevel, advancedMode);
		// Unclick the buttons so they're ready when we get back
		unclickAllButtons();
		// Disable so buttons disappear
		enabled = false; 
	}
	
	private void toggleAdvancedMode()
	{
		if (advancedMode)
		{
			advanced = advanced_unclicked;
		}
		else
		{
			advanced = advanced_clicked;	
		}
		advancedMode = ! advancedMode;		
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
		start = start_unclicked;
	}
}
