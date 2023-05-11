using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starfruit : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        health = 2.0f;
        speed = 0.0f;
        xp = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        // play nasty sound
        if (health < 0)
        {
            GameObject.Find("Player").GetComponent<PlayerGear>().starCount += 3;

            Destroy(gameObject);
        }
    }

}
