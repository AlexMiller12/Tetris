using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Terrain : Singleton<Terrain> {

//---------------------------------------------------------------------------FIELDS:
	
	private Transform[,] blocks;
	private bool[,] occupied; //TODO private Transform[,] 
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
			int col = MyMath.castFloat(child.position.x);
			int row = MyMath.castFloat(child.position.z);
			// Mark as occupied on the grid
			Terrain.Instance.blocks[col, row] = child;
			// Remove block from piece and add to terrain
			child.parent = transform;
		}
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
			int col = MyMath.castFloat(child.position.x);
			int row = MyMath.castFloat(child.position.z);
			
			// Put block on to-destroy list
			if (rowNumbers.Contains(row))
			{
				// Old square is unoccupied unless something falls on it
				blocks[col, row] = null;
				blocksToDestroy.Add(child);
			}
		}
		// Destroy all blocks
		for (int i = 0; i < blocksToDestroy.Count; i++) 
		{
			Destroy(blocksToDestroy[i].gameObject);
		}
		lowerBlocksAfterClear(rowNumbers);
	}
	
	/*
	 * Lowers all blocks above given rows one square for each cleared row 
	 * beneath it
	 */
	private void lowerBlocksAfterClear(List<int> rowsCleared)
	{
		foreach (Transform block in transform)
		{
			int blockRow = 	MyMath.castFloat(block.position.z);
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
				int blockCol = MyMath.castFloat(block.position.x);
								
				blocks[blockCol, blockRow - toLower] = block;
				blocks[blockCol, blockRow] = null;
				
				block.Translate(new Vector3(0, 0, -toLower), Space.World);
			}
		}
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
		int col = MyMath.castFloat(child.position.x);
		int row = MyMath.castFloat(child.position.z);
		
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
		occupied = new bool[width, height];	
		blocks = new Transform[width, height];
	}

}
