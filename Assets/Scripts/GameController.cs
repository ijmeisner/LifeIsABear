//
//
//

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// TODO
	// remove Application.Quit in Update once menu is added

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

	public static IEnumerator animDestroy(Object destroyObj, float destroyTime)
	{
		for(; destroyTime >= 0; destroyTime -= Time.deltaTime)
		{
			yield return null;
		}
		Destroy(destroyObj);
	}
}
