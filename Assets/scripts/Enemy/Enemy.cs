using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

    private Rigidbody2D rb;
    public Transform playerTransform; //��ȡ��������
    private Transform enemyTransform;  //��ȡ��������
    private RaycastHit2D hit;         //���߼��

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyTransform = GetComponent<Transform>();
        health_Current = health_Max;  //������Ѫ
    }
    private void Update()
    {
        hit = Physics2D.Raycast(rb.position, Vector2.up, 10f, LayerMask.GetMask("Player")); //���߼��
        Check();
        TurnBack();
        if(hit && isGround)
          Jump();
    }
    private void FixedUpdate()
    { 
         PatrolMove();
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
            enemyTransform.localScale = new Vector3(-1, 1, 1);
        }
        if (dir.x > 0)
        {
            enemyTransform.localScale = new Vector3(1,1,1);
        }
    }
    private void Jump()   //��Ծ
    {
         rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }
   private void Check()
    {
        //isPlayerJump = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(offset.x * transform.localScale.x, offset.y), checkRaduis, playerLayer);  //����Ƿ���ͷ�ϼ��
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(BottomOffset.x * transform.localScale.x, BottomOffset.y), checkRaduis_bottom, groundLayer); //������
    }
    #endregion
    private void OnDrawGizmosSelected() //���Ƽ�ⷶΧ
    {
        //Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(offset.x,offset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(BottomOffset.x, BottomOffset.y), checkRaduis_bottom);
    }

}
