using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int onAttcker;

    private void OnTriggerEnter2D(Collider2D others)
    {
        if (others.gameObject.CompareTag("Player"))
        {
            playerController.instance.ChangeHealth(onAttcker);
        }
    }
}
