using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class AudioManager : Singleton<AudioManager> {
	
//---------------------------------------------------------------------------FIELDS:
	
	public AudioClip rotateSound, dropSound, lineClearSound, loseSound, mergeSound,
					 addToTerrainSound;
	
	public float rotateVol, dropVol, lineClearVol, loseVol, mergeVol, landVol;
	
	public AudioSource gameMusicSound, menuMusicSound; 

//--------------------------------------------------------------------------METHODS:

	public void land()
	{
		AudioSource.PlayClipAtPoint(addToTerrainSound, new Vector3(0, 0, 0), landVol);	
	}
	
	public void drop()
	{
		AudioSource.PlayClipAtPoint(dropSound, new Vector3(0, 0, 0), dropVol);	
	}
	
	public void lineClear()
	{
		AudioSource.PlayClipAtPoint(lineClearSound, new Vector3(0, 0, 0), lineClearVol);		
	}
		
	public void gameOver()
	{
		AudioSource.PlayClipAtPoint(loseSound, new Vector3(0, 0, 0), loseVol);		
	}
		
	public void rotate()
	{
		AudioSource.PlayClipAtPoint(rotateSound, new Vector3(0, 0, 0), rotateVol);	
	}
	
	public void merge()
	{
		AudioSource.PlayClipAtPoint(mergeSound, new Vector3(0, 0, 0), mergeVol);	
	}
	
	public void menuMusic()
	{
		menuMusicSound.Play();
		menuMusicSound.loop = true;
	}
	
	public void gameMusic()
	{
		menuMusicSound.Stop();
		gameMusicSound.Play();
		gameMusicSound.volume = 0.05f;
		gameMusicSound.loop = true;
	}


}

