using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Respawn : MonoBehaviour {

	private List<Transform> spawnPoints = new List<Transform>();
	public List<Transform> SpawnPoints {
		get {
			return spawnPoints;
		}
		internal set {
			spawnPoints = value;
		}
	}
	private Transform lastSpawnPoint = null;

	void Start () {
		List<GameObject> gos = GameObject.FindGameObjectsWithTag( "Spawn Point" ).ToList();
		foreach(GameObject go in gos ) {
			SpawnPoints.Add( go.transform );
		}
	}
	
	void Update () {
		if( Input.GetKeyDown( KeyCode.R ) ) {
			Transform trans = null;
			if( SpawnPoints.Count > 0 ) {
				bool spawnPointAccepted = false;
				while( !spawnPointAccepted ) {
					trans = SpawnPoints[Random.Range( 0, SpawnPoints.Count - 1 )];

					if (trans != lastSpawnPoint || SpawnPoints.Count == 1 ) {
						spawnPointAccepted = true;
					}
				}
			}
			this.transform.position = trans == null ? new Vector3( 0f, 2f, 0f ) : trans.position;
			this.GetComponent<Rigidbody>().velocity = Vector3.zero;
			lastSpawnPoint = trans;
		}
	}
}
