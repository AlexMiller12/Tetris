using UnityEngine;
using System.Collections;

public abstract class Piece : MonoBehaviour {
	
//---------------------------------------------------------------------------FIELDS:

	
//---------------------------------------------------------------------MONO METHODS:
	
	void Start()
	{
		init();	
	}
	
//--------------------------------------------------------------------------METHODS:

	/*
	 * Returns true if the piece can move down without going outside the boundary
	 * or hitting another piece
	 */
	public bool canLower() 
	{		
		Terrain.Instance.resetGrid(10, 20); //TEMP! TODO: find that bug or get rid of the stupid grid
		//Iterate through cube children, check grid below
		foreach (Transform child in transform)
		{
			// find grid location of block
			int col = Mathf.RoundToInt(child.position.x);
			int row = Mathf.RoundToInt(child.position.z);
			// check the row beneath it
			if (! Terrain.Instance.isLegalAndFree(col, row - 1))
			{
				return false;
			}
		} 
		return true;
	}
	
	/*
	 * Returns true if the piece can move without going outside the boundary
	 * or hitting another piece
	 */
	public bool canMove(int xMovement)
	{
		//Iterate through cube children, check grid below
		foreach (Transform child in transform)
		{
			// find grid location of block
			int col = Mathf.RoundToInt(child.position.x) + xMovement;
			int row = Mathf.RoundToInt(child.position.z);		
			// check the col to the left
			if (! Terrain.Instance.isLegalAndFree(col, row))
			{
				return false;
			}
		}
		return true;
	}
			
	/*
	 * Returns true if the piece can rotate left without going outside the boundary
	 * or hitting another piece
	 */
	public bool canRotateLeft()
	{
		// Rotate left
		rotateLeft();
		// Iterate through cube children, see if any are in illegal spot
		foreach (Transform child in transform)
		{
			if (! Terrain.Instance.isLegalAndFree(child))
			{
				// Rotate back
				rotateRight();
				return false;
			}
		}
		// Rotate back
		rotateRight();
		
		return true;
	}
	
	/*
	 * Returns true if the piece can rotate right without going outside the boundary
	 * or hitting another piece
	 */
	public bool canRotateRight()
	{
		// Rotate left
		rotateRight();
		// Iterate through cube children, see if any are in illegal spot
		foreach (Transform child in transform)
		{
			if (! Terrain.Instance.isLegalAndFree(child))
			{
				// Rotate back
				rotateLeft();
				return false;
			}
		}
		// Rotate back
		rotateLeft();
		
		return true;
	}
	
	/*
	 * Lowers the piece
	 */
	public void lower() 
	{
		if (Terrain.Instance.willFallOnGoldBlock(this))
		{
			Terrain.Instance.mergeWithGoldBlock(this);	
		}
		transform.Translate(new Vector3(0, 0, -1), Space.World);
	}
	
	/*
	 * Moves the piece left
	 */
	public void moveLeft() 
	{
		if (Terrain.Instance.willMoveIntoGoldBlock(this, -1))
		{
			Terrain.Instance.mergeWithGoldBlock(this);	
		}
		transform.Translate(new Vector3(-1, 0, 0), Space.World);
	}
	
	/*
	 * Moves the piece right
	 */
	public void moveRight() 
	{
		if (Terrain.Instance.willMoveIntoGoldBlock(this, 1))
		{
			Terrain.Instance.mergeWithGoldBlock(this);	
		}
		transform.Translate(new Vector3(1, 0, 0), Space.World);
	}
	
	/*
	 * Lowers the piece until it cannot be lowered anymore
	 */
	public void quickDrop()
	{		
		while (canLower())
		{
			lower();
		}
	}
		
//-----------------------------------------------------------------ABSTRACT METHODS:

	public abstract void init();
	
	public abstract void rotateLeft();	
	
	public abstract void rotateRight();
	
	
	private void printBlockPositions()
	{
		Debug.Log("num children: " + transform.GetChildCount());
		foreach (Transform child in transform)
		{
			
			// find grid location of block
			float col = child.position.x;
			float row = child.position.z;
			Debug.Log(col + ", " + row);
		}	
		Debug.Log("------------------");
	}
}
