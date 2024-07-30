using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //�ӿ�
    public static Enemy instance { get; private set; }
    public float health { get { return health_Current; } }

    [Header("��������")]
    public float movementSpeed;         //�ƶ��ٶ�
    public Vector2 dir;                 //�������ɫ֮��Ĳ�ֵ
    public float jumpForce;             //��Ծ��
    public float checkRange;            //��ⷶΧ�������Ծ����Ҳ�������
    public float health_Max;            //��Ѫ
    public float health_Current;       //��ǰѪ��

   // [Header("������Ծ����")]
   // public Vector2 offset;              //����λ�Ʋ�
   // public float checkRaduis;          //���뾶
   // public LayerMask playerLayer;      //���ͼ��

    [Header("������")]
    public Vector2 BottomOffset;      //�������
    public float checkRaduis_bottom;   //���뾶
    public LayerMask groundLayer;      //���ͼ��

    [Header("״̬���")]
   // public bool isPlayerJump;           //����Ƿ�����Ծ
    public bool isGround;                 //�����Ƿ��ڵ���
    public bool isDead;                  //�Ƿ�����
    public bool isStop;                  //��ֹ�ƶ�

    [Header("��������")]
    public Text healthText;          //��������
    public GameObject healthTextGameObject; 
    float textTimer = 2.0f;
    float textTimerSeconds;

    protected  Rigidbody2D rb;
    private GameObject playerObject; //��ȡ��������
    private Transform playerTransform;
    private Transform enemyTransform;  //��ȡ��������
    public RaycastHit2D hit;         //���߼��
    private SpriteRenderer sprite;    //����
    private Animator anim;          //������ 

    protected virtual void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag("Player");
        playerTransform = playerObject.transform;
        health_Current = health_Max;  //������Ѫ
        textTimerSeconds = textTimer;
    }
    protected virtual void Update()
    {
        if(!isStop)
        {
            hit = Physics2D.Raycast(rb.position, Vector2.up, 10f, LayerMask.GetMask("Player")); //���߼��
            Check();
            TurnBack();
            if (hit && isGround)
                Jump();
        }
    }
    private void FixedUpdate()
    { 
        if(!isStop)
         PatrolMove();
        if (textTimerSeconds > 0)  //�˺�������ʧ
        {
            textTimerSeconds -= Time.deltaTime;
        }
        else
        {
            healthTextGameObject.SetActive(false);
            textTimerSeconds = textTimer;
        }
        if (isDead == true)  //����������޷������κβ���
        {
            isStop = true;
            anim.SetBool("isDead", true);
        }  
    }
    #region ���÷���
    private void PatrolMove() //׷��
    {
        dir = ((Vector2)playerTransform.position - (Vector2)rb.position).normalized;
        rb.velocity = new Vector2(dir.x * movementSpeed, rb.velocity.y);
    }
    private void TurnBack() //ת��
    {
        if (dir.x < 0) {
            sprite.flipX = true;
        }
        if (dir.x > 0)
        {
            sprite.flipX = false;
        }
    }
    private void Jump()   //��Ծ
    {
         rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
   private void Check()  //���
    {
        //isPlayerJump = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(offset.x * transform.localScale.x, offset.y), checkRaduis, playerLayer);  //����Ƿ���ͷ�ϼ��
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(BottomOffset.x * transform.localScale.x, BottomOffset.y), checkRaduis_bottom, groundLayer); //������
    }
    public void OnDestroy_Died()
    {
        Destroy(this.gameObject);
    }

    #endregion
    private void OnDrawGizmosSelected() //���Ƽ�ⷶΧ
    {
        //Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(offset.x,offset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(BottomOffset.x, BottomOffset.y), checkRaduis_bottom);
    }

    public void TakeDamage(int damage)
    {
        health_Current -= damage;
        anim.SetTrigger("isHit");

        healthText.text = health_Current.ToString() + "/" + health_Max.ToString();
        healthTextGameObject.SetActive(true);

        if (health_Current <= 0)
        {
            isDead = true;
        }
    }
}
