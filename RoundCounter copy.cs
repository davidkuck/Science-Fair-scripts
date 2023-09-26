using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoundCounter : MonoBehaviour
{
    public Text roundText;
    private int currentRound = 1;

    // Initialize the round counter text
    void Start()
    {
        UpdateRoundCounter();
    }

    // Update the round counter text
    void UpdateRoundCounter()
    {
        roundText.text = "Iteration: " + currentRound;
    }

    // Increment the round
    public void IncrementRound()
    {
        currentRound++;
        UpdateRoundCounter();
    }
}
