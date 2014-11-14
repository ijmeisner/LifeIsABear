using UnityEngine;
using System.Collections;
// Make a formula for the velocity in relation to how close to next goal the character is.
public class Fox : MonoBehaviour, IAiAgent {

  AIType              aiType;
  IAiPackage          aiPackage;
  Rigidbody           body;
  IMovementController movementController;
  Vector3             position;
  MeshRenderer        mesh;

  Fox()
  {
    aiType = AIType.FOX;
  }

  public Vector3 GetPosition()
  {
    return position;
  }
  public AIType GetAIType()
  {
    return aiType;
  }
  // Use this for initialization
  void Awake()
  {
    // make mesh, collider, animation data, etc
    MovementControllerFactory movementControllerFactory = MovementControllerFactory.GetInstance ();
    aiPackage = gameObject.AddComponent<FoxAi>(); // REPLACE WITH FACTORY!!!!
    movementController = movementControllerFactory.CreateController( AIType.FOX );
    mesh = GetComponent<MeshRenderer>(); // eventually change to add component and get rid of the mesh on the prefab
    body = GetComponent<Rigidbody>();
    movementController.LinkToAgent ( body, mesh );
    aiPackage.Initialize ( body, movementController );
  }

  void Update()
  {
    position = gameObject.transform.position;
    aiPackage.Think ();
  }
  void FixedUpdate()
  {
    movementController.MoveToGoal ();
  }
  // Update is called once per frame
  public void Live () // TODO: selection of new paths that are in the path graph
          // TODO: include logic into the new paths and when they are chosen and the speed at which traversed.
  {
    position = gameObject.transform.position;
    aiPackage.Think ();
    movementController.MoveToGoal ();
  }
}
