using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Current health: " + other.gameObject.GetComponent<Player>().currentHealth);
            other.gameObject.GetComponent<Player>().currentHealth += 75f;
            Debug.Log("Added: " + 75f);
            Debug.Log("Now health is: " + other.gameObject.GetComponent<Player>().currentHealth);

            other.gameObject.GetComponent<Player>().cakesKilled++;
            Destroy(gameObject);
        }
    }
}
