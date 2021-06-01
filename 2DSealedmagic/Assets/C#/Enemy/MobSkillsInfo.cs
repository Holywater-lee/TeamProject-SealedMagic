using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼��� (P)
�ۼ� �ǵ�:
	MonsterSkill���� �ε����� ���� ��ų�� ������ �޾ƿ����� �Ͽ���.
	������ MonsterSkill�� �ϰ������� ����Ϸ� �Ͽ����� ������ �����
	���ҽ� ���� ���� �� ���Ƽ� ���⿡ ����� �ξ���.
����:
	���� ��ų���� ������ ��� ��ũ��Ʈ, ���ӸŴ��� ������Ʈ�� ����.
*/

public class MobSkillsInfo : MonoBehaviour
{
	[Tooltip("������ ��������Ʈ")]
	public GameObject[] FX_Sprite = new GameObject[3];
	[Tooltip("���� ���� ������Ʈ ������")]
	public GameObject[] Skill_Area = new GameObject[3];
	[Tooltip("���� ����Ʈ ��ƼŬ")]
	public GameObject[] FX_Particle = new GameObject[3];

	[Tooltip("���� ���ð�")]
	public float[] cooldown = new float[3];
	[Tooltip("��Ÿ�")]
	public float[] range = new float[3];
}
