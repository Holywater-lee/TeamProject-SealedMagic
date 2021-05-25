using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDmg : MonoBehaviour
{
    [Tooltip("���� ���� ���ο�")]
    public float SlowSpeed = 0.5f;
    [Tooltip("���� ���� ���ο� ���ӽð�")]
    public float SlowDuration = 3f;
    [Tooltip("���� ��ø Ƚ��")]
    [SerializeField] private int count = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (count == 1)
        {
            if (collision.gameObject.tag == "IceBullet")
            {
                GetComponent<Monster>().Slowspeed(SlowSpeed, SlowDuration);
                count--;
            }

        }
    }
}
