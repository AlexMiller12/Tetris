using UnityEngine;
using System.Collections;

public class Background : Singleton<Background> {
	
//---------------------------------------------------------------------------FIELDS:
		
	public Material lvl0, lvl1, lvl2, lvl3, lvl4, lvl5, lvl6, lvl7, lvl8, lvl9;
	public Material high_score, level_select;

//-----------------------------------------------------------------------MY METHODS:
	
	/*
	 * Changes background texture to high scores
	 */
	public void highScores()
	{
		renderer.material = high_score;
	}

	/*
	 * Changes background texture to the given level
	 */
	public void level(int level)
	{
		switch(level)
		{
		case 0:
			renderer.material = lvl0;
			break;
		case 1:
			renderer.material = lvl1;
			break;
		case 2:
			renderer.material = lvl2;
			break;
		case 3:
			renderer.material = lvl3;
			break;
		case 4:
			renderer.material = lvl4;
			break;
		case 5:
			renderer.material = lvl5;
			break;
		case 6:
			renderer.material = lvl6;
			break;
		case 7:
			renderer.material = lvl7;
			break;
		case 8:
			renderer.material = lvl8;
			break;
		case 9:
			renderer.material = lvl9;
			break;
		}
	}
		
	/*
	 * Changes background texture to level select
	 */
	public void levelSelect()
	{
		renderer.material = level_select;	
	}
}
