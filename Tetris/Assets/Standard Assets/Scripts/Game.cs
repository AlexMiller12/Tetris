using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : Singleton<Game> {
	
//---------------------------------------------------------------CONSTANTS & FIELDS:
	
	private const int LINES_PER_LEVEL = 10;
	private const float CLEAR_TIME = 0.5f; // time it takes to play clear animation
	// Dimensions of grid
	private const int WIDTH = 10, HEIGHT = 20; 
	// Starting square for tetrominos (may need to be individualized)
	private const int START_X = 5, START_Z = 18;
	// Position at which next piece will be displayed
	private Vector3 NEXT_PIECE_SPAWN = new Vector3(18, 0, 16.6f);
	// Position from which tetrominos will begin to fall
	private Vector3 FALLING_PIECE_SPAWN = new Vector3(5, 0, 18);
	// The current piece's fall speed is based on level, which indexes into array
	private int[] ticksPerLower = {44, 39, 34, 29, 24, 20, 16, 12, 8, 5};
	// level 5 = 20, 9 = 5
	public GameObject Line, Back_L, Norm_L, Square, Squig_Left, Squig_Right, T;
		
	private Piece currentPiece, nextPiece;
	private int clock, lines, level, score;
	private bool isGameOver, isClearing;
 
//---------------------------------------------------------------------MONO METHODS:
	
	/*
	 * Main loop
	 */
	void FixedUpdate () 
	{
		clock++;
		if (clock % ticksPerLower[level] == 0)
		{
			if (! isAnimationPlaying() && ! currentPiece.canLower()) // Piece at bottom
			{
				// Add blocks to terrain
				Terrain.Instance.addPiece(currentPiece);		
				// Check if any rows should be cleared
				List<int> linesToClear = Terrain.Instance.checkForClears();
				
				if (linesToClear.Count > 0)
				{
					// Tell game that it's clearing a row and should wait
					isClearing = true;	
					// Clear lines after done blinking
					StartCoroutine( Clear(CLEAR_TIME, linesToClear) );
				}
				// Generate new next piece and start dropping old next
				setCurrentPiece();
				
				Terrain.Instance.resetGrid(WIDTH, HEIGHT);
			}
			else if (! isClearing && currentPiece.canLower())
			{
				currentPiece.lower();
			}		
			if (isGameOver) 
			{
				endGame();
			}
		}
	}
	
	void Update()
	{
		// Check if a player has tried to move/rotate a piece
		checkForKeyInput();
	}
	
//-----------------------------------------------------------------------MY METHODS:
	
	private void addToScore(int numLinesCleared)
	{
		score += scoreLineClear(numLinesCleared);
	}
	
	/*
	 * Plays wiggle animation on each terrain block that's about to be cleared
	 */
	private void wiggleClearingBlocks(List<int> linesToClear)
	{
		Vector3 spin = new Vector3(0, 0, 20);
		for (int i = 0; i < 10; i++)
		{
			foreach (Transform child in Terrain.Instance.transform)
			{
				// Get the row of the block
				int row = Mathf.RoundToInt(child.position.z);
				// If it's going to be cleared, animate it!
				if (linesToClear.Contains(row))
				{
					//child.animation.Play();
					child.transform.Rotate(spin);
				}
			}	
		}
	}
	
	/*
	 * Checks for user input and, if necessary, calls methods to manipulate 
	 * currentPiece or pause the game
	 */
	private void checkForKeyInput()
	{
		if (Input.GetKeyDown(KeyCode.Q))  //TODO TEMP!  JUST FOR DEBUGGING
		{
			Debug.Log("Game --- Break here!");
			int i = 0;
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
	 * terrain overhead.  Updates game state
	 */
	IEnumerator Clear (float waitTime, List<int> linesToClear)
	{
		wiggleClearingBlocks(linesToClear);
		// Wait until blocks have finished playing clear animation
		yield return new WaitForSeconds(waitTime);
		Terrain.Instance.clearLines(linesToClear);
		// It's OK to lower new piece now
		isClearing = false;
				
		int numLinesCleared = linesToClear.Count;
		lines += numLinesCleared;
		score += scoreLineClear(numLinesCleared);
		
		checkToIncreaseLevel();
		// Display updated score and lines count
		setText("Score", score);
		setText("Lines", lines);
	}
	
	private void endGame()
	{
		// Disable this script so pieces stop falling
		enabled = false;
		// Clear Terrain
		Terrain.Instance.clearTerrain();
		// Disable Game
		GameManager.Instance.goToHighScores(score);
		GameObject.Destroy(nextPiece.gameObject);
		GameObject.Destroy(currentPiece.gameObject);
	}
	
	/*
	 * Randomly generates a new currentPiece
	 */
	private Piece generatePiece(ref Vector3 spawnPoint) 
	{
		GameObject newPiece;
		
		int pieceNumber = Random.Range(1, 8);
		
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
			
		case 7:
			newPiece = Instantiate(Line, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			
			break;	
			
		default:
			return null;
		}		
		return newPiece.GetComponent<Piece>();
	}
		
	private bool isAnimationPlaying()
	{
		if (isClearing)   return true;
		if (nextPiece.animation.isPlaying)   return true;
		return false;
	}
	
	/*
	 * Scores based on Nintendo version (minus points for soft drops, as we don't
	 * allow soft drops here)
	 * 
	 */
	private int scoreLineClear(int numLinesCleared)
	{
		switch (numLinesCleared)
		{
		case 1:
			return 40 * (level + 1);
		case 2:
			return 100 * (level + 1);
		case 3:
			return 300 * (level + 1);
		case 4:
			return 1200 * (level + 1);
		default:
			return 0;
		}
	}
	
	private void setCurrentPiece()
	{
		currentPiece = nextPiece;
		currentPiece.name = "Current Piece";
		currentPiece.transform.position = FALLING_PIECE_SPAWN;

		nextPiece = generatePiece(ref NEXT_PIECE_SPAWN);
		nextPiece.name = "Next Piece";
		nextPiece.animation.Play();
		isGameOver = ! currentPiece.canLower();
	}
	
	/*
	 * Starts a new game at given level
	 */
	public void startNewGame(int startLevel)
	{
		enabled = true;
		isGameOver = false;
		isClearing = false;
		level = startLevel;
		clock = 0;
		score = 0;
		lines = 0;
		Terrain.Instance.resetGrid(WIDTH, HEIGHT);
		nextPiece = generatePiece(ref NEXT_PIECE_SPAWN);
		setCurrentPiece();
		
		setText("Score", 0);
		setText("Lines", 0);
		
		Background.Instance.level(startLevel);
	}	
	
	private void setText(string textName, int textValue)
	{
		GameObject textObj = GameObject.Find(textName);
		TextMesh mesh = textObj.GetComponent<TextMesh>();
		mesh.text = "" + textValue;
	}
	
	/*
	 * Updates current level based on number of lines;
	 */
	private void checkToIncreaseLevel()
	{
		// Figure out when we need to increment level
		int linesForNextLevel = (level + 1) * LINES_PER_LEVEL;
		
		if (lines == linesForNextLevel && level < 9) // no levels after 9
		{
			level++;
			Background.Instance.level(level);
		}
	}
}
