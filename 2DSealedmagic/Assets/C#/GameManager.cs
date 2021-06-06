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

	[SerializeField] public int Stagenum = 0;

	//[SerializeField] GameObject PlayerPrefab;
	//[SerializeField] Vector2[] PlayerSpawnPos;

	[SerializeField] float playerMaxHP;
	[SerializeField] float playerMaxMP;

	[HideInInspector] public float plMaxHP;
	[HideInInspector] public float plCurHP;
	[HideInInspector] public float plMaxMP;
	[HideInInspector] public float plCurMP;
	[HideInInspector] public bool[] magicCheck = new bool[4];
	[HideInInspector] public float increasedAtk;

	PlayerObject player;

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		plMaxHP = 1000;
		plCurHP = 1000;
		plMaxMP = 500;
		plCurMP = 500;

		//SpawnPlayer();
	}

	void Start()
	{
		SceneManager.LoadScene("Stage1");
		player = FindObjectOfType<PlayerObject>();
		GetPlayerInfo();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			if (isGameover)
			{
				InitPlayer();
				SceneManager.LoadScene("Stage1");
			}
			else if (isGameClear)
			{
				InitPlayer();
				SceneManager.LoadScene("Stage1");
			}
		}
	}

	public void FindAndGetInfo()
	{
		player = FindObjectOfType<PlayerObject>();
		GetPlayerInfo();
	}

	void SpawnPlayer()
	{
		//Instantiate(PlayerPrefab, PlayerSpawnPos[Stagenum], Quaternion.identity);
	}

	void GetPlayerInfo()
	{
		if (player != null)
		{
			player.maxHealth = plMaxHP;
			player.curHealth = plCurHP;
			player.maxMana = plMaxMP;
			player.curMana = plCurMP;

			for (int i = 0; i <= 3; i++)
			{
				PlayerLongAttack.instance.StageCheck[i] = magicCheck[i];
			}
			PlayerLongAttack.instance.UpAtk = increasedAtk;
		}
	}

	void InitPlayer()
	{
		if (player != null)
		{
			player.maxHealth = playerMaxHP;
			player.curHealth = playerMaxHP;
			player.maxMana = playerMaxMP;
			player.curMana = playerMaxMP;

			for (int i = 0; i <= 3; i++)
			{
				PlayerLongAttack.instance.StageCheck[i] = false;
			}
			PlayerLongAttack.instance.UpAtk = 0f;
		}
	}
}
