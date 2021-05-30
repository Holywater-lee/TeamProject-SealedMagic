using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRange : MonoBehaviour
{
    [Tooltip("�� ���� ��Ÿ��1")]
    private float GcurTime;
    [Tooltip("�� ���� ��Ÿ��2")]
    [SerializeField] private float GroundTime;
    [Tooltip("���� ���� ��Ÿ��1")]
    private float TcurTime;
    [Tooltip("���� ���� ��Ÿ��2")]
    public float ThunderTime;
    [Tooltip("�������� �ִ� ����Ʈ�� �׸�����Ʈ")]
    public GameObject[] impactEffect;
    [Tooltip("Layer üũ")]
    public LayerMask InLayer;
    [Tooltip("�� hit ũ�� ����")]
    public Vector2 size;
    [Tooltip("���� hit ũ�� ����")]
    public Vector2 size2;
    [Tooltip("ĳ���� ������ hit ��������")]
    public int moveDic = 1;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }
    void Update()
    {
        MoveDic();
        GroundAttack();
        ThunderAttack();
    }

    void MoveDic()
    {
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            moveDic = -1;
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            moveDic = 1;
        }
    }
    void GroundAttack()
    {
        if (GcurTime <= 0) // �� ����
        {
            if (Input.GetButtonDown("GroundAttack")) // "X" Attack (ice)
            {
                anim.SetTrigger("isSkill");
                PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
                playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
                GroudAttackShoot();
                GcurTime = GroundTime;
                
            }
        }
        else
        {
            GcurTime -= Time.deltaTime;
        }
        
    }

    void ThunderAttack()
    {

        if (TcurTime <= 0) // ���� ����
        {
            if (Input.GetButtonDown("ThunderAttack")) // "V" Attack (ice)
            {
                anim.SetTrigger("isSkill");
                
                PlayerObject playerMovement = FindObjectOfType<PlayerObject>();
                playerMovement.bCanMove = false; // ĳ���� �̵� ��Ȱ��ȭ
                ThunderShoot();
                TcurTime = ThunderTime;
            }
        }
        else
        {
            TcurTime -= Time.deltaTime;
        }

    }

    // ���� �ȿ� ������ ���� Ű�� ������ �� ����
    void GroudAttackShoot()
    {

        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + new Vector3(4.5f * moveDic, 0, 0) , new Vector2(8, 0.5f), 0, InLayer);
        foreach (Collider2D i in hit)
        {
            
            GameObject GroundAtk = Instantiate(impactEffect[0],i.transform.position, Quaternion.identity);
            Instantiate(impactEffect[1], Vector3.up * 0.5f +  i.transform.position, Quaternion.identity);
            AttackArea area = GroundAtk.GetComponent<AttackArea>();

            if (area != null)
            {
                area.damage = 70;
                area.isEnemyAttack = false;
            }
            break;
        }
        
    }
    // ���� �ȿ� ������ ���� Ű�� ������ ���� ����
    void ThunderShoot()
    {
        Collider2D[] Thit = Physics2D.OverlapBoxAll(transform.position + new Vector3(4.5f * moveDic, 1.4f, 0), new Vector2(8, 4.5f), 0, InLayer);
        foreach (Collider2D i in Thit)
        {
            GameObject thunderAtk = Instantiate(impactEffect[2], i.transform.position, Quaternion.identity);
            Instantiate(impactEffect[3], Vector3.up * 2.5f + i.transform.position, Quaternion.identity);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(4.5f * moveDic, 0, 0), size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + new Vector3(4.5f * moveDic, 1.4f, 0), size2);
    }

}
