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
    private void OnTriggerStay2D(Collider2D other) //�Ա�С����
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isStop = true;
            movementSpeed = 0;
            anim.SetBool("isExplosion", true);
            StartCoroutine(OnDead());
        }
    }
    private IEnumerator OnDead()              //Я��
    {
        yield return new WaitForSeconds(1.09f);   //WaitForSeconds�ȴ�һ������
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
