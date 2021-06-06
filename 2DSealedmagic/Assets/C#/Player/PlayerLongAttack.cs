using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLongAttack : MonoBehaviour
{

	[Tooltip("������ �߻� �Ǵ� ��ġ")]
	public Transform firePoint;
	[Tooltip("�߻� �� Bullet �� ����Ʈ ��")]
	public GameObject[] impactEffect;
	[Tooltip("�⺻ ����/��ų ��Ÿ��")]
	[SerializeField]private float[] ccurTime;
	

	[Tooltip("��ų Ȯ��")]
	public bool[] SillIcon;
	[Tooltip("���������� ��ų Ȯ��")]
	public bool[] StageCheck; 

	[Tooltip("�⺻ ����/��ų ��Ÿ�� ����")]
	[SerializeField] private float[] classicTime;
	[Tooltip("������ ������ �۵��� �������� �̹����� ������ ���� �ȵǰ� ����")]
	private bool binCk = false;
	[Tooltip("���ܿ��� �÷��� ���ݷ�")]
	public float UpAtk = 0f;

	[Tooltip("Layer üũ")]
	public LayerMask InLayer;
	[Tooltip("�� hit ũ�� ����")]
	public Vector2 size;
	[Tooltip("���� hit ũ�� ����")]
	public Vector2 size2;
	[Tooltip("ĳ���� ������ hit ��������")]
	public int moveDic = 1;

	public static PlayerLongAttack instance;

	Animator anim;
	PlayerObject player;


	// ���ݵ��� �� ���� ���� ������ ��Ÿ�ӵ��� ���� �ٸ��� �Ϸ��� ��.
	// ����� ���� ������ �ȉ�����, ���� ���� ó����.
	void Awake()
	{
		anim = GetComponent<Animator>();
		player = FindObjectOfType<PlayerObject>();
		instance = this;
	}
   
    void Update()
	{
		MoveDic();
		Attack();
		FireAttack();
		IceAttack();
		GroundAttack();
		ThunderAttack();
	}
	void MoveDic()
	{
		if (Input.GetAxisRaw("Horizontal") == -1)
		{
			moveDic = -1;
		}
		else if (Input.GetAxisRaw("Horizontal") == 1)
		{
			moveDic = 1;
		}
	}
	void Attack()
	{
		if (ccurTime[0] <= 0)// �⺻ ����
		{
			if (Input.GetButtonDown("Attack")) // "S" Attack (classic)
			{
				anim.SetTrigger("isAttack");
				PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
				playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
				Shoot();
				ccurTime[0] = classicTime[0];
			}
		}
		else
		{
			ccurTime[0] -= Time.deltaTime;
		}
	}
	void FireAttack()
	{
		if (StageCheck[0])
		{
			if (ccurTime[1] <= 0)// �� ����
			{
				if (Input.GetButtonDown("FireAttack") && player.curMana > 0) // "Z" Attack (fire)
				{
					anim.SetTrigger("isSkill");
					PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
					playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
					FireShoot();
					SillIcon[0] = true;
					player.curMana -= 50;
					ccurTime[1] = classicTime[1];
				}
			}
			else
			{
				ccurTime[1] -= Time.deltaTime;
			}
		}

	}
	void IceAttack()
	{
		if (StageCheck[1])
		{
			if (ccurTime[2] <= 0) // ���� ����
			{
				if (Input.GetButtonDown("IceAttack") && player.curMana > 0) // "X" Attack (ice)
				{
					anim.SetTrigger("isSkill");
					PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
					playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
					IceShoot();
					SillIcon[1] = true;
					player.curMana -= 30;
					ccurTime[2] = classicTime[2];
				}
			}
			else
			{
				ccurTime[2] -= Time.deltaTime;
			}
		}
	}
	void GroundAttack()
	{
		if (StageCheck[2])
		{
			if (ccurTime[3] <= 0) // �� ����
			{
				if (Input.GetButtonDown("GroundAttack") && player.curMana > 0) // "X" Attack (ice)
				{
					GroudAttackShoot();
					if (binCk == true)
					{
						anim.SetTrigger("isSkill");
						PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
						playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
						SillIcon[2] = true;
						player.curMana -= 50;
						ccurTime[3] = classicTime[3];
						binCk = false;
					}

				}
			}
			else
			{
				ccurTime[3] -= Time.deltaTime;
			}
		}

	}
	void ThunderAttack()
	{
		if (StageCheck[3])
		{
			if (ccurTime[4] <= 0) // ���� ����
			{
				if (Input.GetButtonDown("ThunderAttack") && player.curMana > 0) // "V" Attack (ice)
				{
					ThunderShoot();
					if (binCk == true)
					{
						anim.SetTrigger("isSkill");
						PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
						playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
						SillIcon[3] = true;
						player.curMana -= 40;
						ccurTime[4] = classicTime[4];
						binCk = false;
					}
				}
			}
			else
			{
				ccurTime[4] -= Time.deltaTime;
			}
		}

	}


	// bullet ����
	void Shoot()
	{
		Bullet Pos = GetComponent<Bullet>();
		GameObject ShootAtk = Instantiate(impactEffect[0], firePoint.position, firePoint.rotation);
		AttackArea area = ShootAtk.GetComponent<AttackArea>();

		if (area != null)
		{
			area.damage = 20 + UpAtk;
			area.isEnemyAttack = false;
		}
	}

	void FireShoot()
	{
		GameObject FireAtk = Instantiate(impactEffect[1], firePoint.position, firePoint.rotation);
		AttackArea area = FireAtk.GetComponent<AttackArea>();

		if (area != null)
		{
			area.damage = 50 + UpAtk;
			area.isEnemyAttack = false;
			area.AttackType = "Fire";
			area.dotDamage = 5;
			area.duration = 5f;
		}
	}

	void IceShoot()
	{
		GameObject IceAtk = Instantiate(impactEffect[2], firePoint.position, firePoint.rotation);
		AttackArea area = IceAtk.GetComponent<AttackArea>();

		if (area != null)
		{
			area.damage = 30 + UpAtk;
			area.isEnemyAttack = false;
			area.AttackType = "Ice";
			area.speedModify = 0.5f;
			area.duration = 4f;
		}
	}

	// ���� �ȿ� ������ ���� Ű�� ������ �� ����
	void GroudAttackShoot()
	{
		Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + new Vector3(7.5f * moveDic, 0, 0), new Vector2(13, 1f), 0, InLayer);
		foreach (Collider2D i in hit)
		{
			GameObject GroundAtk = Instantiate(impactEffect[3], i.transform.position, Quaternion.identity);
			Instantiate(impactEffect[4], Vector3.up * 1.4f + i.transform.position, Quaternion.identity);
			binCk = true;
			AttackArea area = GroundAtk.GetComponent<AttackArea>();

			if (area != null)
			{
				area.damage = 70 + UpAtk;
				area.AttackType = "Earth";
				area.isEnemyAttack = false;
			}
			break;

		}

	}


	// ���� �ȿ� ������ ���� Ű�� ������ ���� ����
	void ThunderShoot()
	{
		Collider2D[] Thit = Physics2D.OverlapBoxAll(transform.position + new Vector3(7.5f * moveDic, 3.5f, 0), new Vector2(13, 10f), 0, InLayer);
		foreach (Collider2D i in Thit)
		{
			GameObject thunderAtk = Instantiate(impactEffect[5], i.transform.position, Quaternion.identity);
			Instantiate(impactEffect[6], Vector3.up * 4.9f + i.transform.position, Quaternion.identity);
			binCk = true;
			AttackArea area = thunderAtk.GetComponent<AttackArea>();

			if (area != null)
			{
				area.damage = 40 + UpAtk;
				area.isEnemyAttack = false;
				area.AttackType = "Stun";
				area.duration = 2f;
			}
		}
	}

	// isAttack �ִϸ��̼��� ���� �� �̵� Ȱ��ȭ
	public void OnCanMonve()
	{
		PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
		playerMovement.bCanMove = true;
	}
	// ��ų ������ �����ִ� �뵵
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position + new Vector3(7.5f * moveDic, 0, 0), size);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(transform.position + new Vector3(7.5f * moveDic, 3.5f, 0), size2);
	}
}
