using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCounter : MonoBehaviour
{
    public GameObject[] foodList;
    public GameObject[] agentList;
    public GameObject[] enemyList;
    List<float> foodData = new List<float>();
    List<float> agentData = new List<float>();
    List<float> enemyData = new List<float>();
    
    private int currentRound = 1;    
    float time = 0;

    // Initialize the round counter text
    void Start()
    {
        UpdateRoundCounter();
    }

    // Update the round counter text
    void UpdateRoundCounter()
    {
        Debug.Log("Iteration: " + currentRound); 
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
            Data = "";
            // Log data or perform actions based on the value of currentRound
            Debug.Log("Logging data for current round: Food = " + foodData + "Agents = " + agentData + "Enemy = " + enemyData);
        }
    }

    // Increment the round
    public void IncrementRound()
    {
        currentRound++;
        UpdateRoundCounter();
        AI_Behavior.Reproduce();
        FoodSpawner.FoodSpawn();
    }

    private void FixedUpdate()
    {
        time+=Time.fixedDeltaTime;
        foodList = GameObject.FindGameObjectsWithTag("Food");
        agentList = GameObject.FindGameObjectsWithTag("Agent");
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        //if time save data point
        if(time > 100)
        {
            
            foodData.Add(foodList.Length);
            agentData.Add(agentList.Length);
            enemyData.Add(enemyList.Length);
            IncrementRound();
            
            time = 0;
        }
    }
}
