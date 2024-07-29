using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    private Animator anim;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }
    private void OnTriggerStay2D(Collider2D other) //自爆小兵用
    {
        if (other.gameObject.tag == "Player")
        {
            movementSpeed = 0;
            anim.SetBool("isExplosion",true);
            StartCoroutine(OnDead());
        }
    }
    private IEnumerator OnDead( )              //携程
    {
        yield return new WaitForSeconds(1f);   //WaitForSeconds等待一个秒数
        Destroy(this.gameObject);
    }
}
