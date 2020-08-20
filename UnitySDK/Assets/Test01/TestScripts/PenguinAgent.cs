using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class PenguinAgent : Agent 
{
    public GameObject prefabHeart;
    public GameObject prefabRegurgitatedFish;

    private PenguinArea penguinArea;
    private Animator animator;
    private RayPerception3D rayPerception;
    private GameObject baby;

    private bool isFull; // If true, penguin has a full stomach.

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // Convert actions to axis values.
        float forward = vectorAction[0];
        float leftOrRight = 0.0f;
        if (vectorAction[1] == 1.0f)
        {
            leftOrRight = -1.0f;
        }
        else if (vectorAction[1] == 2.0f)
        {
            leftOrRight = 1.0f;
        }

        // Set animator parameters.
        animator.SetFloat("Vertical", forward);
        animator.SetFloat("Horizontal", leftOrRight);

        // Tiny nagative reward every step.
        AddReward(1.0f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        isFull = false;
        penguinArea.ResetArea();
    }

    public override void CollectObservations()
    {
        // Has the penguin eaten.
        AddVectorObs(isFull);

        // Distance to the baby.
        AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));

        // Direction to baby.
        AddVectorObs((baby.transform.position - transform.position).normalized);

        // Direction penguin is facing.
        AddVectorObs(transform.forward);

        // RayPerception (sight)
        // ================================
        // rayDistance : How far to raycast.
        // rayAngles : Angles to raycast (0 is right, 90 is forward, 180 is left).
        // detectableObjects : List of tags which correspond to object types agent can see.
        // startOffset : Starting height offset of ray from center of agent.
        // endOffset : Ending height offset of ray from center of agent.
        float rayDistance = 20.0f;
        float[] rayAngles = { 30.0f, 60.0f, 90.0f, 120.0f, 150.0f };
        string[] detectableObjects = { "PenguinBaby", "PenguinFish", "PenguinWall" };
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0.0f, 0.0f));
    }

    private void Start()
    {
        penguinArea = GetComponentInParent<PenguinArea>();
        baby = penguinArea.penguinBaby;
        animator = GetComponent<Animator>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, baby.transform.position) < penguinArea.radiusFeed)
        {
            // Close enough, try to feed the baby.
            RegurgitateFish();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PenguinFish"))
        {
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("PenguinBaby"))
        {
            // Try to feed the baby.
            RegurgitateFish();
        }
    }

    private void EatFish(GameObject objectFish)
    {
        if (true == isFull)
            return; // Can't eat another fish while full.
        
        isFull = true;

        penguinArea.RemoveSpecificFish(objectFish);
        AddReward(1.0f);
    }

    private void RegurgitateFish()
    {
        if (false == isFull)
            return; // Nothing to regurgitate

        isFull = false;

        // Spawn regurgitated fish.
        GameObject regurgitatedFish = Instantiate<GameObject>(prefabRegurgitatedFish);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.position = baby.transform.position;
        Destroy(regurgitatedFish, 4.0f);

        // Spawn heart.
        GameObject heart = Instantiate<GameObject>(prefabHeart);
        heart.transform.parent = transform.parent;
        heart.transform.position = baby.transform.position + Vector3.up;
        Destroy(heart, 4.0f);

        AddReward(1.0f);
    }
}
