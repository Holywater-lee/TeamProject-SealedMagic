using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���� �⺻ ������
���߿� �� ��:
���� ��� �� ���İ� �������� �ϱ�
*/

public class Monster : MonoBehaviour
{
	[Tooltip("���� �����̴� �ӵ�")]
	[SerializeField] float moveSpeed;
	[Tooltip("���� ü��")]
	public float life;
	[Tooltip("���Ͱ� ��ų�� ������ �ִ°�?")]
	[SerializeField] bool hasSkill;
	[Tooltip("����ϴ� ��ų�� �ε��� ��ȣ (int)")]
	[SerializeField] int skillIndex;
	//[Tooltip("����ϴ� ��ų�� ���� ���ð� (float)")]
	//[SerializeField] float skillCooldown;
	[Tooltip("�Ϲ� ���� ���ط� (float)")]
	[SerializeField] float normalAttackDamage;
	[Tooltip("�Ϲ� ���� ���� ���ð� (float)")]
	[SerializeField] float normalAttackCooldown;
	[Tooltip("�Ϲ� ���� ��Ÿ� (float)")]
	[SerializeField] float normalAttackRange;
	[Tooltip("������ �����ϱ� ���� ��������?")]
	[SerializeField] bool isColoredMonster;
	[Tooltip("���� �Ϲ� ���� ���� ������Ʈ")]
	[SerializeField] GameObject monsterAttackArea;
	[Tooltip("���� �Ϲݰ��� ����Ʈ")]
	[SerializeField] GameObject normalAtkFX;

	[Tooltip("���Ͱ� ���� ������ �̹���")]
	public GameObject AttackDamageText;
	[Tooltip("���Ͱ� ���� ������ ��ġ")]
	public Transform hudPos;

	Animator monsterAnim;
	Rigidbody2D rigid;
	SpriteRenderer spRenderer;

	[HideInInspector] public GameObject traceTarget;
	[HideInInspector] public bool isTracing = false;

	bool isPlayerInAttackRange = false;

	bool isDead = false;
	float moveDir = 1;
	float changedSpeed = 1;
	float atkDir;
	float delay = 0f;

	bool canSkill = true; // ���� ��ų ��� �������� ����
	public bool CanSkill
	{
		get
		{
			return canSkill;
		}
		set
		{
			canSkill = value;
		}
	}
	bool canAttack = true;

	bool isAttacking = false;
	bool isStunned = false;

	void Awake()
	{
		monsterAnim = gameObject.GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		spRenderer = GetComponent<SpriteRenderer>();

		Invoke("Think", Random.Range(3, 6));
	}

	void FixedUpdate()
	{
		CheckRaycast();
		Move();
		TryAttack();
	}

	// ����ĳ��Ʈ üũ
	void CheckRaycast()
	{
		if (!isDead || !isStunned)
		{
			LayerMask plform = new LayerMask();
			plform = LayerMask.GetMask("Platform");
			LayerMask spLayer = new LayerMask();
			spLayer = LayerMask.GetMask("Spikes");
			LayerMask plLayer = new LayerMask();
			plLayer = LayerMask.GetMask("Player");

			Vector2 front = new Vector2(rigid.position.x + moveDir, rigid.position.y);
			RaycastHit2D rayPlatform = Physics2D.Raycast(front, Vector2.down, 1f, plform.value);
			RaycastHit2D raySpikes = Physics2D.Raycast(front, Vector2.down, 1f, spLayer.value);
			Debug.DrawRay(front, Vector2.down, Color.red);

			if (moveDir == 1) atkDir = 1;
			else if (moveDir == -1) atkDir = -1;

			RaycastHit2D rayPlayer = Physics2D.Raycast(transform.position, new Vector2(atkDir, 0), normalAttackRange, plLayer.value);
			Debug.DrawRay(transform.position, new Vector2(atkDir, 0) * normalAttackRange, Color.green);

			if (rayPlayer.collider) isPlayerInAttackRange = true;
			else if (!rayPlayer.collider) isPlayerInAttackRange = false;

			if (!rayPlatform.collider || raySpikes.collider)
			{
				moveDir = moveDir * -1;
				CancelInvoke();
				Invoke("Think", 2);
			}
			else if (isTracing && !isPlayerInAttackRange && !isAttacking)
			{
				Vector2 plPos = traceTarget.transform.position;

				if (plPos.x < transform.position.x)
				{
					moveDir = -1;
				}
				else if (plPos.x >= transform.position.x)
				{
					moveDir = 1;
				}
			}
			else if (isPlayerInAttackRange || isAttacking)
				moveDir = 0;
		}
	}

