using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapefruit : Fruit
{

    public GameObject grapefruitSlice;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        health = 2.0f;
        speed = 1.0f;
        xp = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GoToPlayer(speed);
    }

    private void OnTriggerEnter(Collider other)
    {



    }


    public override void ModifyFruitSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public override float GetFruitSpeed()
    {
        return speed;
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        // play nasty sound
        if (health < 0)
        {

            // instantiate dead grapefruit
            Instantiate(grapefruitSlice, transform.position, Quaternion.Euler(0,-45,0));

            Destroy(gameObject);
        }
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
