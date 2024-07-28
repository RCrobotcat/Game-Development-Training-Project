using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buff : MonoBehaviour
{
    public int atkbost;//加成攻击力
    public float speedbost;//加成子弹飞行速度
    public float Stspeed;//加成射速

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D other)//炮塔进入增益范围,shooter指的是炮塔tag
    {
        if (other.tag == "Kicker_Tower")
        {
            GetComponentInParent<shoot>().SetAtkBost(atkbost);
            GetComponentInParent<shoot>().SetSpeedBost(speedbost);
            GetComponentInParent<shoot>().SetStspeed(Stspeed);
        }
    }

    public void OnTriggerExit2D(Collider2D other)//炮塔离开增益范围
    {
        if (other.tag == "Kicker_Tower")
        {
            GetComponentInParent<shoot>().SetAtkBost(0);
            GetComponentInParent<shoot>().SetSpeedBost(0);
            GetComponentInParent<shoot>().SetStspeed(0);
        }
    }
}
