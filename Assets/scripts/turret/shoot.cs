using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//该shoot为基础炮塔
public class shoot : MonoBehaviour
{
    public static shoot instance { get; private set; }

    public float shootDuration;//射击间隔
    private float shootTimer = 0;
    public Transform ShootPointTransform;//射击口
    public bullet bulletes;//子弹脚本
    public float bulletSpeed;//子弹飞行速度
    public int atkValue;//攻击力

    private List<GameObject> enemy;//可以攻击的集合
    private GameObject targetObject;//目标怪物
    private float dis;

    private Transform turret;//可以旋转的部位；

    private Vector2 dir;//射击方向

    public int Hp;//需要流失的血量
    private int Chp;//当前血量

    private int AtkBost = 0;//加成攻击力
    private float SpeedBost = 0;//加成子弹飞行速度
    private float Stspeed = 0;//加成攻击速度
    // Start is called before the first frame update
    void Start()
    {
        Chp = Hp;
        dis = 1000;
        targetObject = null;
        enemy = new List<GameObject>();
        turret = transform.GetChild(0);
        InvokeRepeating("Blood_loss", 1,1);//流血的时间控制
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemy.Count > 0)
        {
            if (targetObject == null)
            {
                //选择敌人
                targetObject = SelectTarget();
            }
        }
        if (targetObject != null)
        {
            //瞄准
            LookTarget();
        }
    }

    private void Shoot()//射击方法
    {
        shootTimer += Time.deltaTime;
        if (shootTimer > (shootDuration - Stspeed))
        {
            Shooting();
            shootTimer = 0;
        }
    }
    private void Shooting()//子弹生成
    {
        bullet bu = GameObject.Instantiate(bulletes, ShootPointTransform.position, Quaternion.identity);
        bu.SetDirection(dir);
        bu.SetSpeed(bulletSpeed + SpeedBost);
        bu.SetATKValue(atkValue + AtkBost);
    }

    public void OnTriggerEnter2D(Collider2D other)//敌人进入攻击范围,enemy指的是敌人tag
    {
        if (other.tag == "enemy")
        {
            if (!enemy.Contains(other.gameObject))
            {
                enemy.Add(other.gameObject);
            }

        }


    }

    public void OnTriggerExit2D(Collider2D other)//敌人离开攻击范围
    {
        if (other.tag == "enemy")
        {
            if (targetObject != null && other.name == targetObject.name)
            {
                targetObject = null;
            }
            if (enemy.Contains(other.gameObject))
            {
                enemy.Remove(other.gameObject);
            }
        }
    }

    //选最近敌人攻击方法：
    private GameObject SelectTarget()
    {
        GameObject temp = null;
        float distance = 0;
        for (int i = 0; i < enemy.Count; i++)
        {
            distance = Vector2.Distance(transform.position, enemy[i].transform.position);
            if (distance <= dis)
            {
                dis = distance;
                temp = enemy[i];
            }
        }
        return temp;
    }

    private void LookTarget()//瞄准方法
    {
        dir = targetObject.transform.position - transform.position;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        turret.transform.rotation = Quaternion.Euler(0, 0, angle);
        Shoot();
    }

    private void Blood_loss()//血量流失和损毁的方法
    {
        Chp--;
        if (Chp <= 0)
        {
            Destroy(this.gameObject);
            GetComponent<Collider2D>().enabled = false;
        }
    }


    public void SetAtkBost(int AtkBost)//以下为外部传入增益的方法
    {
        this.AtkBost = AtkBost;
    }

    public void SetSpeedBost(float SpeedBost)
    {
        this.SpeedBost = SpeedBost;
    }
    public void SetStspeed(float Stspeed)
    {
        this.Stspeed = Stspeed;
    }

    public void TakeDamage(int damage)//受伤函数
    {
        Chp -= damage;
    }
}





