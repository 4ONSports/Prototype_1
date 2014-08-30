using UnityEngine;
using System.Collections;

public class MatchScenario_TargetPractice : MatchScenario {
	
	public int numberOfTriesLeft = 10;
	public Transform[] spawnPoints;
	public GameObject player;
	public GameObject ball;
	public float timeToDetermineResult = 6.0f;
	public PostTarget[] targets;

	private int currSpawnIndex = 0;
	private float timeOfShot = 0.0f;
	private bool playerScored = false;
	private bool targetHit = false;
	
	// Use this for initialization
	void Awake () {
		if( spawnPoints.Length > 0 ) {
			SpawnPlayerAndBall( currSpawnIndex );
		}
	}
	
	void Start() {
	}
	
	void Update () {
		CheckIfResultCanBeDetermined (Time.time);
	}

	protected override void _OnPlayerShoot () {
		if( numberOfTriesLeft > 0 ) {
			--numberOfTriesLeft;
			timeOfShot = Time.time;
		}
	}
	
	protected override void _OnGoalScored () {
		playerScored = true;
	}
	
	protected override void _OnTargetHit () {
		targetHit = true;
	}

	void CheckIfResultCanBeDetermined( float fTime ) {
		if( timeOfShot>0 && fTime-timeOfShot >=timeToDetermineResult ) {
			DetermineResult();
		}
	}
	
	void DetermineResult () {
		if( targetHit ) {
			currSpawnIndex = ++currSpawnIndex % spawnPoints.Length;
			SpawnPlayerAndBall (currSpawnIndex);
			targetHit = false;
		}
		else {
			SpawnPlayerAndBall (currSpawnIndex);
		}
	}
	
	void SpawnPlayerAndBall ( int spawnIndex ) {
		if( spawnIndex >= 0 && spawnIndex < spawnPoints.Length ) {
			player.transform.position = spawnPoints [currSpawnIndex].position;
			player.transform.rotation = Quaternion.identity;
			ball.transform.position = spawnPoints [currSpawnIndex].position;
			ball.transform.rotation = Quaternion.identity;

			// Enable post targets for spawn points
			for( int i=0; i<targets.Length; ++i ) {
				if( targets[i].spawnpoint != null && targets[i].spawnpoint.name == spawnPoints [currSpawnIndex].name ) {
					targets[i].gameObject.SetActive(true);
				}
				else {
//					targets[i].gameObject.SetActive(false);
					targets[i].Reset();
				}
			}
		}
	}
}
