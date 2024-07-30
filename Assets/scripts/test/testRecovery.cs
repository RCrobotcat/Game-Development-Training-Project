using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRecovery : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.instance.ChangeHealth(1);
            magicController.instance.changeMagic(1);
        }
        /*if (collision.gameObject.CompareTag("Monster"))
        {
            monsterGhost.instance.TakeDamage(1);
        }*/
    }
}
