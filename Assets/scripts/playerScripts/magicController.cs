using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicController : MonoBehaviour
{
    public static magicController instance { get; private set; }

    public int maxMagic = 10;
    int currentMagic;
    public int magic { get { return currentMagic; } }

    public bool magicIsValid; // Check if the magic is able to be used

    public magicBarForPlayer magicBar;
    public HealthSystemForDummies magicSystem;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        currentMagic = maxMagic;
        magicIsValid = true;
    }

    public void changeMagic(int amount)
    {
        if (amount > 0 && magicIsValid == false)
        {
            magicIsValid = true;
        }

        if (amount < -1 && currentMagic < 2)
        {
            amount = -1;
        }

        magicSystem.AddToCurrentHealth(amount);
        currentMagic = Mathf.Clamp(currentMagic + amount, 0, maxMagic);

        if (currentMagic <= 0)
        {
            magicIsValid = false;
        }
    }
}
