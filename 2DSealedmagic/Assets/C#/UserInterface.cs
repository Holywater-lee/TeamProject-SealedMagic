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
	[Tooltip("�޴� �İ� �̹��� ������Ʈ")]
	[SerializeField] GameObject backImage;
	[Tooltip("���ӿ��� UI")]
	[SerializeField] GameObject gameOverUI;
	[Tooltip("����Ŭ���� UI")]
	[SerializeField] GameObject gameClearUI;
	[Tooltip("UI ĵ���� ������Ʈ")]
	[SerializeField] GameObject UICanvas;
	public Text message;
	public Text message2;
	public float Hp;
	public float Mana;

	Animator anim;
	PlayerObject player;

	public static UserInterface instance;

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
		anim = UICanvas.GetComponent<Animator>();
		instance = this;
	}

	void Update()
	{
		healthBar.fillAmount = player.curHealth / player.maxHealth;
		manaBar.fillAmount = player.curMana / player.maxMana;
		if (Input.GetButtonDown("Cancel") && !GameManager.instance.isGameover && !GameManager.instance.isGameClear)
		{
			if (MenuUI.activeSelf)
			{
				MenuUI.SetActive(false);
				backImage.SetActive(false);
			}
			else
			{
				MenuUI.SetActive(true);
				backImage.SetActive(true);
			}
		}

		Init_HP();
		Init_MANA();
		message.text = (Hp).ToString();
		message2.text = (Mana).ToString();
	}

	public void GameOver()
	{
		GameManager.instance.isGameover = true;
		MenuUI.SetActive(false);
		gameOverUI.SetActive(true);
		backImage.SetActive(false);
		anim.SetBool("isGameover", true);
	}

	public void GameClear()
	{
		GameManager.instance.isGameClear = true;
		MenuUI.SetActive(false);
		gameClearUI.SetActive(true);
		backImage.SetActive(true);
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
