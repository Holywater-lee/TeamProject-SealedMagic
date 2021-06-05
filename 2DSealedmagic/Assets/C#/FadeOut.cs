using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    /*����ٰ� Det() �־, ���� �������� ������ ����Ʈ ó�� ����*/

    [Tooltip("���� �ð�")]
    public float fadeOutTime = 1.0f;
    [Tooltip("���������� ����")]
    public bool fadeinout = true;


    void Start()
    {
        FaOut();
    }


    void FaOut()
    {
        if (fadeinout == true)
            StartCoroutine(aktfadeOut(GetComponent<SpriteRenderer>()));
    }

    IEnumerator aktfadeOut(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;
        while (tmpColor.a > 0f)
        {
            tmpColor.a -= 1f * Time.deltaTime / fadeOutTime;
            _sprite.color = tmpColor;
            if (tmpColor.a <= 0f)
                tmpColor.a = 0f;
            yield return null;
        }
        Destroy(this.gameObject);
        _sprite.color = tmpColor;
    }

    void Det()
    {
        Destroy(gameObject);
    }
}
