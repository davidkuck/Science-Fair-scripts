using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;


public class ObjectTracker: MonoBehaviour
{
    public GameObject[] foodList;
    public GameObject[] agentList;
    public GameObject[] enemyList;

    
    private int currentRound = 0;    
    private float time = 0f;
    private float RoundLength = 10f;

    private string logFilePath = "Assets/simulation_log.txt"; // Path to your log file
    private int[] info = new int[3];


    public FoodSpawner FoodSpawner;
    public Enemy_behaviour enemy;
    public AI_behaviour creature;

    // Initialize the round counter text
    void Start()
    {

        IncrementRound();
    }

    // Update the round counter text
    void UpdateRoundCounter()
    {
        info[0] = foodList.Length;
        info[1] = agentList.Length;
        info[2] = enemyList.Length;
        using (StreamWriter writer = File.AppendText(logFilePath))
        {
            writer.WriteLine("iteraton:" + currentRound);
            writer.WriteLine("Logging data for current round: Food = " + info[0] + " Agents = " + info[1] + " Enemy = " + info[2]);
        }
        
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
            Debug.Log("Logging data for current round: Food = " + info[0] + " Agents = " + info[1] + " Enemy = " + info[2]);
        }
    }

    // Increment the round
    public void IncrementRound()
    {
        currentRound++;
        foodList = GameObject.FindGameObjectsWithTag("Food");
        agentList = GameObject.FindGameObjectsWithTag("Creature");
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(enemyList.Length);
        
        UpdateRoundCounter();
        creature.Reproduce();
        FoodSpawner.FoodSpawn();
        enemy.Reproduce();

    }

    private void FixedUpdate()
    {
        time+=Time.fixedDeltaTime;

        //if time save data point
        if(time >= RoundLength)
        {
            time = 0f;



            IncrementRound();
            
             
        }
    }
}
