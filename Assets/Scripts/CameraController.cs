//
// Camera Follows player, and Rotates by moving mouse
//

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

<<<<<<< HEAD
	//
=======
	// TODO
	// fix FOV is not clamping for some reason
	// add camera wobble (to be used in sprinting) if we want to add it
>>>>>>> origin/master

	// Public:
	public Transform player;
	public float cameraXSpeed;
	public float cameraYSpeed;
	public Vector3 offset;
	public float zoomModifier;
	//

	// Private:
	private Transform cameraTransform;

	private float xInput;
	private float yInput;
	private float wInput;
<<<<<<< HEAD

	private float minY;
	private float maxY;
	private float minFOV;
	private float maxFOV;
	private float currentPitch;
	private Vector3 newAngles;
	//
=======
	private float minY = 0.0f;
	private float maxY = 45.0f;
	private float minFOV = 30.0f;
	private float maxFOV = 90.0f;
	private float currentPitch = 0.0f;
	// private int wobbleDir = -1;
	// private float wobbleModifier = 0.1f;
>>>>>>> origin/master
	
	void Start()
	{
<<<<<<< HEAD
		minY = 0.0f;
		maxY = 45.0f;
		minFOV = 30.0f;
		maxFOV = 90.0f;
		currentPitch = 0.0f;

		cameraTransform = GetComponent<Transform>();

		cameraTransform.position = player.position + offset; 
		cameraTransform.LookAt(player.position);
=======
		transform.position = player.position + offset; 
		transform.LookAt(player.position);
>>>>>>> origin/master
	}

	void LateUpdate ()
	{
		wInput = Input.GetAxis("Mouse ScrollWheel");
		if (wInput < 0)
		{
			camera.fieldOfView -= wInput * zoomModifier;
			camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFOV, maxFOV);
		}
		else if(wInput > 0)
		{
			camera.fieldOfView -= wInput * zoomModifier;
			camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, minFOV, maxFOV);
		}

		yInput = Input.GetAxisRaw("Mouse Y") * cameraYSpeed * -1;
		xInput = Input.GetAxisRaw("Mouse X") * cameraXSpeed;

		player.transform.Rotate(Vector3.up * xInput);

		currentPitch = Mathf.Clamp(currentPitch + yInput, minY, maxY);
		transform.localEulerAngles = Vector3.right * currentPitch;
	}

	public static IEnumerator cameraWobble() // TODO
	{
		yield return null;
	}
}
