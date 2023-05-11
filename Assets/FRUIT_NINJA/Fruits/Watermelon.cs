using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : Fruit
{
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        health = 5.0f;
        speed = 0.25f;
        xp = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GoToPlayer(speed);
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
