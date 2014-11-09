//
// Camera Follows player (attached), and Rotates by moving mouse
//

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// TODO
	// Prevent from going through things it shouldn't
	// find a way to "lerp" camera rotation without breaking it

	// Public:
	public Transform player;
	public float cameraXSpeed;
	public float cameraYSpeed;
	public Vector3 offset;
	public float zoomModifier;
	// -

	// Private:
	private Transform m_cameraTransform;
	private Transform m_playerTransform;

	private float m_xInput;
	private float m_yInput;
	private float m_wInput;

	private float m_minY;
	private float m_maxY;
	private float m_minFOV;
	private float m_maxFOV;
	private float m_currentPitch;
	private Vector3 m_newAngles;
	private Vector3 m_oldAngles;
	// -
	
	void Start() // make sure this happens after load
	{
		m_minY = 0.0f;
		m_maxY = 45.0f;
		m_minFOV = 30.0f;
		m_maxFOV = 90.0f;
		m_currentPitch = 12.0f;

		m_cameraTransform = GetComponent<Transform>();
		m_playerTransform = player.GetComponent<Transform>();

		m_cameraTransform.position = m_playerTransform.position + offset; 
		m_cameraTransform.LookAt(m_playerTransform.position);
	}

	void LateUpdate ()
	{
		// Mouse Wheel Zoom
		m_wInput = Input.GetAxis("Mouse ScrollWheel");
		if (m_wInput < 0)
		{
			camera.fieldOfView -= m_wInput * zoomModifier;
			camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, m_minFOV, m_maxFOV);
		}
		else if(m_wInput > 0)
		{
			camera.fieldOfView -= m_wInput * zoomModifier;
			camera.fieldOfView = Mathf.Clamp(camera.fieldOfView, m_minFOV, m_maxFOV);
		}

		// Mouse movement input
		m_yInput = Input.GetAxis("Mouse Y") * cameraYSpeed * -1;
		m_xInput = Input.GetAxis("Mouse X") * cameraXSpeed;

		// Camera Follows player and rotates around him
		m_cameraTransform.position = m_playerTransform.position + offset;
		m_cameraTransform.RotateAround(m_playerTransform.position, Vector3.up, m_xInput);
		offset = m_cameraTransform.position - m_playerTransform.position;

		// Camera up and down rotation
		m_currentPitch = Mathf.Clamp(m_currentPitch + m_yInput, m_minY, m_maxY);
		m_newAngles.Set(m_currentPitch,
		                m_cameraTransform.localEulerAngles.y,
		                m_cameraTransform.localEulerAngles.z);
		m_cameraTransform.localEulerAngles = m_newAngles;

		// Player rotates sideways with camera
		m_newAngles.Set(m_playerTransform.localEulerAngles.x,
		                m_cameraTransform.localEulerAngles.y,
		                m_playerTransform.localEulerAngles.z);
		m_playerTransform.localEulerAngles = m_newAngles;
	}
}
