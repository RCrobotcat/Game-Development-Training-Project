using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerController.instance.ChangeHealth(-1);
        }
    }
}