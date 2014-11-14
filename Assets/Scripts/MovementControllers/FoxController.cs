using UnityEngine;
using System.Collections;

public class FoxController : IMovementController
{
  Rigidbody body; // handle to the rigidbody to apply forces to it for movement
  MeshRenderer mesh; // handle to the mesh so that the animal can be turned
  MoveState moveState; // movement state of this animal
  StanceState stanceState; // stance state of this animal
  float maxForce; // maximum force this animal can have
  float curMaxVeloc; // maximum velocity for this animal in its current state
  float maxVeloc; // maximum velocity for this animal
  Vector3 directionFacing;
  Vector3 distToNextWaypoint; // distance to next point along current path
  Vector3 path; // Current path being traversed
  float epsilon;

  public FoxController()
  {
    maxForce = 2000;
    maxVeloc = 20;  // ** NOT SURE HOW BIG TO MAKE YET
    moveState = MoveState.WALKING;
    stanceState = StanceState.UPRIGHT;
    directionFacing = Vector3.forward;
    distToNextWaypoint = new Vector3();
    path = new Vector3( 0.0f, 0.0f, 0.0f );
    epsilon = 0.3f;
    DecideVeloc ();
  }

  public void LinkToAgent( Rigidbody srcBody, MeshRenderer srcMesh ) // add animator as paramter later maybe
  {
    body = srcBody;
    mesh = srcMesh;
  }
  
  public void DecideVeloc()
  {
    float stanceVelocMultiplier = 1.0f;
    float moveVelocMultiplier = 1.0f;
    if( stanceState == StanceState.CROUCHED )
    {
      stanceVelocMultiplier = 0.35f;
    }
    else
    {
      stanceVelocMultiplier = 1.0f;
    }
    if( moveState == MoveState.SLOWING )
    {
      moveVelocMultiplier = 0.0f;
    }
    else if( moveState == MoveState.STILL || moveState == MoveState.SWIM_STILL )
    {
      moveVelocMultiplier = 0.0f;
    }
    else if( moveState == MoveState.WALKING )
    {
      moveVelocMultiplier = 0.1f;
    }
    else if( moveState == MoveState.JOGGING )
    {
      moveVelocMultiplier = 0.5f;
    }
    else if( moveState == MoveState.RUNNING )
    {
    }
    else // moveState == MoveState.SWIM_MOVING
    {
      moveVelocMultiplier = 0.1f*maxVeloc;
    }
    curMaxVeloc = moveVelocMultiplier*stanceVelocMultiplier*maxVeloc;
  }
  
  public void Slow()
  {
    float veloc = Vector3.Magnitude ( body.velocity );
    if( Vector3.Magnitude ( body.velocity ) > 0 )
    {
      body.AddForce ( -1*( veloc*veloc*body.mass )/( 2*Vector3.Magnitude ( distToNextWaypoint ) ) * directionFacing );
    }
  }
  
  public void Move( Vector3 force )
  {
    if( Vector3.Magnitude ( body.velocity ) < curMaxVeloc )
    {
      body.AddForce ( force*Time.deltaTime );
    }
    else{} // else do nothing
  }
  public void MoveToGoal()
  {
    if( path != new Vector3( 0.0f, 0.0f, 0.0f ) )
    {
      if( Vector3.Magnitude ( body.velocity ) < curMaxVeloc )
      {
        if( Vector3.Magnitude ( distToNextWaypoint ) > epsilon )
        {
          DecideVeloc ();
          TurnToDirection ();
          Move ( Vector3.ClampMagnitude ( directionFacing*10000, maxForce ) );
        }
        else
        {
          path = new Vector3( 0.0f, 0.0f, 0.0f );
        }
      }
    }
  }
  public void SetAlongPath( Vector3 newPath, MoveState newMoveState, StanceState newStanceState )
  {
    moveState = newMoveState;
    stanceState = newStanceState;
    path.x = newPath.x;
    path.y = newPath.y;
    path.z = newPath.z;
    TurnToDirection ();
  }
  public void TurnToDirection() // using current position, makes face whereToLook
    // Currently takes a Vector2 as parameter to accomodate current system: REPLACE EVENTUALLY
  {
    directionFacing.x = path.x - body.transform.position.x;
    directionFacing.z = path.z - body.transform.position.z;
    directionFacing.y = path.y - body.transform.position.y + 0.5f;
    distToNextWaypoint = directionFacing;
    Vector3.Normalize ( directionFacing );
    Quaternion q = new Quaternion(); // Changes look direction of animal
    q.SetLookRotation ( directionFacing );
    Transform trans = mesh.transform.root;
    trans.localRotation = q;
  }
  public Vector3 GetPath()
  {
    return path;
  }
}

public enum MoveState
{
  SLOWING,
  STILL,
  WALKING,
  JOGGING,
  RUNNING,
  SWIM_STILL,
  SWIM_MOVING
}
public enum StanceState
{
  CROUCHED,
  UPRIGHT
}
