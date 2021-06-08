using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���� ��ų ��ũ��Ʈ
�ε����� ���� ��ų�� �ٸ��� ����
UseSkill(��ų �ε���, ���� ����, ��ų ��ȣ(1,2));
��ų �ε����� �޸��忡 ���� ����
*/

public class MonsterSkill : MonoBehaviour
{
	MobSkillsInfo skInfo;

	bool canUse_1 = true;
	bool canUse_2 = true;

	private void Awake()
	{
		skInfo = FindObjectOfType<MobSkillsInfo>();
	}

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
		DeactiveSkill(skNum);

		if (index == 0)
		{
			Vector2 Pos = new Vector2(atkDir * skInfo.range[index] + transform.position.x, transform.position.y);

			GameObject atkSp = Instantiate(skInfo.FX_Sprite[index], Pos + new Vector2(0, 2f), Quaternion.identity);
			yield return new WaitForSeconds(0.1f);

			atkSp.transform.localPosition = Pos;
			GameObject atkArea = Instantiate(skInfo.Skill_Area[index], Pos, Quaternion.identity);
			GameObject atkFX = Instantiate(skInfo.FX_Particle[index], Pos, Quaternion.identity);
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

			StartCoroutine(Cooldown(skInfo.cooldown[index], skNum));
		}
		else if (index == 1)
		{
			GetComponent<Monster>().OnProtected = true;
			GameObject atkFX = Instantiate(skInfo.FX_Particle[index], transform.position, Quaternion.identity);
			Destroy(atkFX, 1f);
			StartCoroutine(Cooldown(skInfo.cooldown[index], skNum));
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
