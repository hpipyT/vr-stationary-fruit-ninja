using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : Fruit
{

    void Start()
    {
        player = GameObject.Find("Player");
        health = 2.0f;
        speed = 1.0f;
        xp = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GoToPlayer(speed);
    }


    public override void ModifyFruitSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public override float GetFruitSpeed()
    {
        return speed;
    }

    public override void GoToPlayer(float speed)
    {
        if (moveEnabled)
        {
            Vector3 playerGridPos = new Vector3(player.transform.position.x, 2.0f, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, playerGridPos, Time.deltaTime * speed);
        }
    }
}
