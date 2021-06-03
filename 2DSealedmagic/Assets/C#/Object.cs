using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ������Ʈ (ȭ�� �߻��, ��, ���� ��) ��ũ��Ʈ
�ʿ����� ���� ������ �ۼ����� �ʴ´�.

���μ��� index: ��� �����ϰ� �� ������ �ε��� ��ȣ �ۼ� (0~3)

-- �ʿ��� ���� --
ArrowDispenser: gameObj, Direction
Arrow: amount
Plate: gameObj
Door: null
SealedStone: HP, index
PotionHP/PotionMP: gameObj(FX), amount

-- ObjectType�� ������Ʈ ������ ���� --
ȭ��߻��: ArrowDispenser
ȭ��: Arrow
����: Plate
��: Door
���μ�: SealedStone
����: PotionHP, PotionMP
*/

public class Object : MonoBehaviour
{
	[Tooltip("������Ʈ Ÿ���� �Է�")]
	[SerializeField] string ObjectType;
	[Tooltip("���⿡ ���� Right, Left �Է� / ���ʿ�� �ۼ� ���ص� ��")]
	[SerializeField] public string Direction;
	[Tooltip("������Ʈ Ÿ�Կ� ���� ������Ʈ�� ���� ��")]
	[SerializeField] GameObject gameObj;
	[Tooltip("������Ʈ�� ���ط�, ȸ���� ���� ��ġ")]
	[SerializeField] float amount;
	[Tooltip("������Ʈ �ӵ�")]
	[SerializeField] float speed;
	[Tooltip("������Ʈ ü�� (SealedStone)")]
	[SerializeField] float HP;
	[Tooltip("�ε��� ��ȣ")]
	[SerializeField] int index;

	bool onTrigger = false;
	float floatingDir = 0f;

	Vector2 pos;

	void Start()
	{
		if (ObjectType == "ArrowDispenser")
			StartCoroutine(ArrowDispenser(Direction));
		if (ObjectType == "Arrow")
			StartCoroutine("Arrow");
		if (ObjectType == "PotionHp" || ObjectType == "PotionMP")
		{
			pos = transform.position;
			StartCoroutine("SetFloatingDir");
		}
	}

	IEnumerator ArrowDispenser(string Dir)
	{
		while(true)
		{
			yield return new WaitForSeconds(2f);
			GameObject arrow = Instantiate(gameObj, transform.position, Quaternion.identity);
			if (Dir == "Right") arrow.transform.localScale = new Vector3(1, 1, 1);
			else if (Dir == "Left") arrow.transform.localScale = new Vector3(-1, 1, 1);

			if (arrow != null)
				Destroy(arrow, 2f);
		}
	}

	IEnumerator Arrow()
	{
		int dir = (int)transform.localScale.x;
		AttackArea atkArea = GetComponent<AttackArea>();
		atkArea.isEnemyAttack = true;
		atkArea.damage = amount;
		while (true)
		{
			transform.Translate(dir * speed * Time.deltaTime, 0, 0);
			yield return null;
		}
	}
	
	IEnumerator SetFloatingDir()
	{
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(Floater(0.3f));
		while (true)
		{
			floatingDir = 1f;
			yield return new WaitForSeconds(1f);
			floatingDir = -1f;
			yield return new WaitForSeconds(1f);
		}
	}

	IEnumerator Floater(float speed)
	{
		while (true)
		{
			if (floatingDir == 1f)
				transform.position = Vector2.Lerp(transform.position, pos + Vector2.up * 1f, Time.deltaTime * speed);

			if (floatingDir == -1f)
				transform.position = Vector2.Lerp(transform.position, pos, Time.deltaTime * speed);

			yield return null;
		}
	}

	/*
	 * �ش� �κ��� HP�� 3�̰� 1�� �����ϴ� ������ ��ȹ�Ǿ� ���ŵ�
	 * ��ü ��ũ��Ʈ�� OnTriggerEnter2D�� �ۼ���
	public void onAttack(float dmg)
	{
		HP -= dmg;
		if (HP <= 0)
		{
			//Player.CanUseMagic[index] = true;
			//(�ı� ����Ʈ ���� ��ũ��Ʈ �߰�)

			Destroy(gameObject);
		}
	}
	*/

	void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerObject player = collision.GetComponent<PlayerObject>();
		if (ObjectType == "Plate" && collision.gameObject.tag == "Player" && !onTrigger)
		{
			onTrigger = true;
			gameObj.SetActive(false);
		}

		if (ObjectType == "Arrow")
		{
			if (collision.gameObject.tag == "Player")
			{
				player.OnDamage(amount); // Ʈ�� ������
				Destroy(gameObject);
			}
			
			if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Floor")
			{
				Destroy(gameObject);
			}
		}

		if (ObjectType == "SealedStone")
		{
			if (collision.gameObject.tag == "Attack")
			{
				HP -= 1;
				if (HP <= 0)
				{
					//player.CanUseMagic(index);
					// (�ִٸ�)(�ı� ����Ʈ ���� ��ũ��Ʈ)

					Destroy(gameObject);
				}
			}
		}

		if (ObjectType == "PotionHp" || ObjectType == "PotionMP")
		{
			if(collision.gameObject.tag == "Player")
			{
				PlayerObject pl = FindObjectOfType<PlayerObject>();
				
				if(ObjectType == "PotionHp")
				{
					pl.curHealth += amount;
					if (pl.curHealth >= pl.maxHealth)
						pl.curHealth = pl.maxHealth;
				}
				else if(ObjectType == "PotionMP")
				{
					pl.curMana += amount;
					if (pl.curMana >= pl.maxMana)
						pl.curMana = pl.maxMana;
				}

				//GameObject Pfx = Instantiate(gameObj, pl.transform);
				//Destroy(Pfx, 1f);

				Destroy(gameObject);
			}
		}
	}
}
