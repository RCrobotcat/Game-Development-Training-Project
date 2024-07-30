using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("基础参数")]
    public int monsterLimit;  //刷怪数量限制
    public int monsterEach;   //每一波怪物限制
    public int monsterCounte =0 ;  //生成限制
    public int monsterCounte_Died_IN;  //当前波次怪物死亡数量
    public GameObject[] monsterObject = new GameObject[3];
    public Vector2[] dir = new Vector2[2];      //生成位置

    [Header("状态")]
    public bool isNext;
    public bool isEnd;

    public static int monsterCounte_Died;

    private void Awake()
    {
        monsterCounte_Died = 0;
        isNext = true;
    }
    private void Update()
    {
        if (isNext == true && monsterCounte < monsterEach)
        {
            monsterCounte++;
            CreateMonster();
        }
        else
        {
            isNext = false;
            LastMonster();
        }
        if (isEnd == true)  //全部波数结束就不再调用函数
            return;
    }
    private void FixedUpdate()
    {
        monsterCounte_Died_IN = monsterCounte_Died % monsterEach;
    }
    public static void AddScore(int amount)
    {
        monsterCounte_Died += amount;
    }
    public void LastMonster()
    {
        if (monsterCounte_Died_IN == monsterEach)
        {
            monsterCounte = 0;
            isNext = true;
        }
    }

    public void CreateMonster()
    {
        Instantiate(monsterObject[Random.Range(0, 3)], dir[Random.Range(0,2)], Quaternion.identity);
    }
}
