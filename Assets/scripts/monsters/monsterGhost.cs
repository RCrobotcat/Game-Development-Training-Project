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
    public float speed02;
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

    [Header("生命字体")]
    public Text healthText;          //生命字体
    public GameObject healthTextGameObject; //
    float textTimer = 2.0f;
    float textTimerSeconds;

    private int iceTime;
    private float icespeed;

    public SpriteRenderer spriteRenderer;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
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
            position.x = position.x + Time.deltaTime * (speed-speed02) * direction;
        }
        else
        {
            position.x = position.x + Time.deltaTime * (speed-speed02) * direction;
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

    public void decelerate(float speed02)
    {
        Color newColor = new Color(0.6f,0.7f,0.9f);
        spriteRenderer.color = newColor;
        this.speed02 = speed02;
    }
    public void color()
    {
        Color newColor = new Color(1, 1, 1);
        spriteRenderer.color = newColor;
    }
}
