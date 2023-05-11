using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kiwi : Fruit
{

    bool notTooClose = true;
    GameObject friend;

    float friendSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        speed = 2.5f;
        health = 1.0f;
        xp = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GoToPlayer(speed);
        MoveTowardsFriend(friend, friendSpeed);
    }

    public void OnTriggerEnter(Collider other)
    {
        // check for kiwi frens and move towards them
        if (other.name == "FriendFinder")
        {
            friendSpeed = 2.5f;
            speed = 0.0f;

            friend = other.gameObject;
            // MoveTowardsFriend(other.gameObject);
            
        }

        // if kiwi 

        // if touching their personal bubble stop movement
        if (other.name == "PersonalBubble")
        {
            speed = 0.5f;
            friendSpeed = 0;

            notTooClose = false;
        }
    }

    public void MoveTowardsFriend(GameObject friend, float frenSpeed)
    {

        /*Debug.Log("Current position" + gameObject.transform.position);
        Debug.Log("Friend position" + friend.transform.position);*/
        if (friend != null)
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, friend.transform.position, Time.deltaTime * frenSpeed);

    }

    public override void GoToPlayer(float speed)
    {
        if (moveEnabled)
        {
            // move towards player at rate fruit speed
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
        }
    }

    // kiwi small and moves quickly
    // slows down when near other kiwis because of kiwi fuzz
    // kiwis clump together

}
