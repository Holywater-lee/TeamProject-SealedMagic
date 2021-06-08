using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
�ۼ�: 20181220 �̼���(P)

����: ���� ������Ʈ�� ���Ǵ� ��ũ��Ʈ
��ȣ�ۿ��ϸ� �������� �� ���� ���� ȹ�� �� �������� �̵�
�ش� ��ũ��Ʈ�� �߰��� �� �±׸� Altar�� ������ ��
*/

public class Altar : MonoBehaviour
{
	PlayerObject player;

	//[Tooltip("�������� �Ϸ�� ������ ����Ʈ ������")]
	//public GameObject EndEffect;

	[Tooltip("���� �� ������ ����Ʈ ������")]
	[SerializeField] GameObject concentrateFX;

	[Tooltip("���� ������ �Ѿ�� �����ð�")]
	[SerializeField] float sceneLoadTime = 1f;
	
	// ��ȣ�ۿ� ���ΰ� ����
	[HideInInspector] public bool isInteracting = false;

	[Tooltip("�������� �ð�")]
	[SerializeField] float CompleteTime = 3f;
	[Tooltip("���� �� �̸��� ���� ��")]
	[SerializeField] string nextScene = "";

	float delay = 0f;

	void Start()
	{
		player = FindObjectOfType<PlayerObject>();
	}
	void Update()
	{
		if (isInteracting)
			Interact();
	}
	void Interact()
	{
		concentrateFX.SetActive(true);
		delay += Time.deltaTime;

		// �÷��̾� ��ġ�� ���ϸ� �������� ����
		if (player.transform.position != PlayerObject.Altarpos)
		{
			delay = 0f;
			isInteracting = false;
			concentrateFX.SetActive(false);
		}

		if (delay >= CompleteTime)
		{
			delay = 0f;
			isInteracting = false;
			concentrateFX.SetActive(false);
			Debug.Log("���� ���� ����!");

			GameManager.instance.killedColoredMonster = 0;

			PlayerLongAttack.instance.UpAtk += 50f;
			PlayerLongAttack.instance.Atk += 30f;
			player.maxHealth += 200f;
			player.maxMana += 200f;
			player.curHealth += 300f;
			player.curMana += player.maxMana / 2f;

			StartCoroutine("GoNextLevel");
		}
	}

	IEnumerator GoNextLevel()
	{
		GameManager.instance.plMaxHP = player.maxHealth;
		GameManager.instance.plCurHP = player.curHealth;
		GameManager.instance.plMaxMP = player.maxMana;
		GameManager.instance.plCurMP = player.curMana;

		for (int i = 0; i <= 3; i++)
		{
			GameManager.instance.magicCheck[i] = PlayerLongAttack.instance.StageCheck[i];
		}
		GameManager.instance.increasedAtk = PlayerLongAttack.instance.UpAtk;
		GameManager.instance.increasedNormalAtk = PlayerLongAttack.instance.Atk;
		GameManager.instance.Stagenum++;
		yield return new WaitForSeconds(sceneLoadTime);
		SceneManager.LoadScene(nextScene);
	}
	/*
	public IEnumerator Interact()
	{
		alteranim.SetBool("isInteract", true);
		Debug.Log("���� �ڷ�ƾ ����");
		
		float delay = 0;
		while(delay <= CompleteTime)
		{
			delay += Time.deltaTime;
			Debug.Log("������: " + delay);
			if (player.transform.position != PlayerInteract.pos)
			{
				isInteracting = false;
				alteranim.SetBool("isInteract", false);
				StopCoroutine(intract);
				Debug.Log("���� �ڷ�ƾ ���");
			}

			yield return null;
		}

		yield return null;
		Debug.Log("���� �ڷ�ƾ ����");
		//GameObject Eff = Instantiate(EndEffect, transform);
		//Destroy(Eff, 2f);
		//gameManager.killedColoredMonster = 0;

		//player.attackDamage += 50f;
		//player.magicDamage += 50f;
		player.maxHealth += 200f;
		player.maxMana += 200f;
		player.curHealth += 300f;
		player.curMana += player.maxMana / 2f;

		isInteracting = false;
		alteranim.SetBool("isInteract", false);
		StartCoroutine("GoNextLevel");
	}

	void GoNextLevel()
	{
		CTime += Time.deltaTime;

		if (CTime >= sceneLoadTime)
		{
			CTime = 0f;
			concentrated = false;
			SceneManager.LoadScene(nextScene);
		}
	}
	*/
}
