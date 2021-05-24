using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �߰��Ѱ�: maxHealth, curHealth, maxMana, curMana �߰�
// Ȥ�� ���ݷ� ������ �� ��ũ��Ʈ�� ������ ����?

public class PlayerObject : MonoBehaviour
{
	public float maxSpeed;
	private float runSpeed = 4;

	public float jumpPower;
	private float jumpcount = 2;

	// �߰��� �κ� 21.05.18
	[Tooltip("�÷��̾��� �ִ� ü�� ����")]
	public float maxHealth;
	[Tooltip("�÷��̾��� ���� ü�� ����")]
	public float curHealth;
	[Tooltip("�÷��̾��� �ִ� ���� ����")]
	public float maxMana;
	[Tooltip("�÷��̾��� ���� ���� ����")]
	public float curMana;



	Rigidbody2D rigid;
	SpriteRenderer spriteRenderer;
	Animator anim;



	void Awake()
	{

		rigid = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();

	}

	void Update()
	{
		// Jump
		if (Input.GetButtonDown("Jump") && jumpcount > 0) // spease
		{
			if (jumpcount == 1)// double Jump
			{
				rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 1f);
			}
			rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
			anim.SetBool("Jumpingdown", false);// �ٽ� ������ ������, �������� ����� �������.
			anim.SetBool("Jumping", true);// jump image
			jumpcount--;
		}

		// Jump Max speed
		if (rigid.velocity.y > jumpPower)// right max Speed
		{
			rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 0.8f);
		}

		// Stop speed
		if (Input.GetButtonUp("Horizontal"))
		{
			rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.001f, rigid.velocity.y);
		}

		//Direction image chage
		if (Input.GetAxis("Horizontal") < 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		else if (Input.GetAxis("Horizontal") != 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}

		// Animation(classic and run)
		if (Mathf.Abs(rigid.velocity.x) < 0.5)
		{
			anim.SetBool("isWalk", false);
		}
		else if (Mathf.Abs(rigid.velocity.x) > 0.001)
		{
			anim.SetBool("isWalk", true);
		}


	}

	void FixedUpdate()
	{
		// Left and right Move
		float h = Input.GetAxisRaw("Horizontal");
		rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

		// Max speed
		if (rigid.velocity.x > maxSpeed)// right max Speed
		{
			rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
		}
		else if (rigid.velocity.x < maxSpeed * (-1))// left max Speed
		{
			rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
		}

		//run
		if (Input.GetKey(KeyCode.LeftShift))
		{
			maxSpeed = 7;
		}
		else
		{
			maxSpeed = runSpeed;
		}



		// Jump image chage (Ground Contact)
		if (rigid.velocity.y <= 0)
		{
			anim.SetBool("Jumping", false);// jump image
			anim.SetBool("Jumpingdown", true);// jump image

			Vector2 frontVec = new Vector2(rigid.position.x + rigid.velocity.x * 0.04f, rigid.position.y);
			Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));

			RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1f, LayerMask.GetMask("Platform"));


			if (rayHit.collider != null)
			{
				if (rayHit.distance < 1f)
				{
					anim.SetBool("Jumpingdown", false);// jump image
					jumpcount = 2;
				}
			}
		}
	}

	// Ememy gameObject call 
	void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Spikes")
		{
			OnDamaged(collision.transform.position);
		}
	}


	// I received Damage On
	void OnDamaged(Vector2 targetPos)
	{
		//Change Layer (Immortal Active)
		gameObject.layer = 8;

		// Damaged motion coloer
		spriteRenderer.color = new Color(1, 1, 1, 0.4f);


		// Reaction Force
		int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;// Damaged Direction
		rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

		//Animation
		anim.SetTrigger("isDamaged");

		Invoke("OffDamaged", 2);
	}

	// I received Damage Off
	void OffDamaged()
	{
		gameObject.layer = 9;
		spriteRenderer.color = new Color(1, 1, 1, 1);
	}


}
