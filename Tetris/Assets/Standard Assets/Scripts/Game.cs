using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 TODO:
	-get sounds for merging with gold block
	-end game camera revert to default is screwy; fix?
	-add light on top of 3d pieces.
	-maybe make line clear sound louder and more jarring.
		-play with scale when clearing (make them bigger [wchew wchew])
	-High Scores
	-Button for gold block mode
*/

public class Game : Singleton<Game> {
	
//---------------------------------------------------------------CONSTANTS & FIELDS:
	
	private const int LINES_PER_LEVEL = 10;
	private const float CLEAR_TIME = 0.5f; // time it takes to play clear animation
	// Dimensions of grid
	private const int WIDTH = 10, HEIGHT = 20; 
	// Chance of generating a gold block on terrain
	private const int PIECES_PER_GOLD_BLOCK = 3;
	// Buffer zones required between top of terrain and ceiling
	private const int GB_UPPER_BUFFER = 4;
	private const int GB_LOWER_BUFFER = 2;
	private const int GB_SIDE_BUFFER = 1;
	// Position at which next piece will be displayed
	private Vector3 NEXT_PIECE_SPAWN = new Vector3(18, 0, 16.6f);
	// Position from which tetrominos will begin to fall
	private Vector3 FALLING_PIECE_SPAWN = new Vector3(5, 0, 18);
	// The current piece's fall speed is based on level, which indexes into array
	private int[] ticksPerLower = {44, 39, 34, 29, 24, 20, 16, 12, 8, 5};
	// level 5 = 20, 9 = 5
	public GameObject LinePrefab, BackLPrefab, NormLPrefab, SquarePrefab, 
					  SquigLeftPrefab, SquigRightPrefab, TPrefab, GoldBlockPrefab;
	 
	private Piece currentPiece, nextPiece;
	private int clock, lines, level, score;
	private bool isClearing, topView, advancedMode;
	 
//---------------------------------------------------------------------MONO METHODS:
	
	/*
	 * Main game loop
	 */
	void FixedUpdate () 
	{
		clock++;
		if (clock % ticksPerLower[level] == 0)
		{
			if (! isAnimationPlaying() && ! currentPiece.canLower()) 
			{
				// Check to add gold block
				checkToCreateGoldBlock();
				// Play sound
				AudioManager.Instance.land();
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
			}
			else if (! isClearing && currentPiece.canLower())
			{		
				currentPiece.lower();
			}		
		}
	}
	
	void Update()
	{
		// Check if a player has tried to move/rotate a piece
		checkForKeyInput();
	}
	
//-----------------------------------------------------------------------MY METHODS:
	
	/*
	 * Checks for user input and, if necessary, calls methods to manipulate 
	 * currentPiece or pause the game
	 */
	private void checkForKeyInput()
	{
		if (Input.GetKeyDown(KeyCode.Q))  //TODO TEMP!  JUST FOR DEBUGGING
		{
			Debug.Log("Game --- Break here!");
		}
		if (Input.GetKeyDown(KeyCode.C)) 
		{
			moveCamera();
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
			if (currentPiece.canMove(-1))
			{
				currentPiece.moveLeft();
			}
		}
		if (Input.GetKeyDown(KeyCode.RightArrow)) 
		{
			if (currentPiece.canMove(1))
			{
				currentPiece.moveRight();
			}
		}
		if (Input.GetKeyDown(KeyCode.Space)) 
		{
			AudioManager.Instance.drop();
			while (currentPiece.canLower())
			{
				currentPiece.lower();
			}
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
			Debug.Log("PAUSE"); //TODO pause!
		}
	}
			
