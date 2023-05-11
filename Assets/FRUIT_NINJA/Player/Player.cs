using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float ultimate;
    public int cakesKilled;

    Image healthBar;

    TMP_Text stars;
    TMP_Text time;
    TMP_Text cakes;

    TMP_Text youLoseScreen;
    TMP_Text youWinScreen;

    public float counterSeconds = 0;



    

    void Awake()
    {
        cakesKilled = 0;
        maxHealth = 100;
        currentHealth = maxHealth;
        healthBar = GameObject.Find("Player/HUD/HP/PlayerHealth").GetComponent<Image>();

        stars = GameObject.Find("Player/HUD/Stars").GetComponent<TMP_Text>();
        time = GameObject.Find("Player/HUD/TimePassed").GetComponent<TMP_Text>();
        cakes = GameObject.Find("Player/HUD/CakesDisplay/CakesCollected").GetComponent<TMP_Text>();
        youLoseScreen = GameObject.Find("Player/HUD/WinLose/Lose").GetComponent<TMP_Text>();
        youWinScreen = GameObject.Find("Player/HUD/WinLose/Win").GetComponent<TMP_Text>();



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth > 100)
            currentHealth = 100f;

        currentHealth -= Time.deltaTime * 2.5f;

        healthBar.fillAmount = currentHealth / maxHealth;

        int currentStars = GameObject.Find("Player").GetComponent<PlayerGear>().starCount;

        stars.text = currentStars.ToString();
        time.text = (Mathf.Floor(counterSeconds * 0.1f)).ToString();
        cakes.text = cakesKilled.ToString();
        counterSeconds += 1;

        if(currentHealth <= 0)
        {
            Time.timeScale = 0;
            youLoseScreen.gameObject.SetActive(true);
            stars.gameObject.SetActive(false);
            // time.gameObject.SetActive(false);

        }

        if (cakesKilled >= 10)
        {
            Time.timeScale = 0;
            youWinScreen.gameObject.SetActive(true);
        }
        
    }
}

// health drains over time
// refill with dessert


// 'level up' meter increases with fruit slain

// level adds abilities 

// 1 Throwing Stars
// Apples, starfruit

// 2 Super Jump
// Grapefruit

// 3 Grapple enemies 
// Watermelon

// 4 Flight
// kiwis

// 5 Big Sword
// Grapes




// ToDo

// Game
// fruit Spawner
// cake Spawner

// Fruits
// 
// 
// Starfruit break

// Player
// 
// 
// slay fruit, gather cake points script
// 
// slice *
// 
// 
// 
// Moldy meter 
