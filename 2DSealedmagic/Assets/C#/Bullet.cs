using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rigid;
 



    void Start()
    {
        rigid.velocity = transform.right * speed;// ���󰡴� �ӵ�(��)
    }

    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Monster monster = hitInfo.GetComponent<Monster>();// Ememy �Լ� ȣ��
        if(monster != null)
        {
            monster.onAttack(damage);// Damage
        }

        Destroy(gameObject);// destroy

    }


}
