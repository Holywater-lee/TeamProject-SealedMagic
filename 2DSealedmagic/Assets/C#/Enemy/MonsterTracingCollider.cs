using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���Ͱ� �Ѿư� ������ ������ ������Ʈ�� ���� ��ũ��Ʈ
���� ��ũ��Ʈ�� �θ�� �Ѵ�.
���� �ȿ� ���Դٸ� 1�� �� �÷��̾ �Ѵ´�.
�� �� ��ũ��Ʈ�� �ִ� ������Ʈ�� ���̾ �ݵ�� PlayerCheckRange�� ������ �� ��
�� plz change layer --> "PlayerCheckRange" when this script added ��
(�ѱ۷� ���� �̸����� â�� ??�� ������ ����� ��)
*/

public class MonsterTracingCollider : MonoBehaviour
{
	[Tooltip("�θ� ������Ʈ")]
	public Monster monster;
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			monster.traceTarget = collision.gameObject;
			monster.CancelInvoke();
		}
	}

	void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			monster.isTracing = true;
		}
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			monster.isTracing = false;
			monster.CancelInvoke();
			monster.Invoke("Think", 0.2f);
		}
	}

}
