//
// Game Controller
// MAKE SURE THIS SCRIPT IS ORDERED LAST SO THAT SAVE DATA IS LOADED CORRECTLY
//

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	// TODO
	// Add Menu and remove Application.Quit in Update once menu is added
	// have weather transition and make sky more gray
	// add sunset/sunrise tint maybe?
	// make particle system created by script instead of using from within editor if possible, but don't know how to yet
	// add actual sun and mooon images

	// Public:
	public Transform sunLight;
	public Transform moonLight;
	public Camera playerCamera;
	public float dayNightSpeed; // number of times faster than real life

	public float weatherLength; // how long a weather condition will last at minimum
	public float noWeatherLength; // how long a weather condition must have to wait to occur at minimum
	public float weatherStartChance; // 0-1, lower is less likely, chance to happen in each 30 seconds
	public float weatherStopChance; // 0-1, lower is less likely, chance to happen each 30 seconds
	// -

	// Public Static:
	public static bool isDay;
	// -

	void Start()
	{
		// Attempt to cap framerate
		Application.targetFrameRate = 60;
		// Don't show mouse cursor
		Screen.showCursor = false;

		isDay = true;
		StartCoroutine(dayNight());
		StartCoroutine(weather());

		// If gamecontroller isn't loaded last, save data might be overwritten by instantiations
		SaveGame.load();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape)) // remove this when menu to quit with is added
		{
			Application.Quit();
		}
	}

	void OnApplicationQuit()
	{
		// SAVE on exit
		SaveGame.save();
	}

	// ---
	// --------------------
	// ---

	public static IEnumerator animDestroy(Object destroyObj, float destroyTime) // destroy object after animation
	{
		// play animation with destroyTime length
		yield return new WaitForSeconds(destroyTime);
		Destroy(destroyObj);
	}

	public IEnumerator dayNight()
	{
		float updateSpeed = 0.1f; // how many seconds before update
		float deltaAngle = (dayNightSpeed*updateSpeed)/480;
		// float sunIntensity = sunLight.light.intensity;
		// float moonIntensity = moonLight.light.intensity;
		float deltaSunLight = deltaAngle/500;
		float deltaMoonLight = deltaAngle/1500;
		Color defaultSkyColor = new Color(100.0f/255.0f, 155.0f/255.0f, 225.0f/255.0f);

		while(true)
		{
			sunLight.Rotate(Vector3.right*deltaAngle);
			moonLight.Rotate(Vector3.right*deltaAngle);
			if(sunLight.eulerAngles.x > 180)
			{
				isDay = false;
				sunLight.light.enabled = false;
				moonLight.light.enabled = true;
			}
			else
			{
				isDay = true;
				sunLight.light.enabled = true;
				moonLight.light.enabled = false;
			}
			// Light brightest at noon
			if(sunLight.eulerAngles.y == 0)
			{
				sunLight.light.intensity += deltaSunLight;
				moonLight.light.intensity -= deltaMoonLight;
			}
			else
			{
				sunLight.light.intensity -= deltaSunLight;
				moonLight.light.intensity += deltaMoonLight;
			}
			if((sunLight.eulerAngles.x <= 90) && ((90-sunLight.eulerAngles.x) != 0))
			{
				// change sky color to black for night and blue for day
				playerCamera.backgroundColor = defaultSkyColor/(90/sunLight.eulerAngles.x);
			}
			yield return new WaitForSeconds(updateSpeed);
		}
	}

	public IEnumerator weather()
	{
		// declare particle system here if possible

		yield return new WaitForSeconds(noWeatherLength);
		while(true)
		{
			float timeChange;
			float startIntensity;

			// Decide if a weather happens
			while(Random.Range(0.0f, 1.0f)>(weatherStartChance))
			{
				yield return new WaitForSeconds(30); // random should be framerate independant
			}

			playerCamera.particleSystem.Play();

			timeChange = 0.0f;
			startIntensity = sunLight.light.intensity;
			// light dims when storm comes
			while(timeChange < noWeatherLength)
			{
				timeChange += Time.deltaTime;
				sunLight.light.intensity = Mathf.Lerp(sunLight.light.intensity,
				                                      startIntensity-0.3f,
				                                      Time.deltaTime);
				// gradually increase particle count here
				yield return null;
			}

			// wait for weather to end
			yield return new WaitForSeconds(weatherLength);

			// weather can last variable amount of time
			while(Random.Range(0.0f, 1.0f)>(weatherStopChance))
			{
				yield return new WaitForSeconds(30);
			}

			playerCamera.particleSystem.Stop();

			// wait until weather is allowed
			timeChange = 0.0f;
			startIntensity = sunLight.light.intensity;
			// return to normal light intensity
			while(timeChange < noWeatherLength)
			{
				timeChange += Time.deltaTime;
				sunLight.light.intensity = Mathf.Lerp(sunLight.light.intensity,
				                                      startIntensity+0.3f,
				                                      Time.deltaTime);
				// gradually decrease particle count here
				yield return null;
			}
		}
	}
}
