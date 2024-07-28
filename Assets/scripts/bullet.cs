using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//子弹模板
public class bullet : MonoBehaviour
{
    private float speed;//子弹飞行速度
    private int atkValue;//攻击力
    private Vector2 direction;//方向
    
    public void SetATKValue(int atkValue)//外部传入子弹数值的方法
    {
        this.atkValue = atkValue;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);//自身朝向飞行方向
        transform.position = direction * speed * Time.deltaTime + (Vector2)transform.position;//飞行方向
    }

    private void OnTriggerEnter2D(Collider2D collision)//子弹接触到敌人并销毁
    {
        if(collision.tag == "enemy")
        {
            Destroy(this.gameObject);
            collision.GetComponent<Enemy>().TakeDamage(atkValue);
        }
    }
}
