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
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
