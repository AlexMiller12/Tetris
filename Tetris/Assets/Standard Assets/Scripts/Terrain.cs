using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 TODO:	
 	-Check for lateral intersections with Gold Block
 */
public class Terrain : Singleton<Terrain> {

//---------------------------------------------------------------------------FIELDS:
	
	private Transform[,] blocks;
	private int gridWidth, gridHeight;
	private GameObject goldBlock; 
		
//---------------------------------------------------------------------MONO METHODS:
	
	void Start()
	{
		goldBlock = null;	
	}
	
//--------------------------------------------------------------------------METHODS:
 	
   /*
	* Adds the Piece to level terrain 
	*/
	public void addPiece (Piece piece) 
	{
		while (piece.transform.childCount > 0)
		{
			// Get the first child (cube)
			Transform child = piece.transform.GetChild(0);
			// Find grid location of block
			int col = Mathf.RoundToInt(child.position.x);
			int row = Mathf.RoundToInt(child.position.z);
			// Mark as occupied on the grid
			blocks[col, row] = child;
			// Remove block from piece and add to terrain
			child.parent = transform;
		}
		// Destroy blockless game piece
		Destroy(piece.gameObject);
	}

	/*
	 * Clears all full rows.  Returns List of lines cleared.
	 */
	public List<int> checkForClears()
	{
		List<int> linesToClear = new List<int>();
		
		for (int i = 0; i < gridHeight; i++)
		{
			if (isGridRowFull(i))
			{
				linesToClear.Add(i);
			}
		}		
		return linesToClear;
	}
	
	/*
	 * Clears the given rows and lowers all blocks above one square for each row
	 * cleared
	 */
	public void clearLines(List<int> rowNumbers)
	{
		List<Transform> blocksToDestroy = new List<Transform>();

		foreach (Transform child in transform)
		{
			// Get the row of the block
			int row = Mathf.RoundToInt(child.position.z);
			// Put block on to-destroy list if it's on death row (yuk yuk)
			if (rowNumbers.Contains(row))
			{
				blocksToDestroy.Add(child);
			}
		}
		// Destroy all blocks on the hit list
		for (int i = 0; i < blocksToDestroy.Count; i++) 
		{
			Destroy(blocksToDestroy[i].gameObject);
		}
		lowerBlocksAfterClear(rowNumbers);
	}
	
	/*
	 * Destroys all blocks in terrain
	 */
	public void clearTerrain()
	{
		for (int i = 0; i < gridWidth; i++)
		{
			for (int j = 0; j < gridHeight; j++)
			{
				Transform block = blocks[i, j];
				if (block != null)
				{
					GameObject.Destroy(block.gameObject);
				}
			}
		}
		destroyGoldBlock();
	}
	
	/*
	 * Lowers all blocks above given rows one square for each cleared row 
	 * beneath it
	 */
	private void lowerBlocksAfterClear(List<int> rowsCleared)
	{
		// Physically lower all blocks
		foreach (Transform block in transform)
		{
			int blockRow = 	Mathf.RoundToInt(block.position.z);
			int toLower = 0;
			
			foreach (int row in rowsCleared)
			{
				if (blockRow > row)
				{
					// Lower each block one square for each row cleared beneath it
					toLower++;
				}
			}
			if (toLower > 0)
			{		
				block.Translate(new Vector3(0, 0, -toLower), Space.World);
			}
		}
		// Update block positions on grid
		resetGrid (gridWidth, gridHeight);
	}
	
	/*
	 * Returns true if the spot at col, row is both within boundaries of grid and
	 * unoccupied by another block
	 */
	public bool isLegalAndFree (int col, int row)
	{
		try
		{
			// If spot is unoccupied, spot is legal and free
			return blocks[col, row] == null;
		}
		catch (IndexOutOfRangeException e)
		{
			// If it doesn't exist on grid, it's off screen and illegal
			return false;	
		}
	}
	
	/*
	 * Returns true if the child is both within boundaries of grid and unoccupied 
	 * by another block
	 */
	public bool isLegalAndFree (Transform child)
	{
		// find grid location of block
		int col = Mathf.RoundToInt(child.position.x);
		int row = Mathf.RoundToInt(child.position.z);
		
		return isLegalAndFree(col, row);
	}
	
