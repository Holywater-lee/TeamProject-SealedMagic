using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("���󰡴� �ӵ�")]
    public float speed = 20f;

    public GameObject Impact;
    public bool ClassicCheck = false;
    public Rigidbody2D rigid;
    public Transform pos;
 

    void Start()
    {
        rigid.velocity = transform.right * speed;// ���󰡴� �ӵ�(��)
        Invoke("Dest", 0.4f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(ClassicCheck == true)
        {
            Instantiate(Impact, transform.position, transform.rotation);
        }
        Destroy(gameObject);// ��򰡿� �ε�ġ�� �ı�
    }

    void Dest()
    {
        Destroy(gameObject);
    }

}