	// �̵�
	void Move()
	{
		if (moveDir != 0 && !isDead && !isStunned)
		{
			spRenderer.flipX = moveDir == -1 ? true : false;
			monsterAnim.SetBool("isWalk", true);
		}
		else if (moveDir == 0 || isStunned)
			monsterAnim.SetBool("isWalk", false);


		if (!isDead && !isAttacking && !isPlayerInAttackRange && !isStunned)
		{
			rigid.velocity = new Vector2(moveDir * moveSpeed * changedSpeed, rigid.velocity.y);
		}
		else if (isPlayerInAttackRange || isAttacking || isDead || isStunned)
		{
			rigid.velocity = Vector2.zero;
		}

		if (isDead)
		{
			gameObject.tag = default;
		}
	}
	
	// ������ �õ�
	void TryAttack()
	{
		if(isPlayerInAttackRange && !isStunned && !isDead)
		{
			delay += Time.deltaTime;

			if (delay >= 1f)
			{
				delay = 0f;
				StartCoroutine("SkillAttack");
			}
		}
		
	}

	// �̵��ӵ� ���� ���� ����
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
	
	// ��Ʈ ���� ���� ����
	public void startDotDamage(float damage, float duration)
	{
		StartCoroutine(dotDamageState(damage, duration));
	}

	// ���� ���� ����
	public void startStun(float duration)
	{
		if (isStunned)
		{
			StopCoroutine("StunState");
			StartCoroutine(StunState(duration));
		}
		else
			StartCoroutine(StunState(duration));
	}

