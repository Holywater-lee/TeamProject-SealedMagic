using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
�ۼ�: 20181220 �̼���(P)

����: �����÷��� UI
�ٸ� ������Ʈ���� UserInterface.instance.GameOver();�� ȣ�����ָ� ��� ���ӿ����� �ȴ�.
*/

public class UserInterface : MonoBehaviour
{
	[Header("UI")]
	[Tooltip("ü�� �� UI")]
	[SerializeField] Image healthBar;
	[Tooltip("���� �� UI")]
	[SerializeField] Image manaBar;
	[Tooltip("�޴� UI")]
	[SerializeField] GameObject MenuUI;
	[Tooltip("�ɼ� UI")]
	[SerializeField] GameObject OptionUI;
	[Tooltip("������ UI")]
	[SerializeField] GameObject ExitUI;
	[Tooltip("�޴� �İ� �̹��� ������Ʈ")]
	[SerializeField] GameObject backImage;
	[Tooltip("���ӿ��� UI")]
	[SerializeField] GameObject gameOverUI;
	[Tooltip("���ӿ��� �� �̹���")]
	[SerializeField] GameObject gameOverBackImage;
	[Tooltip("����Ŭ���� UI")]
	[SerializeField] GameObject gameClearUI;
	[Tooltip("����ȭ�� UI")]
	[SerializeField] GameObject mainScreen;
	[Tooltip("UI ĵ���� ������Ʈ")]
	[SerializeField] GameObject UICanvas;
	[Tooltip("�ʿ� ������ ���� ����")]
	[SerializeField] Text LightMonster;
	[Tooltip("�ʿ� ������ ���� ����")]
	[SerializeField] Text LightMonKill;

	public Text message;
	public Text message2;
	public float Hp;
	public float Mana;

	public bool isMainScreen = true;

	Animator anim;
	PlayerObject player;
	GameManager game;

	public static UserInterface instance;

    void Awake()
    {
		
    }

    void Start()
	{
		anim = UICanvas.GetComponent<Animator>();
		instance = this;
	}

	public void FindPlayer()
	{
		player = FindObjectOfType<PlayerObject>();
		game = FindObjectOfType<GameManager>();
	}

	void Update()
	{
		if (player != null)
		{
			healthBar.fillAmount = player.curHealth / player.maxHealth;
			manaBar.fillAmount = player.curMana / player.maxMana;


			if (Input.GetButtonDown("Cancel") && !GameManager.instance.isGameover && !GameManager.instance.isGameClear)
			{
				if (backImage.activeSelf)
				{
					MenuUI.SetActive(false);
					OptionUI.SetActive(false);
					ExitUI.SetActive(false);
					backImage.SetActive(false);
				}
				else
				{
					MenuUI.SetActive(true);
					OptionUI.SetActive(false);
					ExitUI.SetActive(false);
					backImage.SetActive(true);
				}
			}

			Init_HP();
			Init_MANA();
			message.text = (Hp).ToString();
			message2.text = (Mana).ToString();
			LightMonKill.text = "  / " + GameManager.instance.killedColoredMonster.ToString();
		}
	}

	public void EnableMainScreen()
	{
		mainScreen.SetActive(true);
	}

	public void DisableMainScreen()
	{
		mainScreen.SetActive(false);
	}

	public void GameOver()
	{
		GameManager.instance.isGameover = true;
		MenuUI.SetActive(false);
		gameOverUI.SetActive(true);
		gameOverBackImage.SetActive(true);
		backImage.SetActive(false);
		SoundManager.instance.SoundOff();

		anim.SetBool("isGameover", true);
	}

	public void InitGameoverBool()
	{
		gameOverUI.SetActive(false);
		gameOverBackImage.SetActive(false);
		anim.SetBool("isGameover", false);
	}

	public void InitGameclearBool()
	{
		gameClearUI.SetActive(false);
		anim.SetBool("isGameClear", false);
	}

	public void GameClear()
	{
		GameManager.instance.isGameClear = true;
		MenuUI.SetActive(false);
		gameClearUI.SetActive(true);
		anim.SetBool("isGameClear", true);
	}

	private void Init_HP()
	{
		Hp = player.curHealth;
		Set_HP();
	}
	private void Init_MANA()
	{
		Mana = player.curMana;
		Set_MANA();
	}

	public void ExitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif

		Application.Quit();
	}


	private void Set_HP()
	{

		if (Hp <= 0)
		{
			Hp = 0;
		}
		else
		{
			if (player.curHealth > player.maxHealth)
			{
				Hp = player.maxHealth;
			}
		}
	}

	private void Set_MANA()
	{

		if (Mana <= 0)
		{
			Mana = 0;
		}
		else
		{
			if (player.curMana > player.maxMana)
			{
				Mana = player.maxMana;
			}
		}
	}

}
