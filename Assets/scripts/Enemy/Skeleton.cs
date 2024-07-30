using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    public GameObject ExplosionRange;
    private Animator anim;

    public audio au;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }
    private void OnTriggerStay2D(Collider2D other) //自爆小兵用
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isStop = true;
            movementSpeed = 0;
            anim.SetBool("isExplosion", true);
            StartCoroutine(OnDead());
        }
    }
    private IEnumerator OnDead()              //携程
    {
        yield return new WaitForSeconds(1.09f);   //WaitForSeconds等待一个秒数
        {
            ExplosionRange.SetActive(true);
        }
    }
    //public void OnDestroy()
    //{
    //    Destroy(this.gameObject);
    //}

    public void PlayExplosionSfx()
    {
        au.SfxAudio.Stop();
        au.PlaySfx(au.MonsterExplode);
    }

    public void playFuseSfx()
    {
        au.PlaySfx(au.MonsterHurted);
    }
}
