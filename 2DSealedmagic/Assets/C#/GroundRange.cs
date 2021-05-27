using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRange : MonoBehaviour
{
    //public GroundBullet bulletPos;

    public Vector3 Pos;
    [Tooltip("�� ���� ��Ÿ��1")]
    private float GcurTime;
    [Tooltip("�� ���� ��Ÿ��2")]
    [SerializeField] private float GroundTime;

    public GameObject impactEffect;
    public bool check = false;

    void Update()
    {
        GroundAttack();
        
    }

    void GroundAttack()
    {
        if (GcurTime <= 0) // �� ����
        {
            if (Input.GetButtonDown("GroundAttack") && check == true) // "X" Attack (ice)
            {
                //anim.SetTrigger("isAttack");
                GroudAttackShoot();
                GcurTime = GroundTime;
                check = false;
            }
        }
        else
        {
            GcurTime -= Time.deltaTime;
            
        }
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Pos = collision.transform.position;// Ememy �Լ� ȣ��
            check = true;
        }
        else
        {
            check = false;
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        check = false;
    }



    void GroudAttackShoot()
    {
        GameObject GroundAtk = Instantiate(impactEffect, Vector3.up * 0.5f + Pos, Quaternion.identity);
        AttackArea area = GroundAtk.GetComponent<AttackArea>();

        if (area != null)
        {
            area.damage = 70;
            area.isEnemyAttack = false;
        }
        
    }
}
