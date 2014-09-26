//
// Game Controller
//

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	//

	// Public:
	public Transform sunLight;
	public Transform moonLight;
	public float dayNightSpeed; // number of times faster than real life
	//
	
	// Public Static:
	public static bool isDay;
	//

	// TODO
	// remove Application.Quit in Update once menu is added

	void Start()
	{
		// Attempt to cap framerate
		Application.targetFrameRate = 60;
		// Don't show mouse cursor
		Screen.showCursor = false;

		isDay = true;
		StartCoroutine(dayNight());
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) // remove this when menu to quit with is added
		{
			Application.Quit();
		}
	}

	public static IEnumerator animDestroy(Object destroyObj, float destroyTime) // destroy object after animation
	{
		yield return new WaitForSeconds(destroyTime);
		Destroy(destroyObj);
	}

	public IEnumerator dayNight()
	{
		float updateSpeed = 0.1f; // how many seconds before update
		float deltaAngle = (dayNightSpeed*updateSpeed)/480;

		while(true)
		{
			sunLight.Rotate(Vector3.right*deltaAngle);
			moonLight.Rotate(Vector3.right*deltaAngle);
			if(sunLight.eulerAngles.x > 180)
			{
				isDay = false;
			}
			else
			{
				isDay = true;
			}
			yield return new WaitForSeconds(updateSpeed);
		}
	}
}
