using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ����׿� ��ũ��Ʈ, ���ӸŴ����� ���� �� Ȱ��/��Ȱ��
/*
h��ư: �������� 1�� �̵�
j��ư: �������� 2�� �̵�
k��ư: �������� 3���� �̵�
b��ư: �÷��̾ �ſ� ưư����
n��ư: �÷��̾ �ſ� ����
m��ư: �÷��̾� ��� ���� �ر�
*/

public class test : MonoBehaviour
{
	[Header("����׿� ��ũ��Ʈ")]
	[Header("H: �������� 1�� �̵�")]
	[Header("J: �������� 2�� �̵�")]
	[Header("K: �������� 3���� �̵�")]
	[Header("B: �÷��̾ �ſ� ưư����")]
	[Header("N: �÷��̾ �ſ� ����")]
	[Header("M: �÷��̾� ��� ���� �ر�")]

	[SerializeField] bool isDebug;
	void Update()
	{
		if (isDebug)
		{
			if (Input.GetKeyDown(KeyCode.H))
			{
				SceneManager.LoadScene("Stage1");
			}
			else if (Input.GetKeyDown(KeyCode.J))
			{
				SceneManager.LoadScene("Stage2");
			}
			else if (Input.GetKeyDown(KeyCode.K))
			{
				SceneManager.LoadScene("Stage3");
			}
			else if (Input.GetKeyDown(KeyCode.B))
			{
				GameManager.instance.plMaxHP += 10000;
				GameManager.instance.plCurHP += 10000;
				GameManager.instance.plMaxMP += 10000;
				GameManager.instance.plCurMP += 10000;

				GameManager.instance.FindAndGetInfo();
			}
			else if (Input.GetKeyDown(KeyCode.N))
			{
				GameManager.instance.increasedAtk += 1000;
				GameManager.instance.increasedNormalAtk += 200;
				GameManager.instance.FindAndGetInfo();
			}
			else if (Input.GetKeyDown(KeyCode.M))
			{
				for (int i = 0; i <= 3; i++)
				{
					GameManager.instance.magicCheck[i] = true;
				}
				GameManager.instance.FindAndGetInfo();
			}
		}
	}
}
