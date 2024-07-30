using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    public static audio instance { get; private set; }

    public AudioSource SfxAudio;
    public AudioSource footStepSource;
    public AudioClip PlayerWalk;
    public AudioClip PlayerJump;
    public AudioClip PlayerDash;
    public AudioClip PlayerDeath;
    public AudioClip PlayerHurted;
    public AudioClip PlayerClimb;
    public AudioClip MonsterWalk;
    public AudioClip MonsterJump;
    public AudioClip MonsterDeath;
    public AudioClip MonsterHurted;
    public AudioClip MonsterExplode;
    public AudioClip TurretPut;
    public AudioClip TurretATK;
    public AudioClip atkTowerPut;
    public AudioClip playerHeal;

    // Update is called once per frame
    public void PlaySfx(AudioClip clip)//音效播放调用方法
    {
        SfxAudio.PlayOneShot(clip);
    }

    // footstep sound
    public void PlayWalkSfx()
    {
        footStepSource.PlayOneShot(PlayerWalk);
    }

    public void RandomPlaySfx(AudioClip clip)//音效概率播放调用方法
    {
        int R = Random.Range(1, 11);
        if (R > 5)
        {
            SfxAudio.PlayOneShot(clip);
        }
    }

    public void DoubleRandomPlaySfx(AudioClip clip, AudioClip clip02)//在两种音效中随机调用其中一种
    {
        int R = Random.Range(1, 11);
        if (R > 5)
        {
            SfxAudio.PlayOneShot(clip);
        }
        else
        {
            SfxAudio.PlayOneShot(clip02);
        }
    }
}
