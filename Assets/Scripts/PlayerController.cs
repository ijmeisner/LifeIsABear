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
	// add item and ability and item usage once available
	// decide input controls
	// make attribute dependant stuff dependant on attributes once added
	// learn spherecast behavior and adjust it

	// Ideas for control scheme
	// left click attacks, right click uses ability or item, whichever is selected
	// Make 'Q' select item (and change which if item is selected) and 'E' select/change ability
	// Use Edit->ProjectSettings->Input to change, and maybe we will make configurable at some point

	// Item/Ability value means none; for when you don't have one yet otherwise cycle to nonzero values

	public float forwardSpeed;
	public float sideSpeed;
	public float backwardsModifier; // # times slower than forwardSpeed
	public float jumpModifier;
	public float sprintModifier;
	public float jumpDelay; // can change to when contacting ground, but I think there should be longer delay for a bear
	public Transform cameraTransform;

	public float attackRange; // get this from attribute once attribute class is available
	public float hitForce; // Your strength (*100?), get this from attribute once attribute class is available
	public float stamina; // sprint duraction, get this from attribute once attribute class is available

	private Vector3 m_playerMovement;
	private float m_moveX;
	private float m_moveY;
	private float m_XModifier;
	private float m_YModifier;
	private float m_jumpTime;
	private int m_selectedAbility;
	private int m_selectedItem;
	private bool m_isAbility;
	private int m_numAbilities = 0; // number of different abilities for selection
	private int m_numItems = 0; // number of different abilities for selection
	private float m_weightModifier;
	private bool m_isSprinting;

	void Start()
	{
		m_jumpTime = 0;
		m_selectedItem = 0;
		m_selectedAbility = 0;
		m_isSprinting = false;
		m_weightModifier = GetComponent<Rigidbody>().mass;
	}
	
	void FixedUpdate () // decide controls to use and edit this when agreed upon
	{
		movePlayer();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftShift) && (stamina > 0)) // Sprint, make customizable later
		{
			m_isSprinting = true;
			StartCoroutine(cameraWobble());
			stamina -= 10*Time.deltaTime;
			Mathf.Clamp(stamina, 0, Mathf.Infinity);
		}
		
		if(Input.GetKeyUp(KeyCode.LeftShift) || (stamina <= 0)) // Sprint, make customizable later
		{
			m_isSprinting = false;
			// coroutine for delayed stamina gain
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
			else
			{
				m_selectedItem++;
				if(m_selectedItem > m_numItems)
				{
					m_selectedItem = 1;
				}
			}
		}

		increaseTimes(); // for stuff like jumpTime;
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
		
		transform.Translate(Vector3.ClampMagnitude(m_playerMovement, Mathf.Abs(m_YModifier)));
	}

	void jumpPlayer()
	{
		if(m_jumpTime <= 0)
		{
			rigidbody.AddForce(Vector3.up * jumpModifier);
			m_jumpTime = jumpDelay;
		}
	}

	void useAbility(int ability)
	{
		switch(ability)
		{
			case 1:
				// ability function go here from ability class
				break;
			case 2:
				// ability function go here from ability class
				break;
			default:
				break;
		}
	}

	void useItem(int item)
	{
		switch(item)
		{
			case 1:
				// item function go here from item class
				break;
			case 2:
				// item function go here from item class
				break;
			default:
				break;
		}
	}

	void playerAttack()
	{
		RaycastHit objectHit;
		// Swipe animation or however visual is wanted
		if(Physics.SphereCast(transform.position,
		                              attackRange/2,
		                              transform.TransformDirection(Vector3.forward),
		                              out objectHit,
		                              attackRange))
		{
			objectHit.rigidbody.AddForce(objectHit.normal*hitForce*-1);
			if(objectHit.transform.gameObject.tag == "Destructible" || // in-animate things that can be destroyed
			   objectHit.transform.gameObject.tag == "Target") // or whatever the tag for people is
			{
				// play destroyed animation
				StartCoroutine(GameController.animDestroy(objectHit.transform.gameObject, 1.0f)); // delayed destroy
			}
		}
	}

	void increaseTimes()
	{
		m_jumpTime -= Time.deltaTime;
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
