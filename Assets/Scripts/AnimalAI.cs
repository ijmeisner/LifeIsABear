using UnityEngine;
using System.Collections;

public class AnimalAI : MonoBehaviour, BaseAI {
	public PathGraph pathGraph;
	float lastDecisionTime;
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
	public void pathSelect()
	{
		float currentTime = Time.time;
		if( currentTime - lastDecisionTime > 10.0f)
		{
			pathGraph.Astar ( this.transform.position, this.transform.position + new Vector3(25.0f, 0.0f, 25.0f));
			lastDecisionTime = Time.time;
		}
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
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		pathSelect ();	
	}
}