	/*
	 * Returns true if the entire row is filled with blocks
	 */
	private bool isGridRowFull(int row)
	{
		for (int i = 0; i < gridWidth; i++)
		{
			if (blocks[i, row] == null) 
			{
				return false;	
			}
		}
		return true;
	}
		
	/*
	 * Sets the occupied field to the given dimensions, initializes it to false,
	 * and destroys all cubes in terrain
	 */
	public void resetGrid (int width, int height)
	{
		gridWidth = width;
		gridHeight = height;
		blocks = new Transform[width, height];
		// Populate it with fallen blocks (if they exist)
		foreach (Transform block in transform)
		{
			// Get the col and row of the block
			int col = Mathf.RoundToInt(block.position.x);
			int row = Mathf.RoundToInt(block.position.z);
			try
			{
				blocks[col, row] = block;
			}
			catch (Exception e)
			{
				Debug.Log("col, row " + col + ", " + row + " is no good!");	
				Debug.Log("Caught Exception: \n" + e.ToString());
			}
		}
	}
	
//-------------------------------------------------------------GOLDEN BLOCK METHODS:
	
	/*
	 * Destroys the gold block if it exists
	 */
	private void destroyGoldBlock()
	{
		if (goldBlock != null)
		{
			GameObject.Destroy(goldBlock);	
		}
	}
	
	/*
	 * Returns the highest row that has a block in it
	 */
	public int getHighestBlockRow()
	{
		// Start at top and work down
		for (int row = gridHeight - 1; row > 0; row--)
		{
			for (int col = 0; col < gridWidth; col++)
			{
				if (blocks[col, row] != null)
				{
					return row;	
				}
			}
		}
		// If we didn't find anything, return lowest row
		return 0;
	}

	/*
	 * Returns true if there's a golden block in terrain
	 */
	public bool hasGoldBlock()
	{
		return goldBlock != null;
	}
	
	/*
	 * Returns true if the piece occupies the space right above the golden block
	 */
	public bool willFallOnGoldBlock(Piece piece)
	{
		if (goldBlock == null)
		{
			return false;	
		}
		// Get grid location of golden block
		int goldCol = Mathf.RoundToInt(goldBlock.transform.position.x);
		int goldRow = Mathf.RoundToInt(goldBlock.transform.position.z);
		
		foreach (Transform block in piece.transform)
		{
			// Find grid location of each block
			int col = Mathf.RoundToInt(block.position.x);
			int row = Mathf.RoundToInt(block.position.z);
			// See if it occupies spot above Golden Block
			if (col == goldCol && row == (goldRow + 1))
			{
				return true;
			}
		}
		return false;
	}
	
	/*
	 * Returns true if the piece will move into Gold Block
	 */
	public bool willMoveIntoGoldBlock(Piece piece, int xMovement)
	{
		if (goldBlock == null)
		{
			return false;	
		}
		// Get grid location of golden block
		int goldCol = Mathf.RoundToInt(goldBlock.transform.position.x);
		int goldRow = Mathf.RoundToInt(goldBlock.transform.position.z);
		
		foreach (Transform block in piece.transform)
		{
			// Find grid location of each block
			int col = Mathf.RoundToInt(block.position.x);
			int row = Mathf.RoundToInt(block.position.z);
			// See if it occupies spot next to GB
			if (col == (goldCol - xMovement) && row == goldRow)
			{
				return true;
			}
		}
		return false;
	}
	
	/*
	 * Merges piece with gold block
	 */
	public void mergeWithGoldBlock(Piece piece)
	{
		// Give the gold block a new parent
		goldBlock.transform.parent = piece.transform;
		goldBlock = null;
		AudioManager.Instance.merge();
	}
	
	/*
	 * Returns true if the piece occupies the space right above the golden block
	 */
	private bool overlapsGoldBlock(Piece piece)
	{
		return false;
		// Get grid location of golden block
		int goldCol = Mathf.RoundToInt(goldBlock.transform.position.x);
		int goldRow = Mathf.RoundToInt(goldBlock.transform.position.z);
		
		foreach (Transform block in piece.transform)
		{
			// Find grid location of each block
			int col = Mathf.RoundToInt(block.position.x);
			int row = Mathf.RoundToInt(block.position.z);
			// See if it occupies spot above Golden Block
			if (col == goldCol && row == goldRow)
			{
				return true;
			}
		}
		return false;		
	}
		
	/*
	 * Sets the current goldBlock
	 */
	public void setGoldBlock(GameObject block)
	{
		goldBlock = block;
	}
}
