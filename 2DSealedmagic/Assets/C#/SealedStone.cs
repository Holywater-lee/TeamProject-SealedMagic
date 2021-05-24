using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���μ��� �ν����� �÷��̾ magicNum��° ���� ��������� �ر�
����ν�� �������ݿ��� �ν����� ������
�ڡڡڡ� ���� ������ ��ũ��Ʈ �ڡڡڡ�
*/
// �Ʒ� //ó���Ȱ� �����

public class SealedStone : MonoBehaviour
{
	[Tooltip("���μ��� ü��")]
	[SerializeField] float HP;
	[Tooltip("�ν����� Ȱ��ȭ �� ������ ��ȣ (0~3)")]
	[SerializeField] int magicNum;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Bullet")
		{
			PlayerObject player = FindObjectOfType<PlayerObject>();
			//HP -= player.attackDamage;
			if (HP <= 0)
			{
				//player.canUseMagic[magicNum] = true;
				Destroy(gameObject);
			}
		}
	}
}
