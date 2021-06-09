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

	[SerializeField] AudioClip SFX_Potion;
	[SerializeField] AudioClip SFX_Plate;
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
	[HideInInspector] public float increasedNormalAtk;


	PlayerObject player;
	AudioSource audioSource;

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		audioSource = GetComponent<AudioSource>();
		plMaxHP = 1000;
		plCurHP = 1000;
		plMaxMP = 500;
		plCurMP = 500;
		SceneManager.LoadScene("MainScreen");
	}

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
		GetPlayerInfo();
	}

	void Update()
	{
		if (UserInterface.instance.isMainScreen && Input.anyKey)
		{
			UserInterface.instance.DisableMainScreen();
			UserInterface.instance.isMainScreen = false;
			SceneManager.LoadScene("Stage1");
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			if (isGameover)
			{
				InitPlayer();
				UserInterface.instance.InitGameoverBool();
				isGameover = false;
				SoundManager.instance.SoundOn();

				InitPlayerInfo();

				SceneManager.LoadScene("Stage1");
			}
			else if (isGameClear)
			{
				InitPlayer();
				UserInterface.instance.InitGameclearBool();
				isGameClear = false;
				UserInterface.instance.isMainScreen = true;
				UserInterface.instance.EnableMainScreen();

				InitPlayerInfo();

				SceneManager.LoadScene("MainScreen");
			}
		}
	}

	public void FindAndGetInfo()
	{
		player = FindObjectOfType<PlayerObject>();
		GetPlayerInfo();
	}

	public void Sound(string activity)
	{
		switch (activity)
		{
			case "Potion":
				audioSource.clip = SFX_Potion;
				break;
			case "Plate":
				audioSource.clip = SFX_Plate;
				break;
		}

		if (audioSource != null)
			audioSource.Play();
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
			PlayerLongAttack.instance.Atk = increasedNormalAtk;
		}
	}

	void InitPlayerInfo()
	{
		plMaxHP = playerMaxHP;
		plCurHP = playerMaxHP;
		plMaxMP = playerMaxMP;
		plCurMP = playerMaxMP;

		for (int i = 0; i <= 3; i++)
		{
			magicCheck[i] = false;
		}
		increasedAtk = 0f;
		increasedNormalAtk = 0f;
	}

	void InitPlayer()
	{
		if (player != null)
		{
			player.maxHealth = playerMaxHP;
			player.curHealth = playerMaxHP;
			player.maxMana = playerMaxMP;
			player.curMana = playerMaxMP;
			killedColoredMonster = 0;

			for (int i = 0; i <= 3; i++)
			{
				PlayerLongAttack.instance.StageCheck[i] = false;
			}
			PlayerLongAttack.instance.UpAtk = 0f;
			PlayerLongAttack.instance.Atk = 0f;
		}
	}
}
