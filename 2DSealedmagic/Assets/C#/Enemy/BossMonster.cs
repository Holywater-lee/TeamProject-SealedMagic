using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼��� (P)
�ۼ� �ǵ�:
	���� ������ '�⺻' �������� ������� �Ͽ���.
	��ü���� �ൿ�� �ڽ� ��ũ��Ʈ�� �����. (�� ��ũ��Ʈ�� �θ�)
	���� �ִ��� public�� ����� �����Ͽ���...(���뼺�� ����)
	���͸� ���� �� ���������� �޾��� ���׵��� �������� ����Ͽ���....
����:
	���� ���� �⺻ �ൿ ��ũ��Ʈ
	������ �ڽ� ������Ʈ���� ����........
*/

public class BossMonster : MonoBehaviour
{
	[Header("�ɷ�ġ")]
	[Tooltip("���� ���ط�")]
	[SerializeField] float atkDamage;
	[Tooltip("���ݼӵ�")]
	[SerializeField] float atkSpeed;
	[Tooltip("���� ���� ���ð�")]
	[SerializeField] float atkDelay;
	[Tooltip("�̵��ӵ�")]
	[SerializeField] float moveSpeed;
	[Tooltip("�ִ� �����")]
	[SerializeField] float maxLife;
	[Tooltip("���� �����")]
	[SerializeField] float curLife;

	[Header("���� �� ����Ʈ")]
	[Tooltip("�÷��̾� �ν� ���� ����")]
	[SerializeField] Vector2 recognizePlayerZone;
	[Tooltip("���� ���� ����")]
	[SerializeField] Vector2 atkZone;
	[Tooltip("���� ������ ���� ����")]
	[SerializeField] Vector2 atkPos;
	[Tooltip("���� ����Ʈ: ��������Ʈ")]
	[SerializeField] GameObject atkFX_Sprite;
	[Tooltip("���� ����Ʈ: ��ƼŬ")]
	[SerializeField] GameObject atkFX_Particle;

	[Header("�÷��̾ ������ ��")]
	public LayerMask plCheck;

	[Header("������ ����")]
	[Tooltip("���Ͱ� ���� ������ �̹���")]
	[SerializeField] GameObject AttackDamageText;
	[Tooltip("���Ͱ� ���� ������ ��ġ")]
	[SerializeField] Vector3 hudPos;

	//[Header("����")]
	//public List<BuffBase> onBuff = new List<BuffBase>();

	//public static BossMonster instance;

	protected Animator anim;
	protected Rigidbody2D rigid;
	protected SpriteRenderer spRenderer;
	PlayerObject traceTarget;

	protected bool isAtk = false;
	bool isTracing = false;
	protected bool isDead = false;
	protected bool isPlayerInAtkRange = false;

	protected bool onStunned = false;

	int moveDirection = 0;

	void Awake()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		spRenderer = GetComponent<SpriteRenderer>();

		StartCoroutine(SearchPlayer());
	}

	void FixedUpdate()
	{
		CheckRaycast();
		Move();
	}

	// �ڽ� ������Ʈ���� �������� ��
	protected virtual void TryAttack()
	{
		
	}

	void CheckRaycast()
	{
		if (!isDead || !onStunned)
		{
			LayerMask plfLayer = new LayerMask();
			plfLayer = LayerMask.GetMask("Platform");
			LayerMask spLayer = new LayerMask();
			spLayer = LayerMask.GetMask("Spikes");

			Vector2 front = new Vector2(transform.position.x + moveDirection, transform.position.y);
			RaycastHit2D rayPlatform = Physics2D.Raycast(front, Vector2.down, 1f, plfLayer.value);
			RaycastHit2D raySpikes = Physics2D.Raycast(front, Vector2.down, 1f, spLayer.value);
			Debug.DrawRay(front, Vector2.down, Color.red);

			if (!rayPlatform.collider || raySpikes.collider)
			{
				moveDirection = moveDirection * -1;
			}
			else if (isTracing && !isAtk)
			{
				Vector2 plPos = traceTarget.transform.position;

				if (plPos.x < transform.position.x)
				{
					moveDirection = -1;
				}
				else if (plPos.x >= transform.position.x)
				{
					moveDirection = 1;
				}
			}
			else if (isAtk || isPlayerInAtkRange)
				moveDirection = 0;
		}
	}

	void Move()
	{
		if (moveDirection != 0 && !isDead && !onStunned)
		{
			spRenderer.flipX = moveDirection == -1 ? true : false;
			anim.SetBool("isWalk", true);
		}
		else if (moveDirection == 0 || onStunned)
			anim.SetBool("isWalk", false);


		if (!isDead && !isAtk && !isPlayerInAtkRange && !onStunned)
		{
			rigid.velocity = new Vector2(moveDirection * moveSpeed, rigid.velocity.y);
		}
		else if (isPlayerInAtkRange || isAtk || isDead || onStunned)
		{
			rigid.velocity = Vector2.zero;
		}
	}

	IEnumerator SearchPlayer()
	{
		while (!isTracing)
		{
			CheckPlayer();
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator SearchInAtkRange()
	{
		while (!isDead)
		{
			if (!isPlayerInAtkRange)
			{
				CheckInAtkRange();
			}
			if (isPlayerInAtkRange)
			{
				TryAttack();
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	void CheckInAtkRange()
	{
		Vector3 newAtkPos = new Vector3(moveDirection * atkPos.x + transform.position.x, atkPos.y + transform.position.y, transform.position.z);
		Collider2D[] PlayerChk = Physics2D.OverlapBoxAll(newAtkPos, atkZone, 0, plCheck);
		if (PlayerChk.Length == 1)
		{
			isPlayerInAtkRange = true;
		}
	}

	void CheckPlayer()
	{
		Collider2D[] PlayerChk = Physics2D.OverlapBoxAll(transform.position, recognizePlayerZone, 0, plCheck);
		if (PlayerChk.Length == 1)
		{
			traceTarget = FindObjectOfType<PlayerObject>();
			StartCoroutine(SearchInAtkRange());
			isTracing = true;
		}
	}

	public void onAttack(float damage)
	{
		GameObject AttackText = Instantiate(AttackDamageText);// ������ �̹��� ���
		AttackText.transform.position = transform.position + hudPos;// ������ ��ġ
		AttackText.GetComponent<DmgText>().damage = damage;// ������ �� �ޱ�

		curLife -= damage;
		StartCoroutine("onDamaged");

		if (curLife <= 0 && !isDead)
		{
			isDead = true;
			anim.SetBool("isWalk", false);
			anim.SetBool("isAttack", false);

			StartCoroutine("Die");
		}
	}

	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);

		yield return new WaitForSeconds(0.5f);
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	IEnumerator Die()
	{
		float i = 1f;
		while (i >= 0f)
		{
			spRenderer.color = new Color(1, 1, 1, i);
			i -= 0.05f;
			yield return new WaitForSeconds(0.05f);
			if (i <= 0.05f)
			{
				Destroy(gameObject);
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f);
		Gizmos.DrawWireCube(transform.position, recognizePlayerZone);

		Vector3 newAtkPos = new Vector3(moveDirection * atkPos.x + transform.position.x, atkPos.y + transform.position.y, transform.position.z);
		Gizmos.color = new Color(0f, 1f, 1f);
		Gizmos.DrawWireCube(newAtkPos, atkZone);
	}
}
