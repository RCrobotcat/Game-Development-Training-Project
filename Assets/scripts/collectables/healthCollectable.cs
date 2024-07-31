using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthCollectable : MonoBehaviour
{
    private PolygonCollider2D healthCollider;

    public float existTimer;
    public float existTime = 10.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController.instance.health < playerController.instance.maxHealth || magicController.instance.magic < magicController.instance.maxMagic)
            {
                playerController.instance.ChangeHealth(1);
                magicController.instance.changeMagic(1);
                Destroy(gameObject);
            }
            else return;
        }
    }

    private void Start()
    {
        healthCollider = GetComponent<PolygonCollider2D>();
        existTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ignorePlayer();

        if (existTimer < existTime)
        {
            existTimer += Time.deltaTime;
        }
        else if (existTimer >= existTime)
        {
            Destroy(gameObject);
        }
    }

    private void ignorePlayer()
    {
        Collider2D[] hits_1 = Physics2D.OverlapCircleAll(transform.position, 10.0f);
        foreach (var hit in hits_1)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                Collider2D playerCollider = hit.GetComponent<Collider2D>();
                if (playerCollider != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, healthCollider, true);
                }
            }
        }
    }
}
