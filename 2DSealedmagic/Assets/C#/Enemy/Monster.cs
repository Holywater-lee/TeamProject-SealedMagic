using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
작성: 20181220 이성수(P)

설명: 몬스터 기본 움직임
나중에 할 것:
몬스터 사망 시 알파값 낮아지게 하기
*/

public class Monster : MonoBehaviour
{
	[Header("몬스터 변수")]
	[Tooltip("적이 움직이는 속도")]
	[SerializeField] float moveSpeed;
	[Tooltip("적의 체력")]
	public float life;
	[Tooltip("제단을 해제하기 위한 몬스터인지?")]
	[SerializeField] bool isColoredMonster;
	[Tooltip("지형을 감지할 높이. 몬스터의 키 정도로 입력")]
	[SerializeField] float raycastHeight;

	[Header("스킬")]
	[Tooltip("몬스터가 스킬을 가지고 있는가?")]
	[SerializeField] bool hasSkill;
	[Tooltip("두번째 스킬을 가지고 있는가?/hasSkill이 체크가 되있을 경우만")]
	[SerializeField] bool has2ndSkill;
	[Tooltip("사용하는 스킬의 인덱스 번호 (int)")]
	[SerializeField] int skillIndex;
	[Tooltip("사용하는 두 번째 스킬의 인덱스 번호 (int)")]
	[SerializeField] int skill2Index;
	//[Tooltip("사용하는 스킬의 재사용 대기시간 (float)")]
	//[SerializeField] float skillCooldown;

	[Header("공격")]
	[Tooltip("일반 공격 피해량 (float)")]
	[SerializeField] float normalAttackDamage;
	[Tooltip("일반 공격 재사용 대기시간 (float)")]
	[SerializeField] float normalAttackCooldown;
	[Tooltip("일반 공격 사거리 (float)")]
	[SerializeField] float normalAttackRange;
	[Tooltip("몬스터 일반 공격 범위 오브젝트")]
	[SerializeField] GameObject monsterAttackArea;
	[Tooltip("몬스터 일반공격 이펙트")]
	[SerializeField] GameObject normalAtkFX;

	[Header("몬스터 피해")]
	[Tooltip("몬스터가 받은 데미지 이미지")]
	public GameObject AttackDamageText;
	[Tooltip("몬스터가 받은 데미지 위치")]
	public Transform hudPos;

	[Header("사운드")]
	[Tooltip("공격 사운드")]
	[SerializeField] AudioClip SFX_Attack;
	[Tooltip("스킬1 사운드")]
	[SerializeField] AudioClip SFX_Skill_1;
	[Tooltip("스킬2 사운드")]
	[SerializeField] AudioClip SFX_Skill_2;
	[Tooltip("사망 사운드")]
	[SerializeField] AudioClip SFX_Death;

	// 넉백당하는 '추가' 힘
	[HideInInspector] public float power = 1f;

	Animator monsterAnim;
	Rigidbody2D rigid;
	SpriteRenderer spRenderer;
	[SerializeField] AudioSource audioSource;

	[HideInInspector] public GameObject traceTarget;
	[HideInInspector] public bool isTracing = false;

	bool isPlayerInAttackRange = false;

	bool isDead = false;
	float moveDir = 0;
	float changedSpeed = 1;
	float atkDir;
	float delay = 0f;

	bool canAttack = true;
	bool isAttacking = false;
	[HideInInspector] public bool canSkill_1 = false; // 현재 스킬 사용 가능한지 여부
	[HideInInspector] public bool canSkill_2 = false;

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

	// 레이캐스트 체크
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
			RaycastHit2D rayPlatform = Physics2D.Raycast(front, Vector2.down, raycastHeight, plform.value);
			RaycastHit2D raySpikes = Physics2D.Raycast(front, Vector2.down, raycastHeight, spLayer.value);
			Debug.DrawRay(front, Vector2.down * raycastHeight, Color.red);

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

