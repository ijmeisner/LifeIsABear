//
//
//

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// TODO
	// remove Application.Quit in Update once menu is added
	// alter destroyTime to be grabbed from object?
	// fix the animDestroy delay because it isn't working

	public static float destroyTime = 2.0f;
	
	void Start()
	{
		// Attempt to cap framerate
		Application.targetFrameRate = 60;
		// Don't show mouse cursor
		Screen.showCursor = false;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) // remove this when menu to quit with is added
		{
			Application.Quit();
		}
	}

	public static IEnumerator animDestroy(Object destroyObj)
	{
		for(float i=0.0f; i<=destroyTime; i+=Time.deltaTime);
		Destroy(destroyObj);
		yield return null;
	}
}
