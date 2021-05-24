using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 20f;
    public int damage = 20;
    public Rigidbody2D rigid;
    public GameObject impactEffect;



    void Start()
    {
        rigid.velocity = transform.right * speed;// ���󰡴� �ӵ�(��)
    }

    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyObject ememy = hitInfo.GetComponent<EnemyObject>();// Ememy �Լ� ȣ��
        if(ememy != null)
        {
            ememy.EnemyDamaged(damage);// Damage
        }

        Instantiate(impactEffect, transform.position, transform.rotation);// Impact Effect

        Destroy(gameObject);// destroy

    }


}
