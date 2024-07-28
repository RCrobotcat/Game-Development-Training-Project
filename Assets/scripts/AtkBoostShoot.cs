using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AtkBostShoot为增益炮台
public class AtkBoostShoot : MonoBehaviour
{
    

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
    
    private void Blood_loss()//血量流失和损毁的方法
    {
        Hp--;
        if (Hp <= 0)
        {
            Destroy(this.gameObject);
            //st.SetAtkBost(0);
            //st.SetSpeedBost(0);
            //st.SetStspeed(0);
            //炮台损毁后要在这把增益清除！！！
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
