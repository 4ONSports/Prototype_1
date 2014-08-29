using UnityEngine;
using System.Collections;

public class MatchScenario_Test : MonoBehaviour {

	public int numberOfTriesLeft = 10;
	public Transform[] spawnPoints;
	public GameObject player;
	public GameObject ball;
	public int framesToDeterminResult = 100;
	public WallSlider_Horizontal wallSlider;
	
	private int currSpawnIndex = 0;
	public int framesAfterShooting = -1;
	//private bool playerShot = false;
	private bool playerScored = false;

	// Use this for initialization
	void Awake () {
		if( spawnPoints.Length > 0 ) {
			SpawnPlayerAndBall( currSpawnIndex );
		}
	}

	void Start() {
	}

	void Update () {
		if( framesAfterShooting > 0 ) {
			--framesAfterShooting;
		}

		if( framesAfterShooting == 0 ) {
			DetermineResult();
		}
	}

	public void OnPlayerShoot () {
		if( numberOfTriesLeft > 0 ) {
			--numberOfTriesLeft;
			framesAfterShooting = framesToDeterminResult;
		}
	}

	public void OnGoalScored () {
		playerScored = true;
	}
	
	void DetermineResult () {
		if( playerScored ) {
			currSpawnIndex = ++currSpawnIndex % spawnPoints.Length;
			SpawnPlayerAndBall (currSpawnIndex);
			playerScored = false;
		}
		else {
			SpawnPlayerAndBall (currSpawnIndex);
		}
		framesAfterShooting = -1;
		wallSlider.Reset ();
	}

	void SpawnPlayerAndBall ( int spawnIndex ) {
		if( spawnIndex >= 0 && spawnIndex < spawnPoints.Length ) {
			player.transform.position = spawnPoints [currSpawnIndex].position;
			player.transform.rotation = Quaternion.identity;
			ball.transform.position = spawnPoints [currSpawnIndex].position;
			ball.transform.rotation = Quaternion.identity;
		}
	}
}
