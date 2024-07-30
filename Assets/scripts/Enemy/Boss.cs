using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public static bool isGameOver;

    protected override void Awake()
    {
        base.Awake();
        isGameOver = false;
    }

    public void OnDestroy()
    {
        EnemySystem.AddScore(10);
        isGameOver = true;
        Destroy(this.gameObject);
    }
}