	/*
	 * Decides whether or not to generate a new Gold Block
	 */
	private void checkToCreateGoldBlock()
	{
		// We only want one gold block at a time
		if (! advancedMode || Terrain.Instance.hasGoldBlock())
		{
			return;	
		}
		if (Random.Range(0, PIECES_PER_GOLD_BLOCK) == 1)
		{
			int terrainPeak = Terrain.Instance.getHighestBlockRow();
			int minRow = terrainPeak + GB_LOWER_BUFFER;
			int maxRow = HEIGHT - GB_UPPER_BUFFER;
			// We need enough room between ceiling and tower to gen a gold block
			if (minRow >= maxRow)
			{
				return;	
			}
			int minCol = GB_SIDE_BUFFER;
			int maxCol = WIDTH - GB_SIDE_BUFFER;
			int row = Random.Range(minRow, maxRow + 1); //add one, int is not max inclusive
			int col = Random.Range(minCol, maxCol); 
			
			GameObject goldBlock = Instantiate(GoldBlockPrefab,
											   new Vector3(col, 0, row),
											   Quaternion.identity) as GameObject;
			
			// TODO play sound!
			
			Terrain.Instance.setGoldBlock(goldBlock);
		}
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
	
	/*
	 * Will wait for lines to blink before clearing given rows and lowering all
	 * terrain overhead.  Updates game state
	 */
	IEnumerator Clear (float waitTime, List<int> linesToClear)
	{
		AudioManager.Instance.lineClear();
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
	
	/*
	 * Cleans up the game and goes to high scores
	 */
	IEnumerator EndGame()
	{
		// Disable this script so pieces stop falling
		enabled = false;
		// Stop game music
		AudioManager.Instance.gameMusicSound.Stop();
		// Play game over sound
		AudioManager.Instance.gameOver();
		// Find how long that sound takes to play
		float waitTime = AudioManager.Instance.loseSound.length;
		// Wait that long
		yield return new WaitForSeconds(waitTime);
		// Clear Terrain
		Terrain.Instance.clearTerrain();
		GameObject.Destroy(Terrain.Instance.gameObject);
			
		// Switch "scenes"
		GameManager.Instance.goToHighScores(score, level);
		// Return to default view if necessary
		if (topView)
		{
			OnlyCamera.Instance.animation.CrossFade("CameraToDefaultView");	
		}
		// TODO set inactive in game manager so score and line counts dissapear and
		// we don't have to do destroy these
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
			newPiece = Instantiate(SquarePrefab, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 2:
			newPiece = Instantiate(BackLPrefab, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 3:
			newPiece = Instantiate(NormLPrefab, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 4:
			newPiece = Instantiate(TPrefab, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 5:
			newPiece = Instantiate(SquigRightPrefab, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 6:
			newPiece = Instantiate(SquigLeftPrefab, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			break;
			
		case 7:
			newPiece = Instantiate(LinePrefab, 
								   spawnPoint, 
								   Quaternion.identity) as GameObject;
			
			break;	
			
		default:
			return null;
		}		
		return newPiece.GetComponent<Piece>();
	}
		
	/*
	 * Returns true if something's happening that shouldn't be interrupted
	 */
	private bool isAnimationPlaying()
	{
		// Are lines being cleared?
		if (isClearing)   return true;
		// Is a piece flying into next piece zone?
		if (nextPiece.animation.isPlaying)   return true;
		// All clear!
		return false;
	}
	
	/*
	 * Moves the camera
	 */
	private void moveCamera()
	{
		if (topView)
		{
			OnlyCamera.Instance.animation.CrossFade("CameraToDefaultView");	
			topView = false;
		}
		else
		{
			OnlyCamera.Instance.animation.CrossFade("CameraToTopView");	
			topView = true;
		}	
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
		case 5:
			return 5000 * (level + 1);
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
		
		if (! currentPiece.canLower())
		{
			StartCoroutine(EndGame());	
		}
	}
	
	/*
	 * Starts a new game at given level
	 */
	public void startNewGame(int startLevel, bool advancedMode)
	{
		this.advancedMode = advancedMode;
		enabled = true;
		isClearing = false;
		topView = false;
		level = startLevel;
		clock = 0;
		score = 0;
		lines = 0;
		Terrain.Instance.resetGrid(WIDTH, HEIGHT);
		nextPiece = generatePiece(ref NEXT_PIECE_SPAWN);
		setCurrentPiece();
		
		AudioManager.Instance.gameMusic();
		
		setText("Score", 0);
		setText("Lines", 0);
		
		Background.Instance.level(startLevel);
	}	
	
	/*
	 * Finds and sets 3d text with textName to textValue
	 */
	private void setText(string textName, int textValue)
	{
		GameObject textObj = GameObject.Find(textName);
		TextMesh mesh = textObj.GetComponent<TextMesh>();
		mesh.text = "" + textValue;
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
}
