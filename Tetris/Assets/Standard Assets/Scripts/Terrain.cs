using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Terrain : Singleton<Terrain> {

//---------------------------------------------------------------------------FIELDS:
	
	private Transform[,] blocks;
	private int gridWidth, gridHeight;
		
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
			// Get the col and row of the block
			int col = Mathf.RoundToInt(child.position.x);
			int row = Mathf.RoundToInt(child.position.z);
			
			// Put block on to-destroy list
			if (rowNumbers.Contains(row))
			{
				// Old square is unoccupied unless something falls on it
				blocks[col, row] = null;
				blocksToDestroy.Add(child);
			}
		}
		if (blocksToDestroy.Count != rowNumbers.Count * 10) //TEMP
		{
			Debug.Log("Terrain --- ERROR 3");
		}
		// Destroy all blocks
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
					toLower++;
				}
			}
			if (toLower > 0)
			{		
				block.Translate(new Vector3(0, 0, -toLower), Space.World);
			}
		}
		// Update grid
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
			}
		}
	}

}
