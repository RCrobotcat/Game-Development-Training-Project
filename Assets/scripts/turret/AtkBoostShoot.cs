using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AtkBostShoot为增益炮台
public class AtkBoostShoot : MonoBehaviour
{
    public static AtkBoostShoot instance { get; private set; }

    public int addhp;//给玩家加的血量
    public int Hp;//需要流失的血量
    private int Chp;//当前血量

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Chp = Hp;
        InvokeRepeating("Blood_loss", 1, 1);//流血的时间控制
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.instance.ChangeHealth(addhp); //回血量
        }
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

    public void TakeDamage(int damage)//受伤函数
    {
        Chp -= damage;
    }
}
