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
	Animator alteranim;
	GameManager gameManager;
	PlayerLongAttack playerlonAtk;

	//[Tooltip("�������� �Ϸ�� ������ ����Ʈ ������")]
	//public GameObject EndEffect;

	[Tooltip("���� ������ �Ѿ�� �����ð�")]
	[SerializeField] float sceneLoadTime = 2f;
	
	// ��ȣ�ۿ� ���ΰ� ����
	[HideInInspector] public bool isInteracting = false;

	[Tooltip("�������� �ð�")]
	[SerializeField] float CompleteTime = 3f;
	[Tooltip("���� �� �̸��� ���� ��")]
	[SerializeField] string nextScene;

	float delay = 0f;


	void Start()
	{
		//player = GetComponent<PlayerObject>();
		player = FindObjectOfType<PlayerObject>();
		alteranim = GetComponent<Animator>();
		gameManager = FindObjectOfType<GameManager>();
		playerlonAtk = FindObjectOfType<PlayerLongAttack>();
	}
	void Update()
	{
		if (isInteracting)
			Interact();
	}
	void Interact()
	{
		alteranim.SetBool("isInteract", true);
		delay += Time.deltaTime;

		// �÷��̾� ��ġ�� ���ϸ� �������� ����
		if (player.transform.position != PlayerObject.Altarpos)
		{
			delay = 0f;
			isInteracting = false;
			alteranim.SetBool("isInteract", false);
		}

		if (delay >= CompleteTime)
		{
			delay = 0f;
			alteranim.SetBool("isInteract", false);
			isInteracting = false;
			Debug.Log("���� ���� ����!");
			//GameObject Eff = Instantiate(EndEffect, transform);
			//Destroy(Eff, 2f);
			gameManager.killedColoredMonster = 0;

			playerlonAtk.UpAtk += 50f;
			player.maxHealth += 200f;
			player.maxMana += 200f;
			player.curHealth += 300f;
			player.curMana += player.maxMana / 2f;

			StartCoroutine("GoNextLevel");
		}
	}

	IEnumerator GoNextLevel()
	{
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
