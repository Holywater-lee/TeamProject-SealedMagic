using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerObject : MonoBehaviour
{
    [Tooltip("�÷��̾��� �ִ� ü�� ����")]
    public float maxHealth;
    [Tooltip("�÷��̾��� ���� ü�� ����")]
    public float curHealth;
    [Tooltip("�÷��̾��� �ִ� ���� ����")]
    public float maxMana;
    [Tooltip("�÷��̾��� ���� ���� ����")]
    public float curMana;

    [Tooltip("�÷��̾��� �ִ� �ӵ�")]
    [SerializeField] private float maxSpeed;
    [Tooltip("�÷��̾� �⺻ �ӵ�")]
    private float runSpeed = 8;
    [Tooltip("������ ������ ���� �ö󰡴� �ӵ�")]
    [SerializeField] private float jumpPower;
    [Tooltip("���� ī��Ʈ(���� ����)")]
    [SerializeField] private float jumpcount = 2;
    [Tooltip("�÷��̾ ���� ������ �̹���")]
    public GameObject AttackDamageText;
    [Tooltip("�÷��̾ ���� ������ ��ġ")]
    public Transform hudPos;
    [Tooltip("������ ��ġ�� �޾ƿ��� ��")]
    public static Vector3 Altarpos;
    [Tooltip("���ݽ� �̵� Ȱ��ȭ/��Ȱ��/ ���������� Ȱ��")]
    public bool bCanMove = true;
    [Tooltip("�����϶� �� ������")]
    public bool NoDamege = false;
    [Tooltip("���� �Ҷ� ����")]
    public bool DownSound = false;
    public LayerMask JumpCheck;

    [SerializeField] AudioClip audioWalk;
    [SerializeField] AudioClip audioJumpUp;
    [SerializeField] AudioClip audioJumpDown;
    [SerializeField] AudioClip audiofocus;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    Altar obj;
    AudioSource audioSource;

    private void Start()
    {
        InvokeRepeating("NatureMana", 1, 1);// �ڿ� ���� ���
        GameManager.instance.FindAndGetInfo();
        UserInterface.instance.FindPlayer();
        Icons.instance.FindPlayer();
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Jump();
        chage_Move();
        KeyEnter();// ���� ����
    }

    void FixedUpdate()
    {
        Move();
        Jump_Check();
        HPMana();
    }

    // �÷��̾ �����϶�
    void Move()
    {
        if (bCanMove)
        {
            if (!Input.GetButtonDown("Attack"))
            {
                //PlayerSound("Walk");
                // Left and right Move
                float h = Input.GetAxisRaw("Horizontal");
                rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
            }
        }
        else
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.001f, rigid.velocity.y);
        }
        // Max speed
        if (rigid.velocity.x > maxSpeed)// right max Speed
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            
        }
        else if (rigid.velocity.x < maxSpeed * (-1))// left max Speed
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
            
        }

        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            //run
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("isRun", true);
                maxSpeed = 13;
            }
            else
            {
                anim.SetBool("isRun", false);
                maxSpeed = runSpeed;
            }
        }

    }


    // ���⶧ �̹��� ����� �¿���� �̹��� ����
    void chage_Move()
    {
        // Stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.001f, rigid.velocity.y);
            anim.SetBool("isRun", false);
        }
        if (bCanMove)
        {
            //Direction image chage
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                //spriteRenderer.flipX = true; // 21.05.26 �߰�
            }
            else if (Input.GetAxis("Horizontal") != 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                //spriteRenderer.flipX = false; // 21.05.26 �߰�
            }
        }

        // Animation(classic and run)
        if (Mathf.Abs(rigid.velocity.x) < 0.5)
        {
            anim.SetBool("isWalk", false);
        }
        else if (Mathf.Abs(rigid.velocity.x) > 0.001)
        {
            anim.SetBool("isWalk", true);
        }
    }

    // ���� �Ҷ��� �ӵ�
    void Jump()
    {
        if (bCanMove)
        {
            // Jump
            if (Input.GetButtonDown("Jump") && jumpcount > 0) // spease
            {
                if (jumpcount == 1)// double Jump
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 0.9f);
                }
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                anim.SetBool("isRun", false);// �޸��ٰ� ���� �� ����.
                anim.SetBool("Jumpingdown", false);// �ٽ� ������ ������, �������� ����� �������.
                PlayerSound("JumpUp");
                DownSound = true;
                anim.SetBool("Jumping", true);// jump image
                jumpcount--;
            }
        }

        // Jump Max speed
        if (rigid.velocity.y > jumpPower)// right max Speed
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpPower * 0.8f);
        }
        
    }

    // ���� �� �̹��� �� �ٴ� üũ �� ī���� �ʱ�ȭ
    void Jump_Check()
    {
        // Jump image chage (Ground Contact)
        if (rigid.velocity.y <= 0)
        {
            anim.SetBool("Jumping", false);// jump UP image
            anim.SetBool("Jumpingdown", true);// jump Down image


            Vector2 frontVec = new Vector2(rigid.position.x + rigid.velocity.x * 0.03f, rigid.position.y);
            Debug.DrawRay(frontVec, Vector3.down, new Color(1, 0, 0));

            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 2f, JumpCheck);

            
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.8f)
                {
                    if(DownSound == true)
                    {
                        PlayerSound("JumpDown");
                        DownSound = false;
                    }
                    anim.SetBool("Jumpingdown", false);// jump image
                    jumpcount = 2;
                }
                
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && NoDamege == false)
        {
            OnDamage(10);// ������ �޾ƿ;� �ϴµ� ���� ����
            if (curHealth > 0)
            {
                OnDamaged(collision.transform.position);
            }

        }
        if (collision.gameObject.tag == "Spikes" && NoDamege == false)
        {
            OnDamage(100);
            if (curHealth > 0)
            {
                OnDamaged(collision.transform.position);
            }
        }
    }

    public void OnDamage(float damage)
    {
        if (NoDamege == true)
        {
            damage = 0;
        }
        else
        {
            GameObject AttackText = Instantiate(AttackDamageText);// ������ �̹��� ���
            AttackText.transform.position = hudPos.position;// ������ ��ġ
            AttackText.GetComponent<DmgText>().damage = damage;// ������ �� �ޱ�
        }

        //Damage
        curHealth -= damage;
        if (curHealth >= 0 && NoDamege == false)
        {
            OnDamaged(transform.position); // 21.05.26 �߰�
        }
    }

    // �������� �Ծ����� �ðܳ��� ��
    void OnDamaged(Vector2 targetPos)
    {
        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;// Damaged Direction
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

        //Change Layer (Immortal Active)
        //gameObject.layer = 8;

        // Damaged motion coloer
        StartCoroutine("Damotion");

        //Animation
        anim.SetTrigger("isDamaged");

        NoDamege = true;
        StartCoroutine("OffDamaged");
        //Invoke("OffDamaged", 2);// ���� �ð�

    }

    IEnumerator Damotion()
    {
        int i = 0;
        while (i < 10)
        {
            if (i % 2 == 0)
                spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            else
                spriteRenderer.color = new Color(1, 1, 1, 0.8f);
            yield return new WaitForSeconds(0.2f);
            i++;
        }
    }

    // �������� �԰� ����
    IEnumerator OffDamaged()
    {
        yield return new WaitForSeconds(2f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        NoDamege = false;
    }

    // �ڿ� ���� ����
    void NatureMana()
    {
        if (curMana < maxMana)
        {
            curMana += 5;
        }
    }

    // �� ����
    void HPMana()
    {
        if (curHealth >= maxHealth)
        {
            curHealth = maxHealth;
        }
        else if (curHealth <= 0)// �׾�����
        {
            gameObject.layer = 9;
            // �� �����̰� ����
            bCanMove = false;
            // ü�� 0
            curHealth = 0;
            // ������ ����!
            anim.SetBool("isDie", true);
            // ����ȭ
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            //  ������ �̹��� ����
            UserInterface.instance.GameOver();
            // 2�ʵ� ������������ �̵�
            //Invoke("isDie", 2);
        }
        if (curMana >= maxMana)
        {
            curMana = maxMana;
        }
        else if (curMana <= 0)
        {
            curMana = 0;
        }
    }

    /*
    �ۼ�: 20181220 �̼���(P)

    ����: ���ܿ� �ִ� Interact �Լ��� �����ϰ� ���ִ� ������Ʈ
    ��ȹ�� �� ��ȣ�ۿ� Ű (���� ��� Ű): FŰ (default)
    */

    void KeyEnter()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (Input.GetKeyDown(KeyCode.F) && obj != null && gameManager.killedColoredMonster >= 3 && !obj.isInteracting)
        {
            obj.isInteracting = true;
            Altarpos = transform.position;
            rigid.velocity = Vector2.zero;
            PlayerSound("Focus");
            Debug.Log("���� ����!");
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Altar")
        {
            obj = FindObjectOfType<Altar>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        obj = null;
    }
    
    void PlayerSound(string action)
    {
        switch (action)
        {
            case "Walk":
                audioSource.clip = audioWalk;
                audioSource.PlayOneShot(audioWalk);
                break;
            case "JumpUp":
                audioSource.clip = audioJumpUp;
                audioSource.PlayOneShot(audioJumpUp);
                break;
            case "JumpDown":
                audioSource.clip = audioJumpDown;
                audioSource.PlayOneShot(audioJumpDown);
                break;
            case "Focus":
                audioSource.clip = audiofocus;
                audioSource.PlayOneShot(audiofocus);
                break;
            default:
                Debug.Log("ERROR: �ش� ���� �� ����");
                break;
        }
       
            

    }
}
