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
    private void OnTriggerStay2D(Collider2D other) //�Ա�С����
    {
        if (other.gameObject.tag == "Player")
        {
            movementSpeed = 0;
            anim.SetBool("isExplosion",true);
            StartCoroutine(OnDead());
        }
    }
    private IEnumerator OnDead( )              //Я��
    {
        yield return new WaitForSeconds(1f);   //WaitForSeconds�ȴ�һ������
        Destroy(this.gameObject);
    }
}
