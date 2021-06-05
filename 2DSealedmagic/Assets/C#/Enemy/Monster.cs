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
	[Header("���� ����")]
	[Tooltip("���� �����̴� �ӵ�")]
	[SerializeField] float moveSpeed;
	[Tooltip("���� ü��")]
	public float life;
	[Tooltip("������ �����ϱ� ���� ��������?")]
	[SerializeField] bool isColoredMonster;

	[Header("��ų")]
	[Tooltip("���Ͱ� ��ų�� ������ �ִ°�?")]
	[SerializeField] bool hasSkill;
	[Tooltip("�ι�° ��ų�� ������ �ִ°�?/hasSkill�� üũ�� ������ ��츸")]
	[SerializeField] bool has2ndSkill;
	[Tooltip("����ϴ� ��ų�� �ε��� ��ȣ (int)")]
	[SerializeField] int skillIndex;
	[Tooltip("����ϴ� �� ��° ��ų�� �ε��� ��ȣ (int)")]
	[SerializeField] int skill2Index;
	//[Tooltip("����ϴ� ��ų�� ���� ���ð� (float)")]
	//[SerializeField] float skillCooldown;

	[Header("����")]
	[Tooltip("�Ϲ� ���� ���ط� (float)")]
	[SerializeField] float normalAttackDamage;
	[Tooltip("�Ϲ� ���� ���� ���ð� (float)")]
	[SerializeField] float normalAttackCooldown;
	[Tooltip("�Ϲ� ���� ��Ÿ� (float)")]
	[SerializeField] float normalAttackRange;
	[Tooltip("���� �Ϲ� ���� ���� ������Ʈ")]
	[SerializeField] GameObject monsterAttackArea;
	[Tooltip("���� �Ϲݰ��� ����Ʈ")]
	[SerializeField] GameObject normalAtkFX;

	[Header("���� ����")]
	[Tooltip("���Ͱ� ���� ������ �̹���")]
	public GameObject AttackDamageText;
	[Tooltip("���Ͱ� ���� ������ ��ġ")]
	public Transform hudPos;
	[Tooltip("�˹���ϴ� ��")]
	public float power = 1f;

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

	bool canAttack = true;
	bool isAttacking = false;
	public bool canSkill_1 = false; // ���� ��ų ��� �������� ����
	public bool canSkill_2 = false;

	bool onStunned = false;
	bool onProtected = false;
	public bool OnProtected { get { return onProtected; } set { onProtected = value; } }
	//bool onFired = false;
	//bool onSpeedChanged = false;

	void Awake()
	{
		monsterAnim = gameObject.GetComponentInChildren<Animator>();
		rigid = GetComponent<Rigidbody2D>();
		spRenderer = GetComponent<SpriteRenderer>();

		if (hasSkill) canSkill_1 = true;
		if (has2ndSkill) canSkill_2 = true;

		Invoke("Think", Random.Range(3, 6));
	}

	void Start()
	{
		if (isColoredMonster)
		{
			GameObject coloredFX = Instantiate(GameManager.instance.ColoredMonsterFX, transform.position, Quaternion.identity);
			coloredFX.transform.SetParent(this.transform);
			coloredFX.transform.localScale = new Vector3(1, 1, 1);
		}
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
		if (!isDead || !onStunned)
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
				else if (plPos.x > transform.position.x)
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
		if (moveDir != 0 && !isDead && !onStunned)
		{
			spRenderer.flipX = moveDir == -1 ? true : false;
			monsterAnim.SetBool("isWalk", true);
		}
		else if (moveDir == 0 || onStunned)
			monsterAnim.SetBool("isWalk", false);


		if (!isDead && !isAttacking && !isPlayerInAttackRange && !onStunned)
		{
			rigid.velocity = new Vector2(moveDir * moveSpeed * changedSpeed, rigid.velocity.y);
		}
		else if (isPlayerInAttackRange || isAttacking || isDead || onStunned)
		{
			rigid.velocity = Vector2.zero;
		}
	}

	// ������ �õ�
	void TryAttack()
	{
		if (isPlayerInAttackRange && !onStunned && !isDead)
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
		if (onStunned)
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
		int i = 0;
		changedSpeed = speed;
		//onSpeedChanged = true;

		while (i <= duration)
		{
			yield return new WaitForSeconds(1f);
			i++;

			if (duration < i)
			{
				changedSpeed = 1f;
				//onSpeedChanged = false;
			}
		}
	}

	// ��Ʈ ���ظ� �Դ� ����
	IEnumerator dotDamageState(float damage, float duration)
	{
		int i = 0;
		//onFired = true;

		while (i < duration)
		{
			yield return new WaitForSeconds(1f);
			onAttack(damage);
			i++;
			//onFired = false;
		}
	}

	// ���� ����
	IEnumerator StunState(float duration)
	{
		int i = 0;
		onStunned = true;
		isAttacking = false;
		StopCoroutine("SkillAttack");

		GameObject stunFX = Instantiate(MobSkillsInfo.instance.FX_Stun, transform.position + Vector3.up*0.75f, Quaternion.identity);
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

		if (!onProtected)
		{
			life -= damage;
			StartCoroutine("onDamaged");
		}
		else
		{
			AttackText.GetComponent<DmgText>().damage = 0;
			onProtected = false;
		}

		if (life <= 0 && !isDead)
		{
			isDead = true;
			monsterAnim.SetBool("isWalk", false);
			monsterAnim.SetBool("isAttack", false);

			if (isColoredMonster)
			{
				GameManager.instance.killedColoredMonster += 1;
			}

			StartCoroutine("Die");
		}
	}

	IEnumerator Die()
	{
		float i = 1f;
		while (i >= 0f)
		{
			spRenderer.color = new Color(1, 1, 1, i);
			i -= 0.1f;
			yield return new WaitForSeconds(0.1f);
			if (i <= 0.1f)
			{
				Destroy(gameObject);
			}
		}
	}

	// ���ظ� ���� ǥ��
	IEnumerator onDamaged()
	{

		spRenderer.color = new Color(1, 1, 1, 0.6f);
		rigid.AddForce(Vector2.up * (1.4f * power), ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.5f);
		spRenderer.color = new Color(1, 1, 1, 1);
		power = 1f;
	}

	// ��ų ���� (��ų�� ��Ÿ���̶�� �⺻ ����)
	IEnumerator SkillAttack()
	{
		moveDir = 0;
		if (hasSkill && (canSkill_1 || canSkill_2))
		{
			isAttacking = true;
			MonsterSkill sk = GetComponent<MonsterSkill>();

			// 1�� ��ų ���
			if (canSkill_1 && !canSkill_2)
			{
				canSkill_1 = false;

				sk.UseSkill(skillIndex, (int)atkDir, 1);
			}
			// 2�� ��ų ���
			else if (!canSkill_1 && canSkill_2)
			{
				canSkill_2 = false;

				sk.UseSkill(skill2Index, (int)atkDir, 2);
			}
			else
			{

				int rand = Random.Range(1, 3); // 1~2
				switch (rand)
				{
					case 1:
						canSkill_1 = false;
						sk.UseSkill(skillIndex, (int)atkDir, 1);
						break;
					case 2:
						canSkill_2 = false;
						sk.UseSkill(skill2Index, (int)atkDir, 2);
						break;
					default:
						Debug.Log("ERROR");
						break;
				}
			}

			yield return new WaitForSeconds(3f);
			isAttacking = false;
		}
		else if (!hasSkill || (!canSkill_1 && !canSkill_2))
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
