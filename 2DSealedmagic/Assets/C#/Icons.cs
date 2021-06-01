using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Icons : MonoBehaviour
{
    public Image skillFilter;
    public Text coolTimeCounter; //���� ��Ÿ���� ǥ���� �ؽ�Ʈ

    public float coolTime;

    private float currentCoolTime; //���� ��Ÿ���� ���� �� ����

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
        if(player.FireIcon == true || player.IceIcon == true || player.GroundIcon == true || player.ThunderIcon == true)
        {
            UseSkill();
        }
    }

    public void UseSkill()
    {
        if (canUseSkill)
        {
            Debug.Log("Use Skill");
            skillFilter.fillAmount = 1; //��ų ��ư�� ����
            StartCoroutine("Cooltime");
            currentCoolTime = coolTime;
            coolTimeCounter.text = "" + currentCoolTime;

            StartCoroutine("CoolTimeCounter");

            player.FireIcon = false;
            player.IceIcon = false;
            player.GroundIcon = false;
            player.ThunderIcon = false;
        }
        else
        {
            Debug.Log("���� ��ų�� ����� �� �����ϴ�.");
        }
    }

    IEnumerator Cooltime()
    {
        while (skillFilter.fillAmount > 0)
        {
            skillFilter.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;

            yield return null;
        }

        canUseSkill = true; //��ų ��Ÿ���� ������ ��ų�� ����� �� �ִ� ���·� �ٲ�

        yield break;
    }

    //���� ��Ÿ���� ����� �ڸ�ƾ�� ������ݴϴ�.
    IEnumerator CoolTimeCounter()
    {
        while (currentCoolTime > 0)
        {
            yield return new WaitForSeconds(1.0f);

            currentCoolTime -= 1.0f;
            coolTimeCounter.text = "" + currentCoolTime;
        }

        yield break;
    }

}
