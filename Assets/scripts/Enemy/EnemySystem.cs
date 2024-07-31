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
    public Vector2[] dir = new Vector2[3];      //生成位置
    private int BossCounter = 0;

    [Header("Boss战斗")]
    public GameObject oldGround;
    public GameObject newGround;
    public float TimeCreateBoss;

    [Header("状态")]
    public bool isNext;
    public bool isEnd;
    public bool isBoss;

    public static int monsterCounte_Died;
    public int monsterCounted;

    public GameObject GameOverUi;

    public AudioSource au;
    public AudioClip bossAudioClip;

    private void Awake()
    {
        monsterCounte_Died = 0;
        isNext = true;
    }

    private void Update()
    {
        if (monsterLimit == 0)//全部波数结束就不再调用函数
        {
            isBoss = true;
        }
        if (isBoss == true && BossCounter < 1)
        {
            oldGround.SetActive(false);
            newGround.SetActive(true);
            /*BossCounter++;
            CreateBoss();*/
            TimeCreateBoss -= Time.deltaTime;
            if (TimeCreateBoss <= 0)
            {
                BossCounter++;
                CreateBoss();
            }
        }
        if (!isBoss && !isEnd)
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
        GameOver();
    }
    private void FixedUpdate()
    {
        monsterCounted = monsterCounte_Died;
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
        Instantiate(monsterObject[Random.Range(0, 3)], dir[Random.Range(0, 6)], Quaternion.identity);
    }
    public void CreateBoss()
    {
        Instantiate(monsterObject[3], dir[5], Quaternion.identity);
        au.clip = bossAudioClip;
        au.Play();
    }
    public void GameOver()  //游戏结束
    {
        if (Boss.isGameOver == true)
            isEnd = true;
    }
}
