//
// Camera Follows player, and Rotates by moving mouse
//

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// TODO
	// figure out why localeulerangles isn't setting

	public Transform player;
	public float cameraXSpeed;
	public float cameraYSpeed;
	public Vector3 offset;
	public float zoomModifier;

	private float xInput;
	private float yInput;
	private float wInput;
	private float minY = 0.0f;
	private float maxY = 45.0f;
	private float minFOV = 30.0f;
	private float maxFOV = 90.0f;
	private float currentPitch = 0.0f;
	private Transform cameraTransform;
	private Vector3 newAngles;
	
	void Start ()
	{
		cameraTransform = GetComponent<Transform>();

		cameraTransform.position = player.position + offset; 
		cameraTransform.LookAt(player.position);
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
		newAngles.Set(currentPitch, cameraTransform.localEulerAngles.y, cameraTransform.localEulerAngles.z);
		cameraTransform.localEulerAngles = newAngles;
	}
}