	// 이동
	void Move()
	{
		if (moveDir != 0 && !isDead && !onStunned)
		{
			spRenderer.flipX = moveDir == 1 ? true : false;
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

	// 공격을 시도
	void TryAttack()
	{
		if (isPlayerInAttackRange && !onStunned && !isDead)
		{
			delay += Time.deltaTime;

			if (delay >= 0.1f)
			{
				delay = 0f;
				StartCoroutine("SkillAttack");
			}
		}
	}

	// 이동속도 변경 상태 시작
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

	// 도트 피해 상태 시작
	public void startDotDamage(float damage, float duration)
	{
		StartCoroutine(dotDamageState(damage, duration));
	}

	// 스턴 상태 시작
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

	// speed 매개변수는 1 = 100%
	// 이동속도가 변경된 상태
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

	// 도트 피해를 입는 상태
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

	// 기절 상태
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

	// 이동 방향을 생각
	void Think()
	{
		moveDir = Random.Range(-1, 2);
		float ThinkTime = Random.Range(1f, 3f);
		Invoke("Think", ThinkTime);
	}

	// 공격을 받음
	public void onAttack(float damage)
	{
		if (!isDead)
		{
			GameObject AttackText = Instantiate(AttackDamageText);// 데미지 이미지 출력
			AttackText.transform.position = hudPos.position;// 데미지 위치
			AttackText.GetComponent<DmgText>().damage = damage;// 데미지 값 받기

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
				gameObject.tag = "Untagged";

				if (isColoredMonster)
				{
					GameManager.instance.killedColoredMonster += 1;
				}

				StartCoroutine("Die");
			}
		}
	}

	IEnumerator Die()
	{
		float i = 1f;
		Sound("Death");
		while (i >= 0f)
		{
			spRenderer.color = new Color(1, 1, 1, i);
			i -= 0.2f;
			yield return new WaitForSeconds(0.2f);
			if (i <= 0.1f)
			{
				Destroy(gameObject);
			}
		}
	}

	// 피해를 입음 표시
	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);
		rigid.AddForce(Vector2.up * (4f + power), ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.5f);
		spRenderer.color = new Color(1, 1, 1, 1);
		power = 1f;
	}

	// 스킬 공격 (스킬이 쿨타임이라면 기본 공격)
	IEnumerator SkillAttack()
	{
		moveDir = 0;
		if (hasSkill && (canSkill_1 || canSkill_2))
		{
			isAttacking = true;
			MonsterSkill sk = GetComponent<MonsterSkill>();

			// 1번 스킬 사용
			if (canSkill_1 && !canSkill_2)
			{
				canSkill_1 = false;
				Sound("Skill1");
				sk.UseSkill(skillIndex, (int)atkDir, 1);
			}
			// 2번 스킬 사용
			else if (!canSkill_1 && canSkill_2)
			{
				canSkill_2 = false;
				Sound("Skill2");
				sk.UseSkill(skill2Index, (int)atkDir, 2);
			}
			else
			{

				int rand = Random.Range(1, 3); // 1~2
				switch (rand)
				{
					case 1:
						canSkill_1 = false;
						Sound("Skill1");
						sk.UseSkill(skillIndex, (int)atkDir, 1);
						break;
					case 2:
						canSkill_2 = false;
						Sound("Skill2");
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

				yield return new WaitForSeconds(0.01f);
				StartCoroutine("normalAttack");

				StartCoroutine("CoolingAttack");
			}
		}
	}

	// 스킬 재사용 대기시간 대기
	/*
	IEnumerator CoolingSkill()
	{
		canSkill = false;
		yield return new WaitForSeconds(skillCooldown);
		canSkill = true;
	}*/

	// 공격 재사용 대기시간 대기
	IEnumerator CoolingAttack()
	{
		canAttack = false;
		yield return new WaitForSeconds(normalAttackCooldown);
		canAttack = true;
	}

	// 일반 공격
	IEnumerator normalAttack()
	{
		Vector2 front = new Vector2((spRenderer.flipX ? 1 : -1) * (normalAttackRange / 2 * 1f) + transform.position.x, transform.position.y);

		GameObject atkArea = Instantiate(monsterAttackArea, front, Quaternion.identity);
		var area = atkArea.GetComponent<AttackArea>();
		if (area != null)
		{
			area.isEnemyAttack = true;
			area.damage = normalAttackDamage;
		}

		GameObject atkFX = Instantiate(normalAtkFX, front, Quaternion.identity);
		monsterAnim.SetBool("isAttack", true);

		Sound("Attack");

		// 공격 판정은 0.1초만 존재
		yield return new WaitForSeconds(0.1f);
		Destroy(atkArea);

		yield return new WaitForSeconds(0.5f);
		Destroy(atkFX);

		monsterAnim.SetBool("isAttack", false);
		isAttacking = false;
	}

	void Sound(string activity)
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
				Debug.Log("ERROR: 해당 사운드 값 없음");
				break;
		}

		if (audioSource != null)
			audioSource.Play();
	}
}
