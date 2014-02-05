using UnityEngine;
using System.Collections;

public class MyMath : MonoBehaviour {
	
//-------------------------------------------------------------CONSTANTS AND FIELDS:
	
	public const float EPSILON = 0.0001f;
	
//--------------------------------------------------------------------------METHODS:
	
	public static bool nearlyEqual (float a, float b)
	{
		return nearlyEqual(a, b, EPSILON);
	}
	
	/* From Norm Badler's slides */
	public static bool nearlyEqual (float a, float b, float epsilon)
	{
		float absA = Mathf.Abs(a);
	    float absB = Mathf.Abs(b);
	    float diff = Mathf.Abs(a - b);
		
		if (a == b) 
		{ 	// shortcut
			return true;
	    } 
		else if (a * b == 0) 
		{ 	// a or b or both are zero -- relative error is not meaningful here
			return diff < (epsilon * epsilon);
	    } 
		else 
		{ 	// use relative error
			return diff / (absA + absB) < epsilon;
	    }
	}
	
	/*
	 * 
	 */
	public static int castFloat (float num)
	{
		return (int)(num + EPSILON);
	}
	
}
