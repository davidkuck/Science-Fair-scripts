using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crab_Behavior : MonoBehaviour
{
    public int numRays = 36; // Number of rays to cast in a circle
    public float maxRayDistance = 10f;
    public float[] distances = new float[36];
    public string foodTag = "Food"; // Tag of the player object
    private Color hitPlayerColor = Color.red;
    private Color hitOtherColor = Color.green;

    public Transform healthBar; // Reference to the child object's transform
    public float maxHealth = 100f; // Maximum health value
    public float currentHealth = 100f; // Current health value
    public int attackAmount;
    private float previousHealth; 
    private Vector3 initialScaleHealth; // Initial scale of the health bar

    public bool canEat = true;
    public bool mutateMutations = true;
    public float FB = 0;
    public float LR = 0;
    public int numberOfChildren = 1;
    private bool isMutated = false;
    float elapsed = 0f;
    public float lifeSpan = 0f;
    public bool isDead = false;

    public Transform energyBar; // Reference to the child object's transform
    public float maxEnergy = 100f; // Maximum health value
    public float currentEnergy = 100f; // Current health value
    private float energyGained = 25f;
    private float previousEnergy; 
    private Vector3 initialScaleEnergy; // Initial scale of the health bar
    

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

        // Get the initial scale of the bars
        initialScaleHealth = healthBar.localScale;
        initialScaleEnergy = energyBar.localScale;

        nn = gameObject.GetComponent<NN>();
        movement = gameObject.GetComponent<Movement>();
        distances = new float[numRays];

        this.name = "Agent";

        // Set the initial size based on the current health
        UpdateSizeEnergy();

        hostCollider = GetComponent<Collider2D>(); // Assuming the collider is attached to the same GameObject

    }
    void FixedUpdate()
    {
        
        
        // Check if the energy has changed
        if (currentEnergy != previousEnergy)
        {
            Debug.Log($"it works");
            // Update the size of the health bar
            UpdateSizeEnergy();

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

        movement.Move(FB, LR;)
    }

    void Rays_update()
    {
        for (int i = 0; i < numRays; i++)
        {
            float angle = i * 360f / numRays;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

            Vector3 rayStart = hostCollider.bounds.center + (Vector3)direction * (hostCollider.bounds.extents.x + 0.01f);
            RaycastHit2D hit = Physics2D.Raycast(rayStart, direction, maxRayDistance);

            

            Vector3 rayEnd = hit.collider ? hit.point : (Vector2)rayStart + direction * maxRayDistance;

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag(foodTag))
                {
                    Debug.DrawLine(rayStart, rayEnd, hitPlayerColor);
                    // Use the length of the raycast as the distance to the food object
                    distances[i] = hit.distance / viewDistance;
                }
                else
                {
                    Debug.DrawLine(rayStart, rayEnd, hitOtherColor);
                    distances[i] = maxRayDistance;
                }
            }
            else
            {
                Debug.DrawLine(rayStart, rayEnd, Color.white);
                distances[i] = maxRayDistance; 
            }
        }
    }


    private void OnTriggerEnter2D(Collision2D col)
    {
        // Check if the collision involves the object you want to decrease the variable.
        if (collision.gameObject.CompareTag(foodTag) && canEat))
        {
            Debug.Log("it works");
            currentEnergy += energyGained;
            
            Destroy(col.gameObject);

            // Ensure the variable doesn't go below zero.
            if (currentEnergy < 0)
            {
                currentEnergy = 0;

                
            }

            // Calculate the new size based on the current health
            float newSizeHealth = Mathf.Clamp01(currentEnergy / maxEnergy);

            // Apply the new size to the health bar's scale, taking initial scale into account
            energyBar.localScale = new Vector3(initialScaleHealth.x * newSize, initialScaleEnergy.y, initialScaleEnergy.z);            }
    
    }
    public void ManageEnergy()
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
            this.transform.Rotate(0, 0, 180);
            Destroy(this.gameObject, 3);
            GetComponent<Movement>().enabled = false;
        }
    }
    }
    // Method to update the health and size externally
    public void SetHealth(float newHealth)
    {
        currentHealth = Mathf.Clamp(newHealth, maxHealth, 0f);
        // Calculate the new size based on the current health
        float newSize = Mathf.Clamp01(currentHealth / maxHealth);

        // Apply the new size to the health bar's scale, taking initial scale into account
        healthBar.localScale = new Vector3(initialScaleHealth.x * newSize, initialScaleHealth.y, initialScaleHealth.z);
    }
       // Update the size of the child object based on the current health
    private void UpdateSizeEnergy()
    {
        // Calculate the new size based on the current health
        float newSize = Mathf.Clamp01(currentEnergy / maxEnergy);

        // Apply the new size to the health bar's scale, taking initial scale into account
        energyBar.localScale = new Vector3(initialScaleEnergy.x * newSize, initialScaleEnergy.y, initialScaleEnergy.z);
    }

    // Method to update the health and size externally
    public void SetEnergy(float newEnergy)
    {
        currentEnergy = Mathf.Clamp(newEnergy, maxEnergy, 0f);
        UpdateSizeEnergy();
    }

}

