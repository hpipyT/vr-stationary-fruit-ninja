using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapefruitSlice : Fruit
{
    public GameObject sword;
    Sword script;

    // Start is called before the first frame update
    void Start()
    {
        sword = GameObject.Find("Player/Camera Offset/RightHand Controller/sword");
        script = sword.GetComponent<Sword>();
        player = GameObject.Find("Player");
        health = 2.0f;
        speed = 0.0f;
        xp = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
/*
    private void OnTriggerEnter(Collider other)
    {
        // get player script

        if (script.isThrusting == true)
        {
            Debug.Log("Stabbed the slice");
            Destroy(gameObject);
        }

        // if player is thrusting

        // take damage
        // else dont
    }*/

/*    public override void TakeDamage(int damage)
    {
        if (script.IsThrusting(script.right) == true)
        {
            Debug.Log("Stabbed the slice");
            Destroy(gameObject);
        }
    }*/

}
