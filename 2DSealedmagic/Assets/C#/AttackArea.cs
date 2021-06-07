using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

�ۼ� �ǵ�: �÷��̾�-���� ��ȣ ���� �ǰ� ��ȣ�ۿ���
�ϰ������� ������ �� �ִ� ��ũ��Ʈ�� ������� �Ͽ���. (������ ��ũ��Ʈ?)

����: '���� ����' �� �ִ� ������Ʈ�� ���� ��ũ��Ʈ�� Trigger�� ������ �۵�

����:	�������� ������Ʈ�� AttackArea ����,
		Ÿ ��ũ��Ʈ�� ���ݺκп��� GetComponent<AttackArea>
		�ʼ�: damage, isEnemyAttack ���� ��
		�����̻��� ���: AttackType, dogDamage or speedModify, duration
		isTrigger�� �� üũ�ϱ� �ٶ�
*/

public class AttackArea : MonoBehaviour
{
	//������ �����̻� �����̶�� � ������ ���ԵǴ��� (Fire, Ice, Stun)
	[HideInInspector]public string AttackType;
	// true�� ��� ���� ����, false�� ��� �÷��̾��� ����
	[HideInInspector] public bool isEnemyAttack;
	// ���� �Լ��κ��� �������� ������
	[HideInInspector] public float damage;
	// ��Ʈ ������ ��� ��Ʈ ���ط�
	[HideInInspector] public float dotDamage;
	// �����̻��� �ӵ��� ������ ��� �ӵ� ������
	[HideInInspector] public float speedModify;
	// �����̻��� ���� �ð�
	[HideInInspector] public float duration;

	void OnTriggerEnter2D(Collider2D collision)
	{
		// �÷��̾��� ������ ���
		if (!isEnemyAttack)
		{
			if (collision.gameObject.tag == "Enemy")
			{
				Monster mob = collision.GetComponent<Monster>();

				if (mob != null)
				{
					if (AttackType == "Ice")
					{
						mob.onAttack(damage);
						mob.modifySpeed(speedModify, duration);
					}
					else if (AttackType == "Fire")
					{
						mob.onAttack(damage);
						mob.startDotDamage(dotDamage, duration);
					}
					else if (AttackType == "Stun")
					{
						mob.onAttack(damage);
						mob.startStun(duration);
					}
					else if (AttackType == "Earth")
					{
						mob.power = 15f;
						mob.onAttack(damage);
					}
					else
					{
						mob.onAttack(damage);
					}

					Destroy(gameObject);
				}
				else
				{
					BossMonster boss = collision.GetComponent<BossMonster>();

					if (boss != null)
					{
						boss.onAttack(damage);

						if (AttackType == "Ice")
						{
							boss.modifySpeed(speedModify, duration);
						}
						if (AttackType == "Fire")
						{
							boss.startDotDamage(dotDamage, duration);
						}
						if (AttackType == "Stun")
						{
							boss.startStun(duration);
						}

						Destroy(gameObject);
					}
				}
			}
			else if (collision.gameObject.tag == "SealedStone")
			{
				Object obj = collision.GetComponent<Object>();
				obj.SealedStoneOnAttack();
				Destroy(gameObject);
			}
		}

		// ������ ������ ���
		if (isEnemyAttack)
		{
			if (collision.gameObject.tag == "Player" )
			{
				PlayerObject pl = collision.GetComponent<PlayerObject>();

				if (pl != null)
				{
					pl.OnDamage(damage);

					if (AttackType == "Ice")
					{
						//pl.modifySpeed(speedModify, duration);
					}
					if (AttackType == "Fire")
					{
						//pl.startDotDamage(dotDamage, duration);
					}
					if (AttackType == "Stun")
					{
						//pl.startStun(duration);
					}

					Destroy(gameObject);
				}
			}
		}
	}
}
