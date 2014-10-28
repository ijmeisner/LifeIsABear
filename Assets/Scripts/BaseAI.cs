using UnityEngine;
using System.Collections;

public interface BaseAI
{
	void smell();         // Smell for predators/prey
	void hear();          // Hear sounds
	void lineOfSight();   // Detect things in sight
	Vector2[] pathSelect( Vector3 currentPos );    // Select path to move along
	void attack();        // Attack something
	void lookForOthers(); // Look for other living beings
	void flee();          // Run under various situations
	void hide();


}
