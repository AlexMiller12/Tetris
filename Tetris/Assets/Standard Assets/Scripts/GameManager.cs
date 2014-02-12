using UnityEngine;
using System.Collections;

/*
 
 TODO:
 	-Texture Atlasing
 
 
 */

public class GameManager : MonoBehaviour {
	
//---------------------------------------------------------------------------FIELDS:
	
	
	
//---------------------------------------------------------------------MONO METHODS:
	
	// Use this for initialization
	void Start () {
		Game.Instance.startNewGame(5);
	}
	
	// Update is called once per frame 
	void Update () {
	
	}	
	
//-----------------------------------------------------------------------MY METHODS:
	
	public void startNewGame(int level)
	{
		
	}
	
	/*
	 * Considers player's score for high score list and switches scenes to High_Score
	 */
	public void endCurrentGame(int score)
	{
		
	}
	
}
