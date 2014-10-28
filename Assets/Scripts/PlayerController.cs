//
// Movement (in direction camera is facing), and controls for doing things that bears do
//

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// TODO
	// add animation plays once animations are available
	// add UI response to controls
	// add smoothing to camera wobble
	// remember to check center of mass when a real model is added

	// Public:
	public static Vector3 playerPos; // the position of the player is now retrievable
	public float forwardSpeed; // 0.15 or so with translate
	public float sideSpeed; // something like 1/3 forward speed
	public float backwardsModifier; // # times slower than forwardSpeed, 1/3 or so
	public float jumpModifier; // 2500 or something with 10x grav
	public float sprintModifier; // 1.75 or so
	public Transform cameraTransform;
	//

	// Private:
	private Transform m_playerTransform;
	private Rigidbody m_playerRigidbody;
	private PlayerAttributes m_playerAttributes;

	private float m_weightModifier;
	private bool m_isSprinting;
	private bool m_isGrounded;
	private bool m_jump;

	private Vector3 m_playerMovement;
	private float m_moveX;
	private float m_moveY;
	private float m_XModifier;
	private float m_YModifier;
	private float m_jumpTime;
	// private float m_maxSlope;
	// private RaycastHit m_objectHit;

	private int m_selectedAbility;
	private int m_selectedItem;
	private bool m_isAbility;
	private int m_numAbilities; // number of different abilities for selection
	private int m_numItems; // number of different abilities for selection
	//

	void Start()
	{
		playerPos = this.transform.position;
		// m_maxSlope = 50.0f;
		m_selectedItem = 0;
		m_selectedAbility = 0;
		m_numItems = 0;
		m_numAbilities = 0;
		m_isSprinting = false;
		m_playerAttributes = GetComponent<PlayerAttributes>();
		m_playerRigidbody = GetComponent<Rigidbody>();
		m_playerTransform = GetComponent<Transform>();
		m_weightModifier = m_playerRigidbody.mass;

		// Player doesn't fall onto side
		m_playerRigidbody.centerOfMass = new Vector3(0.0f, -0.5f,0.0f); // adjust for new model size when added
	}
	
	void FixedUpdate()
	{
		movePlayer();

		if(m_jump)
		{
			jumpPlayer();
			m_jump = false;
		}
	}

	void Update()
	{
		m_moveX = Input.GetAxis("Horizontal");
		m_moveY = Input.GetAxis("Vertical");

		if(Input.GetKeyDown(KeyCode.LeftShift) && (m_playerAttributes.curEndurance > 0)) // Sprint
		{
			m_isSprinting = true;
			StartCoroutine(cameraWobble());
			m_playerAttributes.curEndurance -= 1*Time.deltaTime;
			Mathf.Clamp(m_playerAttributes.curEndurance, 0, Mathf.Infinity);
		}
		
		if(Input.GetKeyUp(KeyCode.LeftShift) || (m_playerAttributes.curEndurance <= 0)) // Sprint
		{
			m_isSprinting = false;
			// add delayed stamina gain
		}
		
		if(Input.GetButtonDown("Jump") && m_isGrounded) // Jump
		{
			m_jump = true;
		}
		
		if(Input.GetButtonDown("Fire1")) // Attack
		{
			playerAttack();
		}
		
		if(Input.GetButtonDown("Fire2")) // Use item or ability
		{
			if(m_isAbility)
			{
				useAbility(m_selectedAbility);
			}
			else
			{
				useItem(m_selectedItem);
			}
		}
		
		if(Input.GetButtonDown("SelectAbility")) // select ability, default 'E', make customizable later
		{
			if(!m_isAbility)
			{
				m_isAbility = true;
				// Add to UI that ability is selected
			}
			else
			{
				m_selectedAbility++;
				if(m_selectedAbility > m_numAbilities)
				{
					m_selectedAbility = 1;
				}
			}
		}
		
		if(Input.GetButtonDown("SelectItem")) // select item, default 'Q', make customizable later
		{
			if(m_isAbility)
			{
				m_isAbility = false;
				// Add to UI that item is selected
			}
			else // later add check to skip if have 0, or remove from items list
			{
				m_selectedItem++;
				if(m_selectedItem > m_numItems)
				{
					m_selectedItem = 1;
				}
			}
			if(Input.GetButtonDown("Activate"))
			{
				RaycastHit objectHit;
				if(Physics.SphereCast(m_playerTransform.position,
				                      m_playerAttributes.attRange/2,
				                      m_playerTransform.forward,
				                      out objectHit,
				                      m_playerAttributes.attRange))
				{
					Debug.Log(objectHit.transform.gameObject.name);
				}
			}
		}
		playerPos = this.transform.position;
	}

	void movePlayer()
	{
		m_XModifier = 1;
		m_YModifier = 1;

		if(m_isSprinting)
		{
			m_YModifier *= sprintModifier;
		}

		m_XModifier *= sideSpeed / m_weightModifier;
		m_YModifier *= forwardSpeed / m_weightModifier;

		if(m_moveY < 0)
		{
			m_YModifier /= backwardsModifier;
		}

		m_playerMovement.Set(m_moveX * m_XModifier, 0.0f, m_moveY * m_YModifier);
		
		m_playerTransform.Translate(Vector3.ClampMagnitude(m_playerMovement, Mathf.Abs(m_YModifier)));
		// Using velocity gives all kinds of glitches
		//m_playerRigidbody.velocity = m_playerTransform.TransformDirection(Vector3.ClampMagnitude(m_playerMovement,
		//                                                                                Mathf.Abs(m_YModifier)));
	}

	void jumpPlayer()
	{
		m_playerRigidbody.AddRelativeForce(Vector3.up * jumpModifier * Physics.gravity.magnitude);
	}

	void useAbility(int ability)
	{
		return; // pending ability implementation
	}

	void useItem(int item)
	{
		return; // pending item implementation
	}

	void playerAttack()
	{
		RaycastHit m_objectHit;

		// Swipe animation or however visual is wanted
		if(Physics.SphereCast(m_playerTransform.position,
		                      m_playerAttributes.attRange/2,
		                      m_playerTransform.forward,
		                      out m_objectHit,
		                      m_playerAttributes.attRange))
		{
			m_objectHit.rigidbody.AddForce(m_objectHit.normal*m_playerAttributes.strength*100*-1);
			if(m_objectHit.transform.gameObject.CompareTag("Destructible")) // inanimate things that can be destroyed
			{
				StartCoroutine(GameController.animDestroy(m_objectHit.transform.gameObject, 1.0f)); // delayed destroy
			}
		}
	}

	void OnCollisionStay(Collision collision)
	{
		m_isGrounded = true;
	}

	void OnCollisionExit()
	{
		m_isGrounded = false;
	}

	IEnumerator cameraWobble()
	{
		float wobbleModifier = 0.3f;
		float wobbleSpeed = 0.2f;
		// float timeChange;

		cameraTransform.Rotate(Vector3.forward * -wobbleModifier/2);
		yield return new WaitForSeconds(wobbleSpeed);
		while(m_isSprinting)
		{
			cameraTransform.Rotate(Vector3.forward * wobbleModifier);
			yield return new WaitForSeconds(wobbleSpeed);

			cameraTransform.Rotate(Vector3.forward * -wobbleModifier);
			yield return new WaitForSeconds(wobbleSpeed);
		}
		cameraTransform.Rotate(Vector3.forward * wobbleModifier/2);

		// Prevents camera from staying centered
		/*
		 timeChange = 0.0f;
		 while(timeChange < wobbleSpeed)
			{
				timeChange += Time.deltaTime;
				cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation,
				                                            Quaternion.AngleAxis(-wobbleModifier, cameraTransform.forward),
				                                            Time.deltaTime);
				yield return null;
			}
		 */
	}
}
