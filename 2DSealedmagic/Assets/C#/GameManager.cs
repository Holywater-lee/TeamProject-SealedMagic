using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
�ۼ�: 20181220 �̼���(P)

����: ���� �Ŵ��� ��ũ��Ʈ
������ Ȱ��ȭ�ϱ� ���� ���� �� ������
*/

public class GameManager : MonoBehaviour
{
	public int killedColoredMonster = 0;
	public bool isGameover = false;
	public bool isGameClear = false;
	public GameObject ColoredMonsterFX;

	public static GameManager instance;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (isGameover)
			{
				SceneManager.LoadScene("Stage1");
			}
			else if (isGameClear)
			{
				SceneManager.LoadScene("Stage1");
			}
		}
	}
}
