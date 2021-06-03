using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
�ۼ�: 20181220 �̼���(P)

����: �����÷��� UI
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
	[Tooltip("ü�¹� ��ġ")]
	public Text message;
	[Tooltip("������ ��ġ")]
	public Text message2;
	[Tooltip("�޾ƿ��� ü��")]
	public float Hp;
	[Tooltip("�޾ƿ��� ����")]
	public float Mana;



	PlayerObject player;

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
		
	}

	void Update()
	{
		//healthBar.value = player.curHealth / player.maxHealth;
		//manaBar.value = player.curMana / player.maxMana;
		healthBar.fillAmount = player.curHealth / player.maxHealth;
		manaBar.fillAmount = player.curMana / player.maxMana;
		if (Input.GetButtonDown("Cancel"))
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
