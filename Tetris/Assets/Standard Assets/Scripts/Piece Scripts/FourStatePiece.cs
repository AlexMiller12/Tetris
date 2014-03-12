using UnityEngine;
using System.Collections;

/*
 * This is for Pieces that can be rotated on four sides: L, backwards L, and T
 */

public class FourStatePiece : Piece {
	
//---------------------------------------------------------------------------FIELDS:
		
//--------------------------------------------------------------------------METHODS:
	
	override public void init()
	{
		;	
	}
 
	override public void rotateLeft() 
	{
		rotate(-90);
	}
	
	override public void rotateRight() 
	{
		rotate(90);
	}
	
	private void rotate(int rotate)
	{
		// Play whishy sound
		AudioManager.Instance.rotate();
		// Move out of grid plane so it can rotate freely
		transform.Translate(new Vector3(0, 1, 0), Space.World);
		// Perform rotation
		transform.Rotate(new Vector3(0, rotate, 0), Space.World);
		// Move back into grid plane
		transform.Translate(new Vector3(0, -1, 0), Space.World);		
	}
}
