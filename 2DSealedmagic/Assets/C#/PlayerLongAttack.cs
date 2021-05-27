using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLongAttack : MonoBehaviour
{
    [Tooltip("������ �߻� �Ǵ� ��ġ")]
    public Transform firePoint;
    [Tooltip("�߻� �� Bullet")]
    public GameObject[] impactEffect;
    [Tooltip("�⺻ ���� ��Ÿ��1")]
    private float ccurTime;
    [Tooltip("�� ���� ��Ÿ��1")]
    private float FcurTime;
    [Tooltip("���� ���� ��Ÿ��1")]
    private float IcurTime;
    [Tooltip("���� ���� ��Ÿ��1")]
    private float TcurTime;

    [Tooltip("�⺻ ���� ��Ÿ��2")]
    public float classicTime;
    [Tooltip("�� ���� ��Ÿ��2")]
    public float fireTime;
    [Tooltip("���� ���� ��Ÿ��2")]
    public float iceTime;
    [Tooltip("���� ���� ��Ÿ��2")]
    public float ThunderTime;

    Animator anim;

    // ���ݵ��� �� ���� ���� ������ ��Ÿ�ӵ��� ���� �ٸ��� �Ϸ��� ��.
    // ����� ���� ������ �ȉ�����, ���� ���� ó����.
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Attack();
        FireAttack();
        IceAttack();
        ThunderAttack();
    }

    void Attack()
    {
        if (ccurTime <= 0)// �⺻ ����
        {
            if (Input.GetButtonDown("Attack")) // "S" Attack (classic)
            {
                anim.SetTrigger("isAttack");
                Shoot();
                ccurTime = classicTime;
            }
        }
        else
        {
            ccurTime -= Time.deltaTime;
        }
    }

    void FireAttack()
    {
        if (FcurTime <= 0)// �� ����
        {
            if (Input.GetButtonDown("FireAttack")) // "Z" Attack (fire)
            {
                anim.SetTrigger("isAttack");
                FireShoot();
                FcurTime = fireTime;
            }
        }
        else
        {
            FcurTime -= Time.deltaTime;
        }
    }

    void IceAttack()
    {
        if (IcurTime <= 0) // ���� ����
        {
            if (Input.GetButtonDown("IceAttack")) // "X" Attack (ice)
            {
                anim.SetTrigger("isAttack");
                IceShoot();
                IcurTime = iceTime;
            }
        }
        else
        {
            IcurTime -= Time.deltaTime;
        }
    }

    void ThunderAttack()
    {
        if (TcurTime <= 0) // ���� ����
        {
            if (Input.GetButtonDown("ThunderAttack")) // "V" Attack (ice)
            {
                anim.SetTrigger("isAttack");
                ThunderShoot();
                TcurTime = ThunderTime;
            }
        }
        else
        {
            TcurTime -= Time.deltaTime;
        }
    }

    // bullet ����

    void Shoot()
    {
        GameObject ShootAtk = Instantiate(impactEffect[0], firePoint.position, firePoint.rotation);
        AttackArea area = ShootAtk.GetComponent<AttackArea>();

        if (area != null)
        {
            area.damage = 20;
            area.isEnemyAttack = false;
        }
    }

    void FireShoot()
    {  
        GameObject FireAtk = Instantiate(impactEffect[1], firePoint.position, firePoint.rotation);
        AttackArea area = FireAtk.GetComponent<AttackArea>();

        if (area != null)
        {
            area.damage = 50;
            area.isEnemyAttack = false;
            area.AttackType = "Fire";
            area.dotDamage = 5;
            area.duration = 5f;
        }
    }

    void IceShoot()
    {
        GameObject IceAtk = Instantiate(impactEffect[2], firePoint.position, firePoint.rotation);
        AttackArea area = IceAtk.GetComponent<AttackArea>();

        if (area != null)
        {
            area.damage = 30;
            area.isEnemyAttack = false;
            area.AttackType = "Ice";
            area.speedModify = 0.5f;
            area.duration = 4f;
        }
    }

    // �߰� 21.05.27
    void ThunderShoot()
    {
        GameObject thunderAtk = Instantiate(impactEffect[3], firePoint.position, firePoint.rotation);
        AttackArea area = thunderAtk.GetComponent<AttackArea>();
        
        if (area != null)
        {
            area.damage = 40;
            area.isEnemyAttack = false;
            area.AttackType = "Stun";
            area.duration = 2f;
        }
    }
}
