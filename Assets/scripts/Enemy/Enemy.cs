using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health { get { return health_Current; } }

    [Header("基础参数")]
    public float movementSpeed;         //移动速度             //保存原始速度
    public Vector2 dir;                 //怪物与角色之间的差值
    public float jumpForce;             //跳跃力
    public float checkRange;            //检测范围：玩家跳跃怪物也会跟着跳
    public float health_Max;            //满血
    public float health_Current;       //当前血量

    // [Header("跟随跳跃参数")]
    // public Vector2 offset;              //矫正位移差
    // public float checkRaduis;          //检测半径
    // public LayerMask playerLayer;      //检测图层

    [Header("地面检测")]
    public Vector2 BottomOffset;      //地面矫正
    public float checkRaduis_bottom;   //检测半径
    public LayerMask groundLayer;      //检测图层

    [Header("状态检测")]
    // public bool isPlayerJump;           //玩家是否在跳跃
    public bool isGround;                 //怪物是否在地面
    public bool isDead;                  //是否死亡
    public bool isStop;                  //禁止移动

    [Header("生命字体")]
    public Text healthText;          //生命字体
    public GameObject healthTextGameObject;
    float textTimer = 2.0f;
    float textTimerSeconds;

    protected Rigidbody2D rb;
    private GameObject playerObject; //获取人物坐标
    private Transform playerTransform;
    private Transform enemyTransform;  //获取自身坐标
    public RaycastHit2D hit;         //射线检测
    private SpriteRenderer sprite;    //精灵
    private Animator anim;          //动画器 
    public GameObject healthCollectable;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag("Player");
        playerTransform = playerObject.transform;
        health_Current = health_Max;  //赋予满血
        textTimerSeconds = textTimer;
    }
    protected virtual void Update()
    {
        if (!isStop)
        {
            hit = Physics2D.Raycast(rb.position, Vector2.up, 10f, LayerMask.GetMask("Player")); //射线检测
            Check();
            TurnBack();
            if (hit && isGround)
                Jump();
        }
    }
    private void FixedUpdate()
    {
        if (!isStop)
            PatrolMove();
        if (textTimerSeconds > 0)  //伤害字体消失
        {
            textTimerSeconds -= Time.deltaTime;
        }
        else
        {
            healthTextGameObject.SetActive(false);
            textTimerSeconds = textTimer;
        }
        if (isDead == true)  //如果死亡就无法进行任何操作
        {
            isStop = true;
            anim.SetBool("isDead", true);
        }
    }
    #region 调用方法
    private void PatrolMove() //追击
    {
        dir = ((Vector2)playerTransform.position - (Vector2)rb.position).normalized;
        rb.velocity = new Vector2(dir.x * movementSpeed, rb.velocity.y);
    }
    private void TurnBack() //转身
    {
        if (dir.x < 0)
        {
            sprite.flipX = true;
        }
        if (dir.x > 0)
        {
            sprite.flipX = false;
        }
    }
    private void Jump()   //跳跃
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
    private void Check()  //检测
    {
        //isPlayerJump = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(offset.x * transform.localScale.x, offset.y), checkRaduis, playerLayer);  //玩家是否在头上检测
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(BottomOffset.x * transform.localScale.x, BottomOffset.y), checkRaduis_bottom, groundLayer); //地面检测
    }
    public void OnDestroy_Died()
    {
        // 40%几率掉落红心(可恢复血量和专注值)
        int dropHealthRate = Random.Range(1, 11);
        if (dropHealthRate <= 4)
        {
            Instantiate(healthCollectable, gameObject.transform.position, Quaternion.identity);
        }

        /*Instantiate(healthCollectable, gameObject.transform.position, Quaternion.identity);*/

        EnemySystem.AddScore(1);

        Destroy(this.gameObject);
    }

    #endregion
    private void OnDrawGizmosSelected() //绘制检测范围
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

    public void decelerate(float speed02)
    {
        Color newColor = new Color(0.6f, 0.7f, 0.9f);
        sprite.color = newColor;
        this.movementSpeed -= speed02;
    }
    public void color()
    {
        Color newColor = new Color(1, 1, 1);
        sprite.color = newColor;
    }
}
