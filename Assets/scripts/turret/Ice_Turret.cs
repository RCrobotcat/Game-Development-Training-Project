using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice_Turret : MonoBehaviour
{
    public static Ice_Turret instance { get; private set; }

    public int Hp;//流失的血量
    private int Chp;//当前血量
    public bool isFreazon=true;
    public float decelerateSpeed;//减速
    // Start is called before the first frame update
    void Start()
    {
        Chp = Hp;
       InvokeRepeating("Blood_loss", 1, 1);//流血的时间控制
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            if (isFreazon)
            {
                collision.GetComponent<monsterGhost>().decelerate(decelerateSpeed);
            }          
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "enemy")
        {
            collision.GetComponent<monsterGhost>().decelerate(0);
            collision.GetComponent<monsterGhost>().color();
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
