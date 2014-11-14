using UnityEngine;
using System.Collections;

public class AiRandom : MonoBehaviour {

  IMovementController movementController;
  Vector3 position;
  Vector3 targetPosition;
  Terrain terrain;
  Vector3 offset; // offset of how far ai can travel from player
  Rigidbody body;
  MeshRenderer mesh;
  void Start()
  {
    offset = new Vector3( 100.0f, 0.0f,  100.0f );
    terrain = Terrain.activeTerrain;
    body = GetComponent<Rigidbody>();
    mesh = GetComponent<MeshRenderer>();
    movementController = new FoxController();
    movementController.LinkToAgent ( body, mesh );
  }
  void FixedUpdate()
  {
    position = this.position;
    targetPosition = movementController.GetPath ();
    if( targetPosition.sqrMagnitude == 0.0f )
    {
      float x = Random.Range ( position.x - offset.x, position.x + offset.x );
      float z = Random.Range ( position.z - offset.z, position.z + offset.z );
      float y = terrain.terrainData.GetHeight( (int)x, (int)z );
      targetPosition = new Vector3( x, y, z );
      movementController.SetAlongPath ( targetPosition, MoveState.WALKING, StanceState.UPRIGHT );
    }
    movementController.MoveToGoal ();
  }
}
