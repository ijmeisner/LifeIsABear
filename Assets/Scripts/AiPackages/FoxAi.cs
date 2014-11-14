using UnityEngine;
using System.Collections;

public class FoxAi : MonoBehaviour ,IAiPackage
{
  PathGraph pathGraph;
  Rigidbody body;
  Vector3 position;
  float lastDecisionTime;
  IMovementController movementController;

  public FoxAi()
  {
  }
  public void Initialize( Rigidbody srcBody, IMovementController srcMovementController )
  {
    body = srcBody;
    movementController = srcMovementController;
  }
  public void smell() // TODO
  {
    return;
  }
  public void hear() // TODO
  {
    return;
  }
  public void lineOfSight() // TODO
  {
    return;
  }

  public void pathSelect( Vector3 currentPos )
  {
    /*
    pathGraph = PathGraph.activeGraph;
    Vector2[] path;
    path = null;
    float currentTime = Time.time;
    if( true ) // true for debugging purposes
    {
      int[] pathIndices = pathGraph.Astar ( currentPos, currentPos + new Vector3(-5.0f, 0.0f, -5.0f) );

      Debug.Log ( "PathSelect()" );
      int pathSize = 0;
      if( pathIndices != null )
      {
        pathSize = pathIndices.Length;
        path = new Vector2[ pathSize];
      }
      else
      {
        pathSize = 0;
      }
      int i;
      if( pathIndices != null && pathIndices[0] != -3)
      {
        for( i = 0; i < pathSize; i++ )
        {
          path[i] = new Vector2( pathGraph.CurrentCell( pathIndices[i]).GetCenter().x, pathGraph.CurrentCell ( pathIndices[i]).GetCenter ().z );
          path[i] += PathGraph.activeLocalOrigin; // must take into account offset of pathgraph
        }
        lastDecisionTime = Time.time;
      }
      else
      {
        path = null;
        lastDecisionTime = Time.time;
        // agent is at goal!
      }
      movementController.SetAlongPath ( path, MoveState.WALKING, StanceState.UPRIGHT );
    }
    */
  }
  public void attack() // TODO
  {
    return;
  }
  public void lookForOthers() // TODO
  {
    return;
  }
  public void flee() // TODO
  {
    return;
  }
  public void hide() // TODO
  {
    return;
  }
  public void Awake()
  {
    pathGraph = PathGraph.activeGraph;
  }
  public void Think() // basic interface function for external classes
  {
    /*
    pathGraph = PathGraph.activeGraph;
    Vector2[] path = movementController.GetPath ();
    // line of sight stuff not yet included
    if( path == null )
    {
      pathSelect ( body.transform.position ) ;
    }
    */
  }
}



