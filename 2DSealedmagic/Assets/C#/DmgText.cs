using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;// ���� �����ϱ� ���� TMP

public class DmgText : MonoBehaviour
{
    [Tooltip("�������� ���� �ö󰡴� �ӵ�")]
    public float moveSpeed;
    [Tooltip("�������� ���������� �ӵ�")]
    public float alphaSpeed;
    [Tooltip("�������� �ı��Ǵ� �ð�")]
    public float destroyTime;
    [Tooltip("����")]
    Color alpha;
    [Tooltip("�޾ƿ� ������")]
    public float damage;

    TextMeshPro text;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();// �������� �޾ƿ´�
        alpha = text.color;// ����
        Invoke("DestroyObject", destroyTime);
    }


    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;
    }

    private void DestroyObject()// �ı�
    {
        Destroy(gameObject);
    }
}
