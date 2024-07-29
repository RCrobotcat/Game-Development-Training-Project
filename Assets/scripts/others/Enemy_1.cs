using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人模板
public class Enemy_1 : MonoBehaviour
{
    private int HP = 1000;
    private int currentHp;
    // Start is called before the first frame update
    void Start()
    {
        currentHp = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        this.currentHp -= damage;
        if (currentHp <= 0)
        {
            Destroy(this.gameObject);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
