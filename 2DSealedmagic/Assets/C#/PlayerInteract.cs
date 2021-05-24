using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ���ܿ� �ִ� Interact �Լ��� �����ϰ� ���ִ� ������Ʈ
��ȹ�� �� ��ȣ�ۿ� Ű (���� ��� Ű): FŰ (default)
*/

public class PlayerInteract : MonoBehaviour
{
	Altar obj;
	PlayerObject player;
	public static Vector3 pos;

	void Start()
	{
		player = GetComponent<PlayerObject>();
	}

	void Update()
	{
		KeyEnter();
	}

	void KeyEnter()
	{
		GameManager gameManager = FindObjectOfType<GameManager>();
		if (Input.GetKeyDown(KeyCode.F) && obj != null && gameManager.killedColoredMonster >= 3 && !obj.isInteracting)
		{
			obj.isInteracting = true;
			pos = player.transform.position;
			
			//obj.StartCoroutine(obj.intract);
			//Debug.Log("���� �ڷ�ƾ �õ�");
		}
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Altar")
		{
			obj = FindObjectOfType<Altar>();
		}
	}
	
	void OnTriggerExit2D(Collider2D collision)
	{
		obj = null;
	}
}
