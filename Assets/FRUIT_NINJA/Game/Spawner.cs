using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    int maxFruitCount = 0;


    GameObject map;

    public float spawnDelay = 10.0f;

    public GameObject apple1;
    public GameObject apple2;
    public GameObject apple3;

    public GameObject starfruit;
    public GameObject kiwi;
    public GameObject watermelon;
    public GameObject grapefruit;

    public GameObject cake1;
    public GameObject cake2;

    Vector2 randomPt;

    public Collider[] fruitHit = new Collider[100];

    bool fruitSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
        map = GameObject.Find("Map");
        
        

    }

    private void Update()
    {
        if (!fruitSpawning)
        {
            fruitSpawning = true;
            StartCoroutine(StartSpawningFruit());
            StartCoroutine(StartSpawningCake());
        }
    }

    public IEnumerator StartSpawningFruit()
    {
        // select random pt       

        randomPt = ChooseRandomPt();
        Vector3 spawnPt = new Vector3(randomPt.x, 2.0f, randomPt.y);

        // get size of fruit collider 
        GameObject fruitPrefab = ChooseFruit();

        float fruitRadius = fruitPrefab.GetComponent<CapsuleCollider>().radius;
        //Debug.Log(fruitRadius);

        int reasonableCount = 0;
        //while (Physics.OverlapSphereNonAlloc(spawnPt, fruitRadius, fruitHit, Physics.AllLayers, QueryTriggerInteraction.UseGlobal) > 0 && reasonableCount < 100)
        while (Physics.OverlapSphere(spawnPt, fruitRadius).Length > 0 && reasonableCount < 100)
        {
            //Debug.Log("Found object at spawn point, selecting new spawn point");

            randomPt = ChooseRandomPt();

            spawnPt = new Vector3(randomPt.x, 1.5f, randomPt.y);

            reasonableCount++;
        }
/*        if (reasonableCount > 100)
            yield break;*/

        Vector3 torque = new Vector3(100.0f, 0f, 0f);

        if (maxFruitCount < 50)
        {
            GameObject spawnFruit = Instantiate(fruitPrefab, spawnPt, Quaternion.identity);
            spawnFruit.GetComponent<Rigidbody>().AddRelativeTorque(torque);

            maxFruitCount++;
        }

        yield return new WaitForSeconds(spawnDelay);

        fruitSpawning = false;
    }

    public IEnumerator StartSpawningCake()
    {
        // select random pt       

        randomPt = ChooseRandomPt();
        Vector3 spawnPt = new Vector3(randomPt.x, 1.5f, randomPt.y);

        // get size of fruit collider 
        GameObject fruitPrefab = ChooseCake();

        float fruitRadius = fruitPrefab.GetComponent<CapsuleCollider>().radius;
        //Debug.Log(fruitRadius);

        int reasonableCount = 0;
        //while (Physics.OverlapSphereNonAlloc(spawnPt, fruitRadius, fruitHit, Physics.AllLayers, QueryTriggerInteraction.UseGlobal) > 0 && reasonableCount < 100)
        while (Physics.OverlapSphere(spawnPt, fruitRadius).Length > 0 && reasonableCount < 100)
        {
            //Debug.Log("Found object at spawn point, selecting new spawn point");

            randomPt = ChooseRandomPt();

            spawnPt = new Vector3(randomPt.x, 1.5f, randomPt.y);

            reasonableCount++;
        }
        if (reasonableCount > 100)
            yield break;

        GameObject spawnFruit = Instantiate(fruitPrefab, spawnPt, Quaternion.identity);

        yield return new WaitForSeconds(spawnDelay * 3.0f);
        //yield return new WaitForSeconds(10.0f);

        fruitSpawning = false;
    }



    public Vector2 ChooseRandomPt()
    {
        float posX = Random.Range(-50,50);
        float posZ = Random.Range(-50,50);

        return new Vector2(posX, posZ);
    }

    public GameObject ChooseFruit()
    {
        // select number between 0 and 7
        switch (Random.Range(0, 7))
        {
            case 0:
                return apple1;
            case 1:
                return apple2;
            case 2:
                return apple3;
            case 3:
                return starfruit;
            case 4:
                return kiwi;
            case 5:
                return watermelon;
            case 6:
                return grapefruit;
        }


        return kiwi;
    }


    public GameObject ChooseCake()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
            case 1:
                return cake2;
            case 2:
                return cake1;
        }
        
        return cake2;
    }

}



// make a square

// choose random spot in square to spawn a fruit and cookie/cake

// get square

// choose random x,z coordinate 

// check if fruit is within radius of player, if is, rechoose

// check if fruit spawn will collide with an object, if is, rechoose
// Physics.OverlapSphere (can choose radius by fruit/dessert)





// type of fruit that can spawn connected to player level