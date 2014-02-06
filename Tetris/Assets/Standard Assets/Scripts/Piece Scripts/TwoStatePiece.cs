using UnityEngine;
using System.Collections;

/*
 * This is for those Pieces that can be rotated on two sides, such as the Squig, 
 * Squig Left, and Line piece.
 */


public class TwoStatePiece : Piece {

//---------------------------------------------------------------------------FIELDS:
	
	bool isFlipped;
	
//--------------------------------------------------------------------------METHODS:
		
	override public void init()
	{
		isFlipped = false;	
	}
 
	override public void rotateLeft() 
	{
		int rotate = 90;
		// Move out of grid plane so it can rotate freely
		transform.Translate(new Vector3(0, 1, 0), Space.World);
		// If already rotated, gotta rotate it back
		if (isFlipped)   rotate *= -1;
		// Perform rotation
		transform.Rotate(new Vector3(0, rotate, 0), Space.World);
		// Move back into grid
		transform.Translate(new Vector3(0, -1, 0), Space.World);
		// Toggle flipped status
		isFlipped = ! isFlipped;
	}
	
	
	override public void rotateRight() 
	{
		rotateLeft();
	}
}
