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
		//Iterate through cube children, check grid below
		foreach (Transform child in transform)
		{
			// find grid location of block
			int col = MyMath.castFloat(child.position.x);
			int row = MyMath.castFloat(child.position.z);
			// check the row beneath it
			if (! Terrain.Instance.isLegalAndFree(col, row - 1))
			{
				return false;
			}
		} 
		return true;
	}
	
	/*
	 * Returns true if the piece can move left without going outside the boundary
	 * or hitting another piece
	 */
	public bool canMoveLeft()
	{
		//Iterate through cube children, check grid below
		foreach (Transform child in transform)
		{
			// find grid location of block
			int col = (int)(child.position.x + MyMath.EPSILON);
			int row = (int)(child.position.z + MyMath.EPSILON);		
			// check the col to the left
			if (! Terrain.Instance.isLegalAndFree(col - 1, row))
			{
				return false;
			}
		}
		return true;
	}
	
	/*
	 * Returns true if the piece can move right without going outside the boundary
	 * or hitting another piece
	 */
	public bool canMoveRight()
	{
		//Iterate through cube children, check grid below
		foreach (Transform child in transform)
		{
			// find grid location of block
			int col = (int)(child.position.x + MyMath.EPSILON);
			int row = (int)(child.position.z + MyMath.EPSILON);
			// check the col to the right
			if (! Terrain.Instance.isLegalAndFree(col + 1, row))
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
		transform.Translate(new Vector3(0, 0, -1), Space.World);
	}
	
	/*
	 * Moves the piece left
	 */
	public void moveLeft() 
	{
		transform.Translate(new Vector3(-1, 0, 0), Space.World);
	}
	
	/*
	 * Moves the piece right
	 */
	public void moveRight() 
	{
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
	
}
