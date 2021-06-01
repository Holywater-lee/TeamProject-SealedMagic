using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼��� (P)
����:
	���� ������ �����ؼ� ������� �Ͽ����� �켱������ ���� �̷�
*/

public class BuffBase : MonoBehaviour
{
    public string type;
    public float percentage;
    public float duration;
    public float currentTime;
	WaitForSeconds seconds = new WaitForSeconds(0.1f);

	public void Init(string type, float per, float dur)
	{
		this.type = type;
		percentage = per;
		duration = dur;
		currentTime = duration;
	}

	public void Execute()
	{
		// BossMonster.instance.onBuff.Add(this);
		StartCoroutine(Activation());
	}

	IEnumerator Activation()
	{
		while (currentTime > 0)
		{
			currentTime -= 0.1f;
			yield return seconds;
		}
		currentTime = 0;
		DeActivation();
	}

	public void DeActivation()
	{
		Destroy(gameObject);
	}
}
