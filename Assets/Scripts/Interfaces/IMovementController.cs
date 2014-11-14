using UnityEngine;
using System.Collections;

public interface IMovementController {

  void LinkToAgent ( Rigidbody srcBody, MeshRenderer srcMesh ); // Possibly add animator in too
  void DecideVeloc();
  void Slow();
  void Move( Vector3 force );
  void MoveToGoal();
  void SetAlongPath( Vector3 newPath, MoveState newMoveState, StanceState newStanceState );
  Vector3 GetPath();
  void TurnToDirection();
}

public enum AIType
{
  FOX,
  DEER,
  SQUIRREL,
  RABBIT,
  HIKER,
  BEAR
}