using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float ghostDelay = 0.05f;
    private float ghostDelaySeconds;
    public GameObject ghost;
    public bool makeGhost;
    public playerController player;

    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                // Create a ghost object
                GameObject currentGhost;
                if (player.playerSprite.flipX)
                {
                    currentGhost = Instantiate(ghost, transform.position, Quaternion.Euler(0, 180, 0));
                }
                else
                {
                    currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                }
                ghostDelaySeconds = ghostDelay;
                Destroy(currentGhost, 1f); // Destroy the ghost object after 1 second
            }
        }
    }
}
