using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("���󰡴� �ӵ�")]
    public float speed = 20f;

    public Rigidbody2D rigid;
 

    void Start()
    {
        rigid.velocity = transform.right * speed;// ���󰡴� �ӵ�(��)
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GroundBullet")
            Destroy(gameObject, 0.01f);
        else
            Destroy(gameObject, 1);// ��򰡿� �ε�ġ�� �ı�
    }
}
