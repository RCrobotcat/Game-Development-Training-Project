using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class monsterGhost : MonoBehaviour
{
    public static monsterGhost instance { get; private set; }

    public float changeTime = 2.0f;
    public float speed = 5f;
    bool left;

    Rigidbody2D rb;
    float timer;
    int direction = 1;

    private SpriteRenderer sprite;
    private Animator animator;

    bool isDead;

    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    public Text healthText;
    public GameObject healthTextGameObject;
    float textTimer = 2.0f;
    float textTimerSeconds;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        timer = changeTime;
        currentHealth = maxHealth;
        textTimerSeconds = textTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        if (direction == -1)
        {
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
        }

        if (textTimerSeconds > 0)
        {
            textTimerSeconds -= Time.deltaTime;
        }
        else
        {
            healthTextGameObject.SetActive(false);
            textTimerSeconds = textTimer;
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;

        if (left)
        {
            position.x = position.x + Time.deltaTime * speed * direction;
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
        }

        if (!isDead)
        {
            rb.MovePosition(position);
        }

    }

    public void setTriggerToDie()
    {
        animator.SetTrigger("GhostDie");
        isDead = true;
    }

    void OnDisappearAnimationComplete()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("isHit");

        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
        healthTextGameObject.SetActive(true);

        if (currentHealth <= 0)
        {
            setTriggerToDie();
        }
    }
}
