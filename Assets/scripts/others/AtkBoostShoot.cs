using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AtkBostShoot为增益炮台
public class AtkBoostShoot : MonoBehaviour
{
    public int atkbost;//加成攻击力
    public float speedbost;//加成子弹飞行速度
    public float Stspeed;//加成射速
    public shoot st;//增益目标，后面有新炮塔这里要列出来，方便下面调用。（shoot是基础炮塔）

    private float Hp;//需要流失的血量

    // Start is called before the first frame update
    void Start()
    {
        Hp = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("Blood_loss", 10);//流血的时间控制
    }
    public void OnTriggerEnter2D(Collider2D other)//炮塔进入增益范围,shooter指的是炮塔tag
    {
        if (other.tag == "shooter")
        {
            st.SetAtkBost(atkbost);
            st.SetSpeedBost(speedbost);
            st.SetStspeed(Stspeed);
        }//这个方法比较繁琐，就是如果后面有新炮台要增益，那个炮台自己的脚本要写对应的三个增益方法，然后在这里用if调用。
    }

    public void OnTriggerExit2D(Collider2D other)//炮塔离开增益范围
    {
        if (other.tag == "shooter")
        {
               
            }
        }
    private void Blood_loss()//血量流失和损毁的方法
    {
        Hp--;
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
            st.SetAtkBost(0);
            st.SetSpeedBost(0);
            st.SetStspeed(0);
            //炮台损毁后要在这把增益清除！！！
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
