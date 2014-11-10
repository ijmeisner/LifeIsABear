using UnityEngine;
using System.Collections;

public class AnimalAI : ScriptableObject, BaseAI
{
	PathGraph pathGraph;
	float lastDecisionTime;

	public AnimalAI()
	{
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

	public Vector2[] pathSelect( Vector3 currentPos )
	{
		pathGraph = PathGraph.activeGraph;
		Vector2[] path;
		path = null;
		float currentTime = Time.time;
		if( true ) // true for debugging purposes
		{
			int[] pathIndices = null;
			pathIndices = pathGraph.Astar ( currentPos, currentPos + new Vector3(-5.0f, 0.0f, -5.0f));
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
				}
				lastDecisionTime = Time.time;
			}
			else
			{
				path = null;
				lastDecisionTime = Time.time;
				// agent is at goal!
			}
		}
		return path;
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
		lastDecisionTime = Time.time - 15.0f;
	}
	public void Update()
	{
		pathGraph = PathGraph.activeGraph;
	}
}
