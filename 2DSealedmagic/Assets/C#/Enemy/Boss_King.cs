using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼��� (P)

����:
	���� ���� �� �ϳ��� 'ŷ'
    �ɷ��� ����ϸ� ���ݷ��� �����ϰ� ���ݹ����� �þ��.
*/

public class Boss_King : BossMonster
{
	[Header("���� ����: King")]
	[Tooltip("�ɷ� ���� ���ð� (0.5�� ����)")]
	public float abilityCooldown;
	[Tooltip("�ɷ� ���ӽð�")]
	public float abilityDuration;

	// �ν����ͺ� Ȯ�ο�, ���߿� public�������
	public bool onAbility = false;

	float abCooldown = 0f;

	// ���� ��ũ��Ʈ ������
	protected override void TryAttack()
	{
		if (abCooldown <= 0f)
		{
			UseAbility();
			StartCoroutine(CooldownAbility(abilityCooldown));
		}
		else
		{
			
		}
	}

	void UseAbility()
	{
		
		anim.SetBool("onAbility", true);
	}

	IEnumerator CooldownAbility(float cooldown)
	{
		abCooldown = cooldown;

		while (abCooldown > 0)
		{
			abCooldown -= 0.5f;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
