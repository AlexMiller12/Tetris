using UnityEngine;
using System.Collections;

public class T : Piece {
	
//---------------------------------------------------------------------------FIELDS:
	
	
	
//--------------------------------------------------------------------------METHODS:
	 
	override public void init()
	{
		;	
	}
 
	override public void rotateLeft() 
	{
		rotate(90);
	}
	
	
	override public void rotateRight() 
	{
		rotate(-90);
	}
	
	private void rotate(int rotate)
	{
		// Move out of grid plane so it can rotate freely
		transform.Translate(new Vector3(0, 1, 0), Space.World);
		// Perform rotation
		transform.Rotate(new Vector3(0, rotate, 0), Space.World);
		// Move back into grid
		transform.Translate(new Vector3(0, -1, 0), Space.World);		
	}
}
