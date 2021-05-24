using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���� ������Ʈ ����
ȸ�������� ��� tag�� HealPotion
����ȸ�� ������ ��� tag�� ManaPotion���� ���� �� ����� ��
*/

public class Potion : MonoBehaviour
{
	[Tooltip("���� ȹ��� ����� ����Ʈ ������")]
	[SerializeField] GameObject potionFx;
	[Tooltip("ȸ����")]
	[SerializeField] float recoveryAmount;
	[Tooltip("��������: HealPotion / ManaPotion")]
	[SerializeField] string potionKinds;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player" && potionKinds == "HealPotion")
		{
			PlayerObject player = FindObjectOfType<PlayerObject>();
			player.curHealth += recoveryAmount;
			//GameObject hfx = Instantiate(potionFx, player.transform);
			//Destroy(hfx, 1f);
			Destroy(gameObject);
		}
		if (collision.tag == "Player" && potionKinds == "ManaPotion")
		{
			PlayerObject player = FindObjectOfType<PlayerObject>();
			player.curMana += recoveryAmount;
			//GameObject mfx = Instantiate(potionFx, player.transform);
			//Destroy(mfx, 1f);
			Destroy(gameObject);
		}
	}
}
