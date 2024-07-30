using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("基础参数")]
    public int monsterLimit;  //刷怪每波限制
    public int monsterEach;   //每一波怪物限制
    public int monsterCounte = 0;  //生成限制
    public GameObject[] monsterObject = new GameObject[3];
    public Vector2[] dir = new Vector2[2];      //生成位置

    [Header("状态")]
    public bool isNext;
    public bool isEnd;

    public static int monsterCounte_Died;

    public int monsterCount;

    private void Awake()
    {
        monsterCounte_Died = 0;
        isNext = true;
    }

    private void FixedUpdate()
    {
        // monsterCounte_Died = monsterCounte_Died % (monsterEach + 1);
        monsterCount = monsterCounte_Died;

        if (monsterLimit == 0)//全部波数结束就不再调用函数
        {
            isEnd = true;
        }
        if (!isEnd)
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
        }
    }
    public static void AddScore(int amount)
    {
        monsterCounte_Died += amount;
    }
    public void LastMonster()
    {
        if (monsterCounte_Died == monsterEach)
        {
            monsterCounte_Died = 0;
            monsterLimit--;
            monsterCounte = 0;
            isNext = true;
        }
    }

    public void CreateMonster()
    {
        Instantiate(monsterObject[Random.Range(0, 3)], dir[Random.Range(0, 2)], Quaternion.identity);
    }
}
