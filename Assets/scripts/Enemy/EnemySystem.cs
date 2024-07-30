using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    [Header("��������")]
    public int monsterLimit;  //ˢ��ÿ������
    public int monsterEach;   //ÿһ����������
    public int monsterCounte = 0;  //��������
    public GameObject[] monsterObject = new GameObject[3];
    public Vector2[] dir = new Vector2[2];      //����λ��

    [Header("״̬")]
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

        if (monsterLimit == 0)//ȫ�����������Ͳ��ٵ��ú���
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
