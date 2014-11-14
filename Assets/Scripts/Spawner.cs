using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

  GameObject[] animals;
  public GameObject prefab;
  Quaternion q;
  float maxOffset;
  float randx;
  float randz;
  float height;
  public GameObject player;
	// Use this for initialization
	void Start () {
    q = new Quaternion();
    q.SetLookRotation ( Vector3.forward );
    maxOffset = 200.0f;
    animals = new GameObject[100];
    for( int i = 0; i < 100; i++ )
    {
      randx = Random.Range ( player.transform.position.x - maxOffset
                           , player.transform.position.x + maxOffset );
      randz = Random.Range ( player.transform.position.z - maxOffset
                            , player.transform.position.z + maxOffset );
      height = TerrainControl.playerTerrain.terrainData.GetHeight ( (int)randx, (int)randz );
      animals[i] = (GameObject)Instantiate ( prefab, new Vector3( randx, height, randz ), q );
    }
	}
	
	// Update is called once per frame
	void Update () {
    for( int i = 0; i < 100; i++ )
    {
      if( Vector3.Magnitude ( animals[i].transform.position - player.transform.position ) > 250 )
      {
        Destroy ( animals[i] );

        randx = Random.Range ( player.transform.position.x - maxOffset
                              , player.transform.position.x + maxOffset );
        randz = Random.Range ( player.transform.position.z - maxOffset
                              , player.transform.position.z + maxOffset );
        height = TerrainControl.playerTerrain.terrainData.GetHeight ( (int)randx, (int)randz );
        animals[i] = (GameObject)Instantiate ( prefab, new Vector3( randx, height, randz ), q );
      }
    }	
	}
}
