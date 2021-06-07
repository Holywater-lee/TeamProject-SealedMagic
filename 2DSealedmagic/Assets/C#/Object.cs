using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
�ۼ�: 20181220 �̼���(P)

����: ������Ʈ (ȭ�� �߻��, ��, ���� ��) ��ũ��Ʈ
�ʿ����� ���� ������ �ۼ����� �ʴ´�.

���μ��� index: ��� �����ϰ� �� ������ �ε��� ��ȣ �ۼ� (0~3)

-- �ʿ��� ���� --
ArrowDispenser: gameObj, Direction
Arrow: amount, speed
Plate: gameObj
Door: null
SealedStone: HP, index
PotionHP/PotionMP: gameObj(FX), amount, speed

-- ObjectType�� ������Ʈ ������ ���� --
ȭ��߻��: ArrowDispenser
ȭ��: Arrow
����: Plate
��: Door
���μ�: SealedStone
����: PotionHP, PotionMP

����� ArrowDispenser�� ���� ��
(����, ������ ���ӸŴ���)
*/

public class Object : MonoBehaviour
{
	[Tooltip("������Ʈ Ÿ���� �Է�")]
	[SerializeField] string ObjectType;
	[Tooltip("���⿡ ���� Right, Left �Է� / ���ʿ�� �ۼ� ���ص� ��")]
	[SerializeField] public string Direction;
	[Tooltip("������Ʈ Ÿ�Կ� ���� ������Ʈ�� ���� ��")]
	[SerializeField] GameObject gameObj;
	[Tooltip("������Ʈ�� ���ط�, ȸ���� ���� ��ġ")]
	[SerializeField] float amount;
	[Tooltip("������Ʈ �ӵ�")]
	[SerializeField] float speed;
	[Tooltip("������Ʈ ü�� (SealedStone)")]
	[SerializeField] float HP;
	[Tooltip("�ε��� ��ȣ")]
	public int index;

	bool onTrigger = false;
	public bool isFloating = false;

	Animator anim;
	SpriteRenderer spRenderer;
	AudioSource audioSource;

	Vector2 currentPos;

	float time = 0f;
	float PosY = 0f;

	void Awake()
	{
		anim = GetComponentInParent<Animator>();
		spRenderer = GetComponent<SpriteRenderer>();
	}

	void Start()
	{
		if (ObjectType == "ArrowDispenser")
		{
			audioSource = GetComponent<AudioSource>();
			StartCoroutine(ArrowDispenser(Direction));
		}
		else if (ObjectType == "Arrow")
		{
			StartCoroutine(Arrow());
		}
		else if (ObjectType == "PotionHP" || ObjectType == "PotionMP")
		{
			currentPos = transform.position;
			isFloating = true;
		}
	}

	void Update()
	{
		if (isFloating)
		{
			time += Time.deltaTime * speed;
			PosY = Mathf.Sin(time) * 0.3f; // (* ����;)
			transform.position = currentPos + new Vector2(0, PosY);
		}
	}

	IEnumerator ArrowDispenser(string Dir)
	{
		while (true)
		{
			yield return new WaitForSeconds(2f);
			GameObject arrow = Instantiate(gameObj, transform.position, Quaternion.identity);
			if (Dir == "Right") arrow.transform.localScale = new Vector3(2, 2, 2);
			else if (Dir == "Left") arrow.transform.localScale = new Vector3(-2, 2, 2);

			Sound();

			if (arrow != null)
				Destroy(arrow, 2f);
		}
	}

	IEnumerator Arrow()
	{
		int dir = (int)transform.localScale.x;
		AttackArea atkArea = GetComponent<AttackArea>();
		atkArea.isEnemyAttack = true;
		atkArea.damage = amount;
		while (true)
		{
			transform.Translate(dir * speed * Time.deltaTime, 0, 0);
			yield return null;
		}
	}

	public void SealedStoneOnAttack()
	{
		if (ObjectType == "SealedStone")
		{
			HP -= 1;
			StartCoroutine(onDamaged());

			if (HP <= 0)
			{
				PlayerLongAttack.instance.StageCheck[index] = true;

				Destroy(gameObject);
			}
		}
	}

	IEnumerator onDamaged()
	{
		spRenderer.color = new Color(1, 1, 1, 0.6f);

		yield return new WaitForSeconds(0.3f);
		spRenderer.color = new Color(1, 1, 1, 1);
	}

	void Sound()
	{
		if (audioSource != null)
			audioSource.Play();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerObject player = collision.GetComponent<PlayerObject>();
		if (ObjectType == "Plate" && collision.gameObject.tag == "Player" && !onTrigger)
		{
			onTrigger = true;
			anim.SetBool("isDoor", true);
			GameManager.instance.Sound("Plate");
			gameObj.SetActive(false);
		}

		if (ObjectType == "Arrow")
		{
			if (collision.gameObject.tag == "Player")
			{
				player.OnDamage(amount); // Ʈ�� ������
				Destroy(gameObject);
			}

			if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "Floor")
			{
				Destroy(gameObject);
			}
		}

		if (ObjectType == "PotionHP" || ObjectType == "PotionMP")
		{
			if (collision.gameObject.tag == "Player")
			{
				PlayerObject pl = FindObjectOfType<PlayerObject>();

				if (ObjectType == "PotionHP")
				{
					pl.curHealth += amount;
					if (pl.curHealth >= pl.maxHealth)
						pl.curHealth = pl.maxHealth;
				}
				else if (ObjectType == "PotionMP")
				{
					pl.curMana += amount;
					if (pl.curMana >= pl.maxMana)
						pl.curMana = pl.maxMana;
				}

				//GameObject Pfx = Instantiate(gameObj, pl.transform);
				//Destroy(Pfx, 1f);

				GameManager.instance.Sound("Potion");
				Destroy(gameObject);
			}
		}
	}
}
