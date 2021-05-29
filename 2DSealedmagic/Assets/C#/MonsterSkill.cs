using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���� ��ų ��ũ��Ʈ
*/

public class MonsterSkill : MonoBehaviour
{
	public GameObject FX_Sprite;
	public GameObject Skill_Area;
	public GameObject FX_Particle;

	public float cooldown;
	public float range;

	bool canUse = true;

	public void UseSkill(int index, int atkDir)
	{
		if (canUse)
		{
			Debug.Log("���ų");
			StartCoroutine(Skill(index, atkDir));
		}
	}

	IEnumerator Skill(int index, int atkDir)
	{
		Debug.Log("�ڷ�ƾ����");

		if (index == 0)
		{
			Debug.Log("�ε����� ����");
			canUse = false;

			Vector2 Pos = new Vector2(atkDir * range + transform.position.x, transform.position.y);

			GameObject atkFX = Instantiate(FX_Sprite, Pos, Quaternion.identity);
			GameObject atkArea = Instantiate(Skill_Area, Pos, Quaternion.identity);
			var area = atkArea.GetComponent<AttackArea>();
			Debug.Log("�����ؾ�");
			if (area != null)
			{
				area.isEnemyAttack = true;
				area.damage = 300;
				Debug.Log("������");
			}

			yield return new WaitForSeconds(0.1f);
			Destroy(atkArea);
			Debug.Log("��������");

			yield return new WaitForSeconds(1f);
			Destroy(atkFX);
			Debug.Log("��������");

			StartCoroutine(Cooldown(cooldown));
		}

	}

	IEnumerator Cooldown(float time)
	{
		Debug.Log("�����");
		yield return new WaitForSeconds(time);
		canUse = true;
		Monster mob = GetComponent<Monster>();
		mob.CanSkill = true;
		Debug.Log("��");
	}
}
