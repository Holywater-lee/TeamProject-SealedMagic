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
	�ڽĿ�����Ʈ���� ������ ������ isPlayerInAtkRange = false�� ������Ѵ�...
*/

public class BossMonster : MonoBehaviour
{
	[Tooltip("���� ������ ����ĳ��Ʈ�� ����")]
	[SerializeField] float raycastHeight;

	[Header("�ɷ�ġ")]
	[Tooltip("���� ���ط�")]
	[SerializeField] protected float atkDamage;
	//[Tooltip("���ݼӵ�")]
	//[SerializeField] float atkSpeed;
	[Tooltip("���� ���� ���ð� (0.5�� ����)")]
	[SerializeField] protected float atkDelay;
	[Tooltip("�̵��ӵ�")]
	[SerializeField] float moveSpeed;
	[Tooltip("�ִ� �����")]
	[SerializeField] float maxLife;
	[Tooltip("���� �����")]
	[SerializeField] float curLife;

	[Header("���� �� ����Ʈ")]
	[Tooltip("�÷��̾� �ν� ���� ����")]
	[SerializeField] Vector2 recognizePlayerZone;
	[Tooltip("�÷��̾� ���� �ν� ���� ����")]
	[SerializeField] Vector2 recognizeAtkZone;
	[Tooltip("���� ���� ����")]
	[SerializeField] protected GameObject atkZone;
	[Tooltip("���� ������ ���� ����")]
	[SerializeField] protected Vector2 atkPos;
	[Tooltip("���� ����Ʈ: ��������Ʈ")]
	[SerializeField] protected GameObject atkFX_Sprite;
	[Tooltip("���� ����Ʈ: ��ƼŬ")]
	[SerializeField] protected GameObject atkFX_Particle;

	[Header("����")]
	[Tooltip("���� ����")]
	[SerializeField] AudioClip SFX_Attack;
	[Tooltip("��ų1 ����")]
	[SerializeField] AudioClip SFX_Skill_1;
	[Tooltip("��ų2 ����")]
	[SerializeField] AudioClip SFX_Skill_2;
	[Tooltip("��� ����")]
	[SerializeField] AudioClip SFX_Death;

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
	AudioSource audioSource;

	protected bool isAtk = false;
	//protected bool isAtkCooldown = false;
	bool isTracing = false;
	protected bool isDead = false;
	protected bool isPlayerInAtkRange = false;

	protected bool onStunned = false;

	int moveDirection = 0;
	protected int atkDirection = 0;
	protected float additionalRange = 0f;
	protected float changedSpeed = 1f;

	void Awake()
	{
		anim = gameObject.GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		spRenderer = GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();

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
			RaycastHit2D rayPlatform = Physics2D.Raycast(front, Vector2.down , raycastHeight, plfLayer.value);
			RaycastHit2D raySpikes = Physics2D.Raycast(front, Vector2.down, raycastHeight, spLayer.value);
			Debug.DrawRay(front, Vector2.down * raycastHeight, Color.red);

			if (moveDirection == 1) atkDirection = 1;
			else if (moveDirection == -1) atkDirection = -1;

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
			spRenderer.flipX = moveDirection == 1 ? true : false;
			anim.SetBool("isWalk", true);
		}
		else if (moveDirection == 0 || onStunned)
			anim.SetBool("isWalk", false);

		if (!isDead && !isAtk && !isPlayerInAtkRange && !onStunned)
		{
			rigid.velocity = new Vector2(moveDirection * moveSpeed * changedSpeed, rigid.velocity.y);
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
			if (!isPlayerInAtkRange && !onStunned)
			{
				CheckInAtkRange();
			}
			if (isPlayerInAtkRange && !onStunned)
			{
				TryAttack();
			}
			yield return new WaitForSeconds(0.1f);
		}
	}

	protected void CheckInAtkRange()
	{
		Vector3 newAtkPos = new Vector3(atkDirection * (atkPos.x + additionalRange) + transform.position.x, atkPos.y + transform.position.y, transform.position.z);
		Collider2D[] PlayerChk = Physics2D.OverlapBoxAll(newAtkPos, recognizeAtkZone, 0, plCheck);
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
		if (!isDead)
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
				gameObject.tag = "Untagged";

				StartCoroutine("Die");
			}
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
		Sound("Death");
		while (i >= 0f)
		{
			spRenderer.color = new Color(1, 1, 1, i);
			i -= 0.05f;
			yield return new WaitForSeconds(0.05f);
			if (i <= 0.05f)
			{
				GameClear.instance.BossDead();
				Destroy(gameObject);
			}
		}
	}

	public void modifySpeed(float speed, float duration)
	{
		if (changedSpeed != 1f)
		{
			StopCoroutine("changeSpeedState");
			StartCoroutine(changeSpeedState(speed, duration));
		}
		else
			StartCoroutine(changeSpeedState(speed, duration));
	}

	public void startDotDamage(float damage, float duration)
	{
		StartCoroutine(dotDamageState(damage, duration));
	}

	public void startStun(float duration)
	{
		if (onStunned)
		{
			StopCoroutine("StunState");
			StartCoroutine(StunState(duration));
		}
		else
			StartCoroutine(StunState(duration));
	}

	IEnumerator changeSpeedState(float speed, float duration)
	{
		int i = 0;
		changedSpeed = speed;

		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;

			if (duration < i)
			{
				changedSpeed = 1f;
			}
		}
	}

	IEnumerator dotDamageState(float damage, float duration)
	{
		int i = 0;

		while (i < duration)
		{
			yield return new WaitForSeconds(1f);
			onAttack(damage);
			i++;
		}
	}

	IEnumerator StunState(float duration)
	{
		int i = 0;
		onStunned = true;
		isAtk = false;

		GameObject stunFX = Instantiate(MobSkillsInfo.instance.FX_Stun, transform.position + Vector3.up * 0.75f, Quaternion.identity);
		Destroy(stunFX, duration);

		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;
			if (duration <= i)
			{
				onStunned = false;
			}
		}
	}

	protected void Sound(string activity)
	{
		switch (activity)
		{
			case "Attack":
				audioSource.clip = SFX_Attack;
				break;
			case "Skill1":
				audioSource.clip = SFX_Skill_1;
				break;
			case "Skill2":
				audioSource.clip = SFX_Skill_2;
				break;
			case "Death":
				audioSource.clip = SFX_Death;
				break;
			default:
				Debug.Log("ERROR: �ش� ���� �� ����");
				break;
		}

		if (audioSource != null)
			audioSource.Play();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f);
		Gizmos.DrawWireCube(transform.position, recognizePlayerZone);

		Vector3 newAtkPos = new Vector3(atkDirection * (atkPos.x + additionalRange) + transform.position.x, atkPos.y + transform.position.y, transform.position.z);
		Gizmos.color = new Color(0f, 1f, 1f);
		Gizmos.DrawWireCube(newAtkPos, recognizeAtkZone);
	}
}
