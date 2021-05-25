using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDmg : MonoBehaviour
{
    [Tooltip("�� ���� ������")]
    private int dot = 5;
    [Tooltip("�� ��ø Ƚ��")]
    [SerializeField] private int count = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "FireBullet")
        {
            if (count == 1)
            {
                StartCoroutine(DotDam());
                count--;
            }
        }

    }

    // �ڷ�ƾ�� ���� ��� �� �������� ���ӵȴ�(���� �ð��� �� ������)
    IEnumerator DotDam()
    {
        while (true)
        {
            GetComponent<Monster>().onAttack(dot);
            yield return new WaitForSeconds(1);
        }
    }
}