	// speed �Ű������� 1 = 100%
	// �̵��ӵ��� ����� ����
	IEnumerator changeSpeedState(float speed, float duration)
	{
		//Debug.Log("1. �ڷ�ƾ����");
		int i = 0;
		changedSpeed = speed;

		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;
			//Debug.Log("2. i++, i: " + i);
			if (duration < i)
			{
				changedSpeed = 1f;
				//Debug.Log("3. �ӵ� �������, ��ٸ� �ð�: " + i);
			}
		}
	}

	// ��Ʈ ���ظ� �Դ� ����
	IEnumerator dotDamageState(float damage, float duration)
	{
		int i = 0;
		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			onAttack(damage);
			i++;
		}
	}

	// ���� ����
	IEnumerator StunState(float duration)
	{
		int i = 0;
		isStunned = true;
		isAttacking = false;
		StopCoroutine("SkillAttack");
		// GameObject stunFX = Instantiate(FX_Stun, trasform.position + vector3.up*2f, Quaternion.identity);
		// Destroy(stunFX, 2f);
		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;
			if (duration < i)
			{
				isStunned = false;
			}
		}
	}

	// �̵� ������ ����
	void Think()
	{
		moveDir = Random.Range(-1, 2);
		float ThinkTime = Random.Range(1f, 3f);
		Invoke("Think", ThinkTime);
	}

	// ������ ����
	public void onAttack(float damage)
	{
		GameObject AttackText = Instantiate(AttackDamageText);// ������ �̹��� ���
		AttackText.transform.position = hudPos.position;// ������ ��ġ
		AttackText.GetComponent<DmgText>().damage = damage;// ������ �� �ޱ�

		life -= damage;
		StartCoroutine("onDamaged");
		if (life <= 0 && !isDead)
		{
			isDead = true;
			monsterAnim.SetBool("isWalk", false);
			monsterAnim.SetBool("isAttack", false);
			monsterAnim.SetTrigger("isDeath");
			if (isColoredMonster)
			{
				GameManager gameManager = FindObjectOfType<GameManager>();
				gameManager.killedColoredMonster += 1;
			}
			Destroy(gameObject, 1f);
		}
	}

	// ���ظ� ���� ǥ��
	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);
		rigid.AddForce(Vector2.up * 1.4f, ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.5f);
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	// ��ų ���� (��ų�� ��Ÿ���̶�� �⺻ ����)
	IEnumerator SkillAttack()
	{
		moveDir = 0;
		if (hasSkill && canSkill)
		{
			isAttacking = true;
			canSkill = false;

			MonsterSkill sk = GetComponent<MonsterSkill>();
			sk.UseSkill(skillIndex, (int)atkDir);
			yield return new WaitForSeconds(1f);
			isAttacking = false;
		}
		else if (!hasSkill || !canSkill)
		{
			if (canAttack)
			{
				isAttacking = true;

				yield return new WaitForSeconds(0.1f);
				StartCoroutine("normalAttack");

				StartCoroutine("CoolingAttack");
			}
		}
	}

	// ��ų ���� ���ð� ���
	/*
	IEnumerator CoolingSkill()
	{
		canSkill = false;
		yield return new WaitForSeconds(skillCooldown);
		canSkill = true;
	}*/

	// ���� ���� ���ð� ���
	IEnumerator CoolingAttack()
	{
		canAttack = false;
		yield return new WaitForSeconds(normalAttackCooldown);
		canAttack = true;
	}

	// �Ϲ� ����
	IEnumerator normalAttack()
	{
		Vector2 front = new Vector2((spRenderer.flipX ? -1 : 1) * (normalAttackRange / 2 * 1.3f) + transform.position.x, transform.position.y);

		GameObject atkArea = Instantiate(monsterAttackArea, front, Quaternion.identity);
		var area = atkArea.GetComponent<AttackArea>();
		if (area != null)
		{
			area.isEnemyAttack = true;
			area.damage = normalAttackDamage;
		}

		GameObject atkFX = Instantiate(normalAtkFX, front, Quaternion.identity);
		monsterAnim.SetBool("isAttack", true);

		// ���� ������ 0.1�ʸ� ����
		yield return new WaitForSeconds(0.1f);
		Destroy(atkArea);

		yield return new WaitForSeconds(0.5f);
		Destroy(atkFX);

		monsterAnim.SetBool("isAttack", false);
		isAttacking = false;
	}

	/*
	void isDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);
		rigid.AddForce(Vector2.up * 1.4f, ForceMode2D.Impulse);
		Invoke("DamagedToNormal", 0.5f);
	}

	void DamagedToNormal()
	{
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	void CooldownSkill()
	{
		canSkill = false;
		skCool -= Time.deltaTime;

		if (skCool <= 0f)
		{
			Debug.Log("��ų �� ��!");
			canSkill = true;
			skCool = skillCooldown;
			//coolingSkill = false;
		}
	}

	void CooldownNormalAttack()
	{
		canAttack = false;
		atkCool -= Time.deltaTime;

		if (atkCool <= 0f)
		{
			Debug.Log("�Ϲݰ��� �� ��!");
			canAttack = true;
			atkCool = normalAttackCooldown;
			//coolingAtk = false;
		}
	}

	void UseSkill()
	{
		if (canUseSkill && canSkill)
		{
			isAttacking = true;
			// MonsterSkill.Skill(0);
			Debug.Log("��ų ���!");

			//skCool = skillCooldown;
			//coolingSkill = true;
		}
		else if (!canUseSkill || !canSkill)
		{
			if (canAttack)
			{
				isAttacking = true;
				Invoke("NormalAttack", 0.1f);
				Debug.Log("�Ϲݰ��� ���!");

				//atkCool = normalAttackCooldown;
				//coolingAtk = true;
			}
		}
	}
	
	void NormalAttack()
	{
		Vector2 front = new Vector2((spRenderer.flipX ? -1 : 1) * (normalAttackRange/2) + rigid.position.x, rigid.position.y);

		GameObject atkArea = Instantiate(monsterAttackArea, front, Quaternion.Euler(0,0,0));
		Destroy(atkArea, 0.5f);
		
		monsterAnim.SetBool("isAttack", true);
		Invoke("EndAttack", 0.5f);
	}


	void EndAttack()
	{
		monsterAnim.SetBool("isAttack", false);
		isAttacking = false;
	}
	*/
}
