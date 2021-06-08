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
	[SerializeField] float abilityCooldown;
	[Tooltip("�ɷ� ���ӽð�")]
	[SerializeField] float abilityDuration;
	[Tooltip("�ɷ��� �ִ� �߰� ���ݷ�")]
	[SerializeField] float abilityIncreaseDamage;
	[Tooltip("�ɷ��� ������ �� �߰� ��Ÿ�")]
	[SerializeField] float abilityIncreaseRange;

	bool onAbility = false;

	float abCooldown = 0f;
	float atkCooldown = 0f;

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
			if (!isAtk && atkCooldown <= 0f)
			{
				StartCoroutine(Attack());

				StartCoroutine(CooldownAtk(atkDelay));
			}
		}
		isPlayerInAtkRange = false;
	}

	void UseAbility()
	{
		onAbility = true;
		anim.SetBool("onAbility", true);
		Sound("Skill1");
		StartCoroutine(duringAbility(abilityDuration));
	}

	IEnumerator Attack()
	{
		isAtk = true;
		Vector3 newAtkPos = new Vector3(atkDirection * (atkPos.x + additionalRange) + transform.position.x, atkPos.y + transform.position.y, transform.position.z);

		anim.SetBool("isAttack", true);
		Sound("Attack");

		yield return new WaitForSeconds(0.2f);

		GameObject atkArea = Instantiate(atkZone, newAtkPos, Quaternion.identity);
		var area = atkArea.GetComponent<AttackArea>();
		if (area != null)
		{
			area.isEnemyAttack = true;
			area.damage = atkDamage + (onAbility ? abilityIncreaseDamage : 0f);
		}

		GameObject atkFX_S = Instantiate(atkFX_Sprite, newAtkPos, Quaternion.identity);
		GameObject atkFX_P = Instantiate(atkFX_Particle, newAtkPos, Quaternion.identity);
		yield return new WaitForSeconds(0.1f);
		Destroy(atkArea);

		yield return new WaitForSeconds(1f);
		Destroy(atkFX_S);
		Destroy(atkFX_P);

		anim.SetBool("isAttack", false);
		isAtk = false;
	}

	IEnumerator duringAbility(float duration)
	{
		float abDur = duration;
		additionalRange = abilityIncreaseRange;
		while (abDur > 0)
		{
			abDur -= 0.5f;
			yield return new WaitForSeconds(0.5f);
			
			if (abDur <= 0.1f)
			{
				onAbility = false;
				additionalRange = 0f;
				anim.SetBool("onAbility", false);
			}
		}
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

	IEnumerator CooldownAtk(float cooldown)
	{
		atkCooldown = cooldown;

		while (atkCooldown > 0)
		{
			atkCooldown -= 0.5f;
			yield return new WaitForSeconds(0.5f);
		}
	}
}
