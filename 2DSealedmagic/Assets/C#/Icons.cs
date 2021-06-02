using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Icons : MonoBehaviour
{
    public Image[] skillFilter;
    public Text[] coolTimeCounter; //���� ��Ÿ���� ǥ���� �ؽ�Ʈ

    public float FcoolTime;
    private float FcurrentCoolTime; //���� ��Ÿ���� ���� �� ����
    public float IcoolTime;
    private float IcurrentCoolTime; //���� ��Ÿ���� ���� �� ����
    public float GcoolTime;
    private float GcurrentCoolTime; //���� ��Ÿ���� ���� �� ����
    public float TcoolTime;
    private float TcurrentCoolTime; //���� ��Ÿ���� ���� �� ����

    private bool canUseSkill = true; //��ų�� ����� �� �ִ��� Ȯ���ϴ� ����
    PlayerLongAttack player;

    void Start()
    {
        //skillFilter.fillAmount = 0;
    }
    private void Awake()
    {
        player = FindObjectOfType<PlayerLongAttack>();
    }

    void Update()
    {
        if(player.FireIcon == true)
        {
            FSkill();
        }else if(player.IceIcon == true)
        {
            ISkill();
        }
        else if (player.GroundIcon == true)
        {
            GSkill();
        }
        else if (player.ThunderIcon == true)
        {
            TSkill();
        }
    }

    public void FSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("Fire Skill");
            coolTimeCounter[0].gameObject.SetActive(true);
            skillFilter[0].fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("FCooltime");
            FcurrentCoolTime = FcoolTime;
            coolTimeCounter[0].text = "" + FcurrentCoolTime;

            StartCoroutine("FCoolTimeCounter");

            player.FireIcon = false;
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    public void ISkill()
    {
        if (canUseSkill)
        {
            Debug.Log("Ice Skill");
            coolTimeCounter[1].gameObject.SetActive(true);
            skillFilter[1].fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("ICooltime");
            IcurrentCoolTime = IcoolTime;
            coolTimeCounter[1].text = "" + IcurrentCoolTime;

            StartCoroutine("ICoolTimeCounter");

            player.IceIcon = false;
            
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    public void GSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("Ground Skill");
            coolTimeCounter[2].gameObject.SetActive(true);
            skillFilter[2].fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("GCooltime");
            GcurrentCoolTime = GcoolTime;
            coolTimeCounter[2].text = "" + GcurrentCoolTime;

            StartCoroutine("GCoolTimeCounter");
           
            player.GroundIcon = false;
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    public void TSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("Thunder Skill");
            coolTimeCounter[3].gameObject.SetActive(true);
            skillFilter[3].fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("TCooltime");
            TcurrentCoolTime = TcoolTime;
            coolTimeCounter[3].text = "" + TcurrentCoolTime;

            StartCoroutine("TCoolTimeCounter");

            player.ThunderIcon = false;
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    IEnumerator FCooltime()
    {
        while (skillFilter[0].fillAmount > 0)
        {
            skillFilter[0].fillAmount -= 1 * Time.smoothDeltaTime / FcoolTime;

            yield return null;
        }
        coolTimeCounter[0].gameObject.SetActive(false);

        canUseSkill = true; //��ų ��Ÿ���� ������ ��ų�� ����� �� �ִ� ���·� �ٲ�

        yield break;
    }

    //���� ��Ÿ���� ����� �ڸ�ƾ�� ������ݴϴ�.
    IEnumerator FCoolTimeCounter()
    {
        while (FcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            FcurrentCoolTime -= 1.0f;
            coolTimeCounter[0].text = "" + FcurrentCoolTime;
        }
        
        yield break;
    }

    IEnumerator ICooltime()
    {
        while (skillFilter[1].fillAmount > 0)
        {
            skillFilter[1].fillAmount -= 1 * Time.smoothDeltaTime / IcoolTime;

            yield return null;
        }
        coolTimeCounter[1].gameObject.SetActive(false);

        canUseSkill = true; //��ų ��Ÿ���� ������ ��ų�� ����� �� �ִ� ���·� �ٲ�

        yield break;
    }

    //���� ��Ÿ���� ����� �ڸ�ƾ�� ������ݴϴ�.
    IEnumerator ICoolTimeCounter()
    {
        while (IcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            IcurrentCoolTime -= 1.0f;
            coolTimeCounter[1].text = "" + IcurrentCoolTime;
        }

        yield break;
    }
    IEnumerator GCooltime()
    {
        while (skillFilter[2].fillAmount > 0)
        {
            skillFilter[2].fillAmount -= 1 * Time.smoothDeltaTime / GcoolTime;

            yield return null;
        }
        coolTimeCounter[2].gameObject.SetActive(false);

        canUseSkill = true; //��ų ��Ÿ���� ������ ��ų�� ����� �� �ִ� ���·� �ٲ�

        yield break;
    }

    //���� ��Ÿ���� ����� �ڸ�ƾ�� ������ݴϴ�.
    IEnumerator GCoolTimeCounter()
    {
        while (GcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            GcurrentCoolTime -= 1.0f;
            coolTimeCounter[2].text = "" + GcurrentCoolTime;
        }
        
        yield break;
    }
    IEnumerator TCooltime()
    {
        while (skillFilter[3].fillAmount > 0)
        {
            skillFilter[3].fillAmount -= 1 * Time.smoothDeltaTime / TcoolTime;

            yield return null;
        }
        coolTimeCounter[3].gameObject.SetActive(false);

        canUseSkill = true; //��ų ��Ÿ���� ������ ��ų�� ����� �� �ִ� ���·� �ٲ�

        yield break;
    }

    //���� ��Ÿ���� ����� �ڸ�ƾ�� ������ݴϴ�.
    IEnumerator TCoolTimeCounter()
    {
        while (TcurrentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            TcurrentCoolTime -= 1.0f;
            coolTimeCounter[3].text = "" + TcurrentCoolTime;
        }
        
        yield break;
    }
}
