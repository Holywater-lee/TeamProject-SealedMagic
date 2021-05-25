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
		�������� ������������ ������
		isTrigger�� �� üũ�ϱ� �ٶ�
*/

public class AttackArea : MonoBehaviour
{
	// true�� ��� ���� ����, false�� ��� �÷��̾��� ����
	[HideInInspector] public bool isEnemyAttack;
	// ���� �Լ��κ��� �������� ������
	[HideInInspector] public float damage;

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
					mob.onAttack(damage);

					Destroy(gameObject);
				}
			}
		}

		// ������ ������ ���
		if (isEnemyAttack)
		{
			if (collision.gameObject.tag == "Player")
			{
				PlayerObject pl = collision.GetComponent<PlayerObject>();

				if (pl != null)
				{
					pl.OnDamage(damage);

					Destroy(gameObject);
				}
			}
		}
	}
}
