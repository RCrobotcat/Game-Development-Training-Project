using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("基础参数")]
    public float movementSpeed;         //移动速度
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

    private Rigidbody2D rb;
    public Transform playerTransform; //获取人物坐标
    private Transform enemyTransform;  //获取自身坐标
    private RaycastHit2D hit;         //射线检测

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        health_Current = health_Max;  //赋予满血
    }
    private void Update()
    {
        hit = Physics2D.Raycast(rb.position, Vector2.up, 10f, LayerMask.GetMask("Player")); //射线检测
        Check();
        TurnBack();
        if(hit && isGround)
          Jump();
    }
    private void FixedUpdate()
    { 
         PatrolMove();
    }
    #region 调用方法
    private void PatrolMove() //追击
    {
        dir = ((Vector2)playerTransform.position - (Vector2)rb.position).normalized;
        rb.velocity = new Vector2(dir.x * movementSpeed, rb.velocity.y);
    }
    private void TurnBack() //转身
    {
        if (dir.x < 0) {
            enemyTransform.localScale = new Vector3(-1, 1, 1);
        }
        if (dir.x > 0)
        {
            enemyTransform.localScale = new Vector3(1,1,1);
        }
    }
    private void Jump()   //跳跃
    {
         rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
   private void Check()
    {
        //isPlayerJump = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(offset.x * transform.localScale.x, offset.y), checkRaduis, playerLayer);  //玩家是否在头上检测
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(BottomOffset.x * transform.localScale.x, BottomOffset.y), checkRaduis_bottom, groundLayer); //地面检测
    }
    #endregion
    private void OnDrawGizmosSelected() //绘制检测范围
    {
        //Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(offset.x,offset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(BottomOffset.x, BottomOffset.y), checkRaduis_bottom);
    }

}
