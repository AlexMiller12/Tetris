using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Terrain : Singleton<Terrain> {

//---------------------------------------------------------------------------FIELDS:
	
	private bool[,] occupied; 
		
//--------------------------------------------------------------------------METHODS:
	IEnumerator ClearAndLower (float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		Debug.Log("Game --- waited for " + waitTime + " seconds!");
	}
	
	
	
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
			int col = (int)(child.position.x + MyMath.EPSILON);
			int row = (int)(child.position.z + MyMath.EPSILON);
			// Mark as occupied on the grid
			Terrain.Instance.occupied[col, row] = true;
			// Remove block from piece and add to terrain
			child.parent = transform;
		}

		Destroy(piece.gameObject);
	}
	
	/*
	 * Clears all full rows.  Returns the number of lines cleared.
	 */
	public int checkForClears()
	{
		// start at top, move down
		// mark lines for clearing
		// set Game.Instance.isClearing = true
		// start coroutine to clear
		//StartCoroutine(TryingACoroutine(5.0f));
		Debug.Log("Terrain --- TODO: checkForClears()");
		return -1;
	}
	
	/*
	 * Clears the given rows and lowers all blocks above one square for each row
	 * cleared
	 */
	public void clearLines(params int[] rowNumbers)
	{
		for (int i = 0; i < rowNumbers.Length; i++)
		{
				
		}
		Debug.Log("Terrain --- TODO: clearLines()");
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
			return ! occupied[col, row];	
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
		int col = (int)(child.position.x + MyMath.EPSILON);
		int row = (int)(child.position.z + MyMath.EPSILON);
		
		return isLegalAndFree(col, row);
	}
		
	/*
	 * Sets the occupied field to the given dimensions, initializes it to false,
	 * and destroys all cubes in terrain
	 */
	public void resetGrid (int width, int height)
	{
		occupied = new bool[width, height];	
	}

}
