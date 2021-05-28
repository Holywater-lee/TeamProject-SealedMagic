using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRange : MonoBehaviour
{
    public Vector3 Pos;
    [Tooltip("�� ���� ��Ÿ��1")]
    private float GcurTime;
    [Tooltip("�� ���� ��Ÿ��2")]
    [SerializeField] private float GroundTime;
    [Tooltip("�������� �ִ� ����Ʈ�� �׸�����Ʈ")]
    public GameObject[] impactEffect;

    public bool check = false;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }
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
                anim.SetTrigger("isSkill");
                PlayerObject playerMovement = GameObject.Find("Player").GetComponent<PlayerObject>();
                playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
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
        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        check = false;
    }

    void GroudAttackShoot()
    {
        GameObject GroundAtk = Instantiate(impactEffect[0], Vector3.up * 0.5f + Pos, Quaternion.identity);
        Instantiate(impactEffect[1], Vector3.up * 0.5f + Pos, Quaternion.identity);
        AttackArea area = GroundAtk.GetComponent<AttackArea>();

        if (area != null)
        {
            area.damage = 70;
            area.isEnemyAttack = false;
        } 
    }

}
