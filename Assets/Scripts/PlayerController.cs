//
// Movement (in direction camera is facing), and controls for doing things that bears do
//

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	// TODO
	// add animation plays once animations are available
<<<<<<< HEAD
	// add UI response to controls
=======
	// add item and ability and item usage once available
	// decide input controls
	// make attribute dependant stuff dependant on attributes once added

	// Ideas for control scheme
	// left click attacks, right click uses ability or item, whichever is selected
	// Make 'Q' select item (and change which if item is selected) and 'E' select/change ability
	// Use Edit->ProjectSettings->Input to change, and maybe we will make configurable at some point

	// Item/Ability value means none; for when you don't have one yet otherwise cycle to nonzero values
>>>>>>> origin/master

	// Public:
	public float forwardSpeed;
	public float sideSpeed;
	public float backwardsModifier; // # times slower than forwardSpeed
	public float jumpModifier;
	public float sprintModifier;
	public float jumpDelay; // can change to when contacting ground, but I think there should be longer delay for a bear
<<<<<<< HEAD
	public Transform cameraTransform;
	//

	// Private:
	private Transform m_playerTransform;
	private Rigidbody m_playerRigidbody;
	private PlayerAttributes m_playerAttributes;

	private float m_weightModifier;
	private bool m_isSprinting;
=======

	public float attackRange; // get this from attribute once attribute class is available
	public float hitForce; // Your strength, get this from attribute once attribute class is available
	public float stamina; // sprint duraction, get this from attribute once attribute class is available
>>>>>>> origin/master

	private Vector3 m_playerMovement;
	private float m_moveX;
	private float m_moveY;
	private float m_jumpTime;

	private int m_selectedAbility;
	private int m_selectedItem;
<<<<<<< HEAD
	private bool m_isAbility;
	private int m_numAbilities; // number of different abilities for selection
	private int m_numItems; // number of different abilities for selection
	//
=======
	private bool isSprinting;
>>>>>>> origin/master

	void Start()
	{
		m_jumpTime = 0;
		m_selectedItem = 0;
		m_selectedAbility = 0;
<<<<<<< HEAD
		m_numItems = 0;
		m_numAbilities = 0;
		m_isSprinting = false;
		m_playerAttributes = GetComponent<PlayerAttributes>();
		m_playerRigidbody = GetComponent<Rigidbody>();
		m_playerTransform = GetComponent<Transform>();
		m_weightModifier = m_playerRigidbody.mass;
=======
		isSprinting = false;
>>>>>>> origin/master
	}
	
	void FixedUpdate ()
	{
		movePlayer();

<<<<<<< HEAD
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
=======
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
>>>>>>> origin/master
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
<<<<<<< HEAD
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
=======
			// ability selector todo - just cycle possible int values
		}

		if(Input.GetButtonDown())
		{
			// item selector todo - just cycle possible int values
		}
		*/

		increaseTimes(); // for stuff like jumpTime;
>>>>>>> origin/master
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
		
<<<<<<< HEAD
		m_playerTransform.Translate(Vector3.ClampMagnitude(m_playerMovement, Mathf.Abs(m_YModifier)));
=======
		transform.Translate(Vector3.ClampMagnitude(m_playerMovement, forwardSpeed));
>>>>>>> origin/master
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
<<<<<<< HEAD
		                              m_playerAttributes.attRange/2,
=======
		                              attackRange,
>>>>>>> origin/master
		                              transform.TransformDirection(Vector3.forward),
		                              out objectHit,
		                              m_playerAttributes.attRange))
		{
<<<<<<< HEAD
			objectHit.rigidbody.AddForce(objectHit.normal*m_playerAttributes.strength*100*-1);
			if(objectHit.transform.gameObject.tag == "Destructible") // inanimate things that can be destroyed
=======
			objectHit.rigidbody.AddForce(objectHit.normal*hitForce*-1);
			if(objectHit.transform.gameObject.tag == "Destructible")
>>>>>>> origin/master
			{
				// play destroyed animation
				StartCoroutine(GameController.animDestroy(objectHit.transform.gameObject, 1.0f)); // delayed destroy
			}
		}
	}

<<<<<<< HEAD
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
=======
	void increaseTimes()
	{
		m_jumpTime -= Time.deltaTime;
>>>>>>> origin/master
	}
}
