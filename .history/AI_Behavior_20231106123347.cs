using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Behavior : MonoBehaviour
{
    public int numRays = 36; // Number of rays to cast in a circle
    public float maxRayDistance = 10f;
    public float[] distances = new float[36];
    public string foodTag = "Food"; // Tag of the player object
    private Color hitPlayerColor = Color.red;
    private Color hitFoodColor = Color.green;
    private Color hitNothingColor = Color.grey;


    public bool canEat = true;
    public bool mutateMutations = true;
    public float YMovement = 0;
    public float XMovement  = 0;
    public int numberOfChildren = 1;
    private bool isMutated = false;
    float elapsed = 0f;
    public float lifeSpan = 0f;
    public bool isDead = false;

    public float maxHealth = 100f; // Maximum health value
    public float currentHealth = 100f; // Current health value
    public int attackAmount;
    private float previousHealth; 

    public float maxEnergy = 100f; // Maximum health value
    public float currentEnergy = 100f; // Current health value
    private float energyGained = 25f;
    private float previousEnergy; 
    

    public float mutationAmount = 0.8f;
    public float mutationChance = 0.2f; 
    public NN nn;
    public Movement movement;


    private Collider2D hostCollider; // Reference to the host object's collider
    void Awake()
    {
        
        // Initialize previousHealth to see if health changed at all
        previousHealth = currentHealth;
        previousEnergy = currentEnergy;


        nn = gameObject.GetComponent<NN>();
        movement = gameObject.GetComponent<Movement>();
        distances = new float[numRays];

        this.name = "Agent";


        hostCollider = GetComponent<Collider2D>(); // Assuming the collider is attached to the same GameObject

    }
    void FixedUpdate()
    {
        
        
        // Check if the energy has changed
        if (currentEnergy != previousEnergy)
        {
            // Update the previous health value
            previousEnergy = currentEnergy;
        }
        //only do this once
        if(!isMutated)
        {
            //call mutate function to mutate the neural network
            MutateCreature();
            this.transform.localScale = new Vector3(size,size,1); // Z scale is set to 1 for 2D
            isMutated = true;
            energy = maxEnergy;
        }

        ManageEnergy();

        Rays_update();

        float[] inputsToNN = distances;

        float[] outputsFromNN = nn.Brian(inputsToNN);

        FB = outputsFromNN[0];
        LR = outputsFromNN[1];

        movement.Move(FB, LR);
    }



    private void OnTriggerEnter2D(Collision2D col)
    {
        // Check if the collision involves the object you want to decrease the variable.
        if (collision.gameObject.CompareTag(foodTag) && canEat)
        {
            Debug.Log("it works");
            currentEnergy += energyGained;
            
            Destroy(col.gameObject);

            // Ensure the variable doesn't go below zero.
            if (currentEnergy < 0)
            {
                currentEnergy = 0;

                
            }
        }
    
    }

    void Rays_update()
    {
    // Define the number of object classes, e.g., food and player
    int numObjectClasses = 2;

    for (int i = 0; i < numRays; i++)
    {
        float angle = i * 360f / numRays;
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

        Vector3 rayStart = hostCollider.bounds.center + (Vector3)direction * (hostCollider.bounds.extents.x + 0.01f);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, maxRayDistance);

        Vector3 rayEnd = hit.collider ? hit.point : (Vector2)rayStart + direction * maxRayDistance;

        double[] inputVector = new double[1 + numObjectClasses]; // Input vector

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag(foodTag))
            {
                Debug.DrawLine(rayStart, rayEnd, hitFoodColor);
                // Use the length of the raycast as the distance to the food object
                inputVector[0] = hit.distance / maxRayDistance; // Distance
                inputVector[1] = 1.0; // Food identification
            }
            else
            {
                Debug.DrawLine(rayStart, rayEnd, hitPlayerColor);

                inputVector[0] = -hit.distance / maxRayDistance; // Distance
                inputVector[2] = 1.0; // Player identification
            }
        }
        else
        {
            Debug.DrawLine(rayStart, rayEnd, Color.white);
            inputVector[0] = 1.0; // Maximum distance (normalized)
        }

        // Store the input vector for this ray in the 'inputVectors' list or array.
        // You can use a list or array to collect all input vectors for later processing with your neural network.
        inputVectors[i] = inputVector;
    }
    }

    void ManageEnergy()
    {
        elapsed += Time.deltaTime;
        lifeSpan += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;

            // Subtract 1 energy per second
            energy -= 1f;
            

        }

        // Starve
        if (energy <= 0)
        {           
            Destroy(this.gameObject,3); 
            GetComponent<Movement>().enabled = false;
        }
    }
    void MutateCreature()
    {
        if(mutateMutations)
        {
            mutationAmount += Random.Range(-1.0f, 1.0f)/100;
            mutationChance += Random.Range(-1.0f, 1.0f)/100;
        }

        //make sure mutation amount and chance are positive using max function
        mutationAmount = Mathf.Max(mutationAmount, 0);
        mutationChance = Mathf.Max(mutationChance, 0);

        nn.MutateNetwork(mutationAmount, mutationChance);
    }
    void Reproduce()
    {
        if (currentEnergy >= 25f)
        {
            numberOfChildren = 2;
        }
            
        else
        {
            numberOfChildren = 1;
        }
            
        //replicate
        for (int i = 0; i< numberOfChildren; i ++) // I left this here so I could possibly change the number of children a parent has at a time.
        {
            //create a new agent, and set its position to the parent's position + a random offset in the x and z directions (so they don't all spawn on top of each other)
            GameObject child = Instantiate(agentPrefab, new Vector3(
                (float)this.transform.position.x + Random.Range(-10, 11), 
                0, 
                (float)this.transform.position.z+ Random.Range(-10, 11)), 
                Quaternion.identity);
            
            //copy the parent's neural network to the child
            child.GetComponent<NN>().layers = GetComponent<NN>().copyLayers();
        }
    Destroy(this.gameObject,3); 
    GetComponent<Movement>().enabled = false;

    }
}
