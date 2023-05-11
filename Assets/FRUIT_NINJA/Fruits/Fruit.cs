using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;

    public bool moveEnabled = true;

    public float health;
    public float speed;
    public float xp;
    

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void GoToPlayer(float speed)
    {
        if (moveEnabled)
        {
            // move towards player at rate fruit speed
            Vector3 playerGridPos = new Vector3(player.transform.position.x, 2.0f, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, playerGridPos, Time.deltaTime * speed);
        }
    }

    public virtual void ModifyFruitSpeed(float speed)
    {

    }

    public virtual float GetFruitSpeed()
    {
        return 0;
    }

    public virtual void SetFruitSpeed(float speed)
    {
        speed = this.speed;
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        // play nasty sound
        if (health < 0)
            Destroy(gameObject);
    }


    // apple, various colors, standard 1-shot enemy
    // colors are muted

    // kiwi small and moves quickly
    // slows down when near other kiwis because of kiwi fuzz
    // kiwis clump together

    // melon, tanky, can only be sliced with Katana
    // recognizable and dark green

    // grapefruit, medium, breaks into thrust-only slices
    // orange yellow, bright red inside, stay still and don't move, walls

    // grape purple, shoots glowing red grapes, can be reflected
    // 

    // star fruit, friendly and non hostile
    // the amount of slices put into it yields that many throwing stars

}
