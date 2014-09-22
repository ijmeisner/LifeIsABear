//
// Camera Follows player, and Rotates by moving mouse
//

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	//

	public Transform player;
	public float cameraXSpeed;
	public float cameraYSpeed;
	public Vector3 offset;

	private float xInput;
	private float yInput;
	private float minY = 0;
	private float maxY = 45;
	private float currentPitch = 0;
	
	void Start () {
		transform.position = player.position + offset; 
		transform.LookAt(player.position);
	}

	void LateUpdate () {
		yInput = Input.GetAxisRaw("Mouse Y") * cameraYSpeed * -1;
		xInput = Input.GetAxisRaw("Mouse X") * cameraXSpeed;

		player.transform.Rotate(Vector3.up * xInput);

		currentPitch = Mathf.Clamp(currentPitch + yInput, minY, maxY);
		transform.localEulerAngles = Vector3.right * currentPitch;
	}
}
