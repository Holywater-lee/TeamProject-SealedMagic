using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���� ��ų ��ũ��Ʈ
�ε����� ���� ��ų�� �ٸ��� ����
UseSkill(��ų �ε���, ���� ����, ��ų ��ȣ(1,2));
*/

public class MonsterSkill : MonoBehaviour
{
	[Header("�ִ� 2��������")]
	[Tooltip("������ ��������Ʈ")]
	[SerializeField] GameObject[] FX_Sprite = new GameObject[2];
	[Tooltip("���� ���� ������Ʈ ������")]
	[SerializeField] GameObject[] Skill_Area = new GameObject[2];
	[Tooltip("���� ����Ʈ ��ƼŬ")]
	[SerializeField] GameObject[] FX_Particle = new GameObject[2];

	[Tooltip("���� ���ð�")]
	[SerializeField] float[] cooldown = new float[2];
	[Tooltip("��Ÿ�")]
	[SerializeField] float[] range = new float[2];

	bool canUse_1 = true;
	bool canUse_2 = true;

	public void UseSkill(int index, int atkDir, int skNum)
	{
		if (canUse_1 && skNum == 1)
		{
			StartCoroutine(Skill(index, atkDir, 1));
		}
		else if (canUse_2 && skNum == 2)
		{
			StartCoroutine(Skill(index, atkDir, 2));
		}
	}

	void DeactiveSkill(int skNum)
	{
		if (skNum == 1) canUse_1 = false;
		else if (skNum == 2) canUse_2 = false;
	}

	IEnumerator Skill(int index, int atkDir, int skNum)
	{
		int ArrayNum = skNum - 1;
		if (index == 0)
		{
			DeactiveSkill(skNum);
			//Debug.Log("skill_index_0");
			Vector2 Pos = new Vector2(atkDir * range[ArrayNum] + transform.position.x, transform.position.y);

			GameObject atkSp = Instantiate(FX_Sprite[ArrayNum], Pos + new Vector2(0, 2f), Quaternion.identity);
			yield return new WaitForSeconds(1.1f);

			atkSp.transform.localPosition = Pos;
			GameObject atkArea = Instantiate(Skill_Area[ArrayNum], Pos, Quaternion.identity);
			GameObject atkFX = Instantiate(FX_Particle[ArrayNum], Pos, Quaternion.identity);
			var area = atkArea.GetComponent<AttackArea>();

			if (area != null)
			{
				area.isEnemyAttack = true;
				area.damage = 300;
			}

			yield return new WaitForSeconds(0.1f);
			Destroy(atkArea);

			yield return new WaitForSeconds(1f);
			Destroy(atkSp);
			Destroy(atkFX);

			StartCoroutine(Cooldown(cooldown[ArrayNum], skNum));
		}
		else if (index == 1)
		{
			DeactiveSkill(skNum);
			Debug.Log("skill_index_1");
			GetComponent<Monster>().OnProtected = true;
			GameObject atkFX = Instantiate(FX_Particle[ArrayNum], transform.position, Quaternion.identity);
			Destroy(atkFX, 1f);
			StartCoroutine(Cooldown(cooldown[ArrayNum], skNum));
		}
	}

	IEnumerator Cooldown(float time, int skNum)
	{
		yield return new WaitForSeconds(time);

		if (skNum == 1)
		{
			canUse_1 = true;
			GetComponent<Monster>().canSkill_1 = true;
		}
		else if (skNum == 2)
		{
			canUse_2 = true;
			GetComponent<Monster>().canSkill_2 = true;
		}
	}
}
