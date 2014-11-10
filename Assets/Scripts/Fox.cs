using UnityEngine;
using System.Collections;

public class Fox : MonoBehaviour {

	AnimalAI     aiPackage;
	Vector3      directionFacing;
	Vector3      difference;
	Vector2[]    path;
	int          currentPathIndex;
	int          pathMaxIndex;
	float        moveSpeed; // Some default value based on animal and state of being (fleeing, hiding, etc)
	MeshRenderer mesh;
	Rigidbody    body;
	float        epsilon; // How close to center of cell before it is counted as being there

	// Use this for initialization
	void Awake()
	{
		// make mesh, collider, animation data, etc
		aiPackage = ScriptableObject.CreateInstance<AnimalAI> ();
		moveSpeed = 55.0f;
		body = GetComponent<Rigidbody> ();
		currentPathIndex = 0;
		epsilon = 0.2f;
		path = null;
	}
	
	// Update is called once per frame
	void FixedUpdate () // TODO: selection of new paths that are in the path graph
					// TODO: include logic into the new paths and when they are chosen and the speed at which traversed.
	{
		if( path != null )
		{
			if( Vector3.Magnitude (difference) > epsilon )
			{
				body.AddForce ( directionFacing*moveSpeed*Time.deltaTime );
				difference.x = path[currentPathIndex].x - this.transform.position.x;
				difference.z = path[currentPathIndex].y - this.transform.position.z;
			}
			else
			{
				currentPathIndex++;
				if( currentPathIndex < pathMaxIndex )
				{
					directionFacing.x = path[currentPathIndex].x - this.transform.position.x;
					directionFacing.z = path[currentPathIndex].y - this.transform.position.z;
					directionFacing.y = 0.5f;
					difference = directionFacing;
					Vector3.Normalize (directionFacing);
					body.AddForce ( directionFacing*moveSpeed*Time.deltaTime );
				}
				else
				{
					path = null;
					currentPathIndex = 0;

				}
			}
		}
		else
		{
			path = aiPackage.pathSelect ( gameObject.transform.position );
			if(path != null)
			{
				pathMaxIndex = path.Length;
				directionFacing.x = path[currentPathIndex].x - this.transform.position.x;
				directionFacing.z = path[currentPathIndex].y - this.transform.position.z;
				directionFacing.y = 0.5f;
				difference = directionFacing;
				Vector3.Normalize (directionFacing);
			}
		}
	}
}
