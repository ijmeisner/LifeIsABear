//
// Movement (in direction camera is facing), and controls for doing things that bears do
//

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// TODO
	// add animation plays once animations are available
	// add item and ability and item usage once available
	// decide input controls
	// make attribute dependant stuff dependant on attributes once added

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

	public float attackRange; // get this from attribute once attribute class is available
	public float hitForce; // Your strength, get this from attribute once attribute class is available
	public float stamina; // sprint duraction, get this from attribute once attribute class is available

	private Vector3 m_playerMovement;
	private float m_moveX;
	private float m_moveY;
	private float m_jumpTime;
	private int m_selectedAbility;
	private int m_selectedItem;
	private bool isSprinting;

	void Start()
	{
		m_jumpTime = 0;
		m_selectedItem = 0;
		m_selectedAbility = 0;
		isSprinting = false;
	}
	
	void FixedUpdate () // decide controls to use and edit this when agreed upon
	{
		movePlayer();

		/*
		if(Input.GetButton("Sprint"))
		{
			isSprinting = true;
			// StartCoroutine("cameraWobble");
			stamina -= Time.deltaTime; // or a factor of the time, depending how stamina is implemented
		}

		if(Input.GetButtonUp("Sprint"))
		{
			isSprinting = false;
			// StopCoroutine("cameraWobble");
		}
		*/

		if(Input.GetButtonDown("Jump"))
		{
			jumpPlayer();
		}

		if(Input.GetButtonDown("Fire1"))
		{
			playerAttack();
		}

		if(Input.GetButtonDown("Fire2"))
		{
			useAbility(m_selectedAbility);
		}

		if(Input.GetButtonDown("Fire3"))
		{
			useItem(m_selectedItem);
		}

		/*
		if(Input.GetButtonDown())
		{
			// ability selector todo - just cycle possible int values
		}

		if(Input.GetButtonDown())
		{
			// item selector todo - just cycle possible int values
		}
		*/

		increaseTimes(); // for stuff like jumpTime;
	}

	void movePlayer()
	{
		m_moveX = Input.GetAxis("Horizontal");
		m_moveY = Input.GetAxis("Vertical");

		m_moveX *= sideSpeed;
		m_moveY *= forwardSpeed;

		if(m_moveY < 0)
		{
			m_moveY /= backwardsModifier;
		}
		if(isSprinting)
		{
			m_moveX *= sprintModifier;
		}
		
		m_playerMovement.Set(m_moveX, 0.0f, m_moveY);
		
		transform.Translate(Vector3.ClampMagnitude(m_playerMovement, forwardSpeed));
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
		                              attackRange,
		                              transform.TransformDirection(Vector3.forward),
		                              out objectHit,
		                              attackRange))
		{
			objectHit.rigidbody.AddForce(objectHit.normal*hitForce*-1);
			if(objectHit.transform.gameObject.tag == "Destructible")
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
}
