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

	// Public:
	public float forwardSpeed;
	public float sideSpeed;
	public float backwardsModifier; // # times slower than forwardSpeed
	public float jumpModifier;
	public float sprintModifier;
	public float jumpDelay; // can change to when contacting ground, but I think there should be longer delay for a bear
	public Transform cameraTransform;
	//

	// Private:
	private Transform m_playerTransform;
	private Rigidbody m_playerRigidbody;
	private PlayerAttributes m_playerAttributes;

	private float m_weightModifier;
	private bool m_isSprinting;

	private Vector3 m_playerMovement;
	private float m_moveX;
	private float m_moveY;
	private float m_XModifier;
	private float m_YModifier;
	private float m_jumpTime;

	private int m_selectedAbility;
	private int m_selectedItem;
	private bool m_isAbility;
	private int m_numAbilities; // number of different abilities for selection
	private int m_numItems; // number of different abilities for selection
	//

	void Start()
	{
		m_jumpTime = 0;
		m_selectedItem = 0;
		m_selectedAbility = 0;
		m_numItems = 0;
		m_numAbilities = 0;
		m_isSprinting = false;
		m_playerAttributes = GetComponent<PlayerAttributes>();
		m_playerRigidbody = GetComponent<Rigidbody>();
		m_playerTransform = GetComponent<Transform>();
		m_weightModifier = m_playerRigidbody.mass;
	}
	
	void FixedUpdate ()
	{
		movePlayer();
	}

	void Update()
	{
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
		
		if(Input.GetButtonDown("Jump")) // Jump
		{
			jumpPlayer();
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
		}

		m_jumpTime -= Time.deltaTime;
	}

	void movePlayer()
	{
		m_XModifier = 1;
		m_YModifier = 1;
		m_moveX = Input.GetAxis("Horizontal");
		m_moveY = Input.GetAxis("Vertical");

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

		m_moveX *= m_XModifier;
		m_moveY *= m_YModifier;

		m_playerMovement.Set(m_moveX, 0.0f, m_moveY);
		
		m_playerTransform.Translate(Vector3.ClampMagnitude(m_playerMovement, Mathf.Abs(m_YModifier)));
	}

	void jumpPlayer()
	{
		if(m_jumpTime <= 0)
		{
			m_playerRigidbody.AddForce(Vector3.up * jumpModifier);
			m_jumpTime = jumpDelay;
		}
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
		RaycastHit objectHit;
		// Swipe animation or however visual is wanted
		if(Physics.SphereCast(transform.position,
		                              m_playerAttributes.attRange/2,
		                              transform.TransformDirection(Vector3.forward),
		                              out objectHit,
		                              m_playerAttributes.attRange))
		{
			objectHit.rigidbody.AddForce(objectHit.normal*m_playerAttributes.strength*100*-1);
			if(objectHit.transform.gameObject.tag == "Destructible") // inanimate things that can be destroyed
			{
				// play destroyed animation
				StartCoroutine(GameController.animDestroy(objectHit.transform.gameObject, 1.0f)); // delayed destroy
			}
		}
	}

	IEnumerator cameraWobble()
	{
		float wobbleModifier = 0.3f;
		
		cameraTransform.Rotate(Vector3.forward * -wobbleModifier/2);
		yield return new WaitForSeconds(0.1f);
		while(m_isSprinting)
		{
			cameraTransform.Rotate(Vector3.forward * wobbleModifier);
			yield return new WaitForSeconds(0.1f);
			cameraTransform.Rotate(Vector3.forward * -wobbleModifier);
			yield return new WaitForSeconds(0.1f);
		}
		cameraTransform.Rotate(Vector3.forward * wobbleModifier/2);
	}
}
