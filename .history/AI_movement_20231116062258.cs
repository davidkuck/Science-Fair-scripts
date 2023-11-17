using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_movement : MonoBehaviour
{
    public CharacterController controller;
    private bool hasController = false;
    private Vector3 playerVelocity;
    public float speed = 10.0F;
    public float rotateSpeed = 10.0F;
    public float YMovement = 0;
    public float XMovement = 0;

    private ObjectTracker objectTracker;
    private Creature creature;

    void Awake()
    {
        objectTracker = FindObjectOfType<ObjectTracker>();
        creature = GetComponent<Creature>();
        controller = GetComponent<CharacterController>();
    }

    public void Move(float FB, float LR)
    {
        //clamp the values of X and Y       
        FB = Mathf.Clamp(FB, -0.3, 1);
        LR = Mathf.Clamp(Lr, -1, 1);


        //move the agent
        if (!creature.isDead)
        {
            // Rotate around y - axis
            transform.Rotate(0, LR * rotateSpeed, 0);

            // Move forward / backward
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            controller.SimpleMove(forward * speed * FB * -1);
        }
    }
}
