using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: UI- �ɼ��� ���� �� - ���� ����
*/

public class SoundManager : MonoBehaviour
{
	[Tooltip("�������")]
	[SerializeField] AudioSource musicSource;
	
	public void SetVolume(float volume)
	{
		musicSource.volume = volume;
	}
}
