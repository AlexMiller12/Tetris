using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {
	
//---------------------------------------------------------------CONSTANTS & FIELDS:
	
	private const float CLEAR_TIME = 0.1f;
	// Dimensions of grid
	private const int WIDTH = 10, HEIGHT = 20; 
	// Starting square for tetrominos (may need to be individualized)
	private const int START_X = 5, START_Z = 18;
	
	
	public GameObject Line, Back_L, Norm_L, Square, Squig_Left, Squig_Right, T;
	
	private Piece currentPiece, nextPiece;
	private int clock, lines, level, score;
	private bool isGameOver, isClearing;
	
//---------------------------------------------------------------------MONO METHODS:
	
	// TEMP
	void Start () 
	{
		startNewGame(0);
	}
	
	/*
	 * Main loop
	 */
	void FixedUpdate () 
	{
		//---TEMP: (should set fixed update based on level)
		clock++;
		if (clock % 40 == 0)
		{
		//---
			if (! isClearing && ! currentPiece.canLower()) 
			{
				// Add blocks to terrain
				Terrain.Instance.addPiece(currentPiece);		
				// Check if any rows should be cleared
				List<int> linesToClear = Terrain.Instance.checkForClears();
				// Destroy blockless game piece
				Destroy(currentPiece.gameObject);
				
				if (linesToClear.Count > 0)
				{
					// Tell game that it's clearing a row and should wait
					isClearing = true;	
					Debug.Log("Game --- TODO: make lines blink!");
					// Clear lines after done blinking
					StartCoroutine(ClearAndLower(CLEAR_TIME, linesToClear));
				}
				else
				{
					currentPiece = generateNewPiece();
				}
			}
			else if (! isClearing)
			{
				currentPiece.lower();
			}		
			if (isGameOver) 
			{
				//TODO disable game script and show high scores
				Debug.Log("Game Over!");
			}
		}
	
	}
	
	void Update()
	{
		// Check if a player has tried to move/rotate a piece
		checkForKeyInput();
	}
	
//------------------------------------------------------------------------MY METHODS:
	
	/*
	 * Checks for user input and, if necessary, calls methods to manipulate 
	 * currentPiece or pause the game
	 */
	private void checkForKeyInput()
	{
		if (Input.GetKeyDown(KeyCode.Q))  //TODO TEMP!  JUST FOR DEBUGGING
		{
			Debug.Log("Game --- Break here! ");
		}
		
		if (Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
			if (currentPiece.canMoveLeft())
			{
				currentPiece.moveLeft();
			}
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)) 
		{
			if (currentPiece.canMoveRight())
			{
				currentPiece.moveRight();
			}
		}
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			currentPiece.quickDrop();
		}
		if (Input.GetKeyDown(KeyCode.A)) 
		{
			if (currentPiece.canRotateLeft())
			{
				currentPiece.rotateLeft();
			}
		}
		if (Input.GetKeyDown(KeyCode.D)) 
		{
			if (currentPiece.canRotateRight())
			{
				currentPiece.rotateRight();
			}
		}
		if (Input.GetKeyDown(KeyCode.Return)) 
		{
			Debug.Log("TODO: PAUSE");
		}
	}
	
	/*
	 * Will wait for lines to blink before clearing given rows and lowering all
	 * terrain overhead
	 */
	IEnumerator ClearAndLower (float waitTime, List<int> linesToClear)
	{
		// Wait until blocks have finished playing clear animation
		yield return new WaitForSeconds(waitTime);
		
		Debug.Log("Game --- waited for " + waitTime + " seconds!"); //TEMP
		
		Terrain.Instance.clearLines(linesToClear);
		
		//Terrain.Instance.lowerBlcoksAfterClear(linesToClear);
		
		currentPiece = generateNewPiece();
		// If a piece can't lower even once, the tower's too high (game over!)
		isGameOver = ! currentPiece.canLower();		
		// It's OK to lower new piece now
		isClearing = false;
	}

	/*
	 * Randomly generates a new currentPiece
	 */
	private Piece generateNewPiece() 
	{
		Vector3 spawnPoint = new Vector3(START_X, 0, START_Z); 
		GameObject newPiece;
		
		int pieceNumber = Random.Range(1, 7);
		
		switch (pieceNumber)
		{
		case 1:			
			newPiece = Instantiate(Square, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 2:
			newPiece = Instantiate(Back_L, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 3:
			newPiece = Instantiate(Norm_L, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 4:
			newPiece = Instantiate(T, spawnPoint, Quaternion.identity) as GameObject;
			break;
			
		case 5:
			newPiece = Instantiate(Squig_Right, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 6:
			newPiece = Instantiate(Squig_Left, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		default:
			newPiece = Instantiate(Line, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			
			break;
			
			
		}
		return newPiece.GetComponent<Piece>();
	}
	
	/*
	 * Starts a new game at given level
	 */
	public void startNewGame(int level)
	{
		isGameOver = false;
		isClearing = false;
		this.level = level;
		clock = 0;
		score = 0;
		lines = 0;
		Terrain.Instance.resetGrid(WIDTH, HEIGHT);
		currentPiece = generateNewPiece();
	}
	
}
