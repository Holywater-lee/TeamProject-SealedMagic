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
Arrow: damage
Plate: gameObj
Door: null
SealedStone: HP, index

-- ObjectType�� ������Ʈ ������ ���� --
ȭ��߻��: ArrowDispenser
ȭ��: Arrow
����: Plate
��: Door
���μ�: SealedStone
*/

public class Object : MonoBehaviour
{
	[Tooltip("������Ʈ Ÿ���� �Է�")]
	[SerializeField] string ObjectType;
	[Tooltip("���⿡ ���� Right, Left �Է� / ���ʿ�� �ۼ� ���ص� ��")]
	[SerializeField] public string Direction;
	[Tooltip("������Ʈ Ÿ�Կ� ���� ������Ʈ�� ���� ��")]
	[SerializeField] GameObject gameObj;
	[Tooltip("������Ʈ ���ط�")]
	[SerializeField] float damage;
	[Tooltip("������Ʈ �ӵ�")]
	[SerializeField] float speed;
	[Tooltip("������Ʈ ü�� (SealedStone)")]
	[SerializeField] float HP;
	[Tooltip("�ε��� ��ȣ")]
	[SerializeField] int index;

	bool onTrigger = false;

	void Start()
	{
		if (ObjectType == "ArrowDispenser")
			StartCoroutine(ArrowDispenser(Direction));
		if (ObjectType == "Arrow")
			StartCoroutine("Arrow");
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
		while (true)
		{
			transform.Translate(dir * speed * Time.deltaTime, 0, 0);
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
		if (ObjectType == "Plate" && collision.gameObject.tag == "Player" && !onTrigger)
		{
			onTrigger = true;
			gameObj.SetActive(false);
		}

		if (ObjectType == "Arrow")
		{
			if (collision.gameObject.tag == "Player")
			{
				//player.OnAttack(damage);
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
	}
}
