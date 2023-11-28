using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracker: MonoBehaviour
{
    public GameObject[] foodList;
    public GameObject[] agentList;
    public GameObject[] enemyList;
    private int creatureCount = 0;
    private int foodCount = 0;
    private int enemyCount = 0;
    
    private int currentRound = 1;    
    private float time = 0f;
    private float RoundLength = 10f;



    public AI_behaviour creature;
    public FoodSpawner FoodSpawner;
    // Initialize the round counter text
    void Start()
    {
        UpdateRoundCounter();
    }

    // Update the round counter text
    void UpdateRoundCounter()
    {
        
        if (currentRound == 1 ||
            currentRound == 10 ||
            currentRound == 100 ||
            currentRound == 250 ||
            currentRound == 500 ||
            currentRound == 750 ||
            currentRound == 1000 ||
            currentRound == 2500 ||
            currentRound == 5000 ||
            currentRound == 7500 ||
            currentRound == 10000 ||
            currentRound == 20000)
        {
            Debug.Log("Iteration: " + currentRound); 
            // Log data or perform actions based on the value of currentRound
            Debug.Log("Logging data for current round: Food = " + foodCount + "Agents = " + creatureCount + "Enemy = " + enemyCount);
        }
    }

    // Increment the round
    public void IncrementRound()
    {
        currentRound++;
        UpdateRoundCounter();
        creature.Reproduce(ref creatureCount);

        FoodSpawner.FoodSpawn(ref foodCount);
    }

    private void FixedUpdate()
    {
        time+=Time.fixedDeltaTime;

        //if time save data point
        if(time >= RoundLength)
        {
            time = 0f;


            foodList = GameObject.FindGameObjectsWithTag("Food");
            agentList = GameObject.FindGameObjectsWithTag("Creature");
            enemyList = GameObject.FindGameObjectsWithTag("Enemy");
            IncrementRound();
            
             
        }
    }
}
