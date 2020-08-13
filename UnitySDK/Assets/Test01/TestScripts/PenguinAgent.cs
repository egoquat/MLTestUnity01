using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class PenguinAgent : Agent
{
    public GameObject prefabHeart;
    public GameObject prefabRegurgiatatedFish;

    private PenguinArea penguinArea;
    private Animator animator;
    private RayPerception3D rayPerception;
    private GameObject baby;

    private bool isFull;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
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

        animator.SetFloat("Vertical", forward);
        animator.SetFloat("Horizontal", leftOrRight);

        AddReward(-1.0f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        isFull = false;
        penguinArea.ResetArea();
    }

    public override void CollectObservations()
    {
        AddVectorObs(isFull);
        AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));
        AddVectorObs((baby.transform.position - transform.position).normalized);
        AddVectorObs(transform.forward);

        // Ray Perception sight
        float rayDistance = 20.0f;
        float[] rayAngles = { 30.0f, 60.0f, 90.0f, 120.0f, 150.0f };
        string[] detectableObjects = { "baby", "penguinFish", "wall" };
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
            RegurgitateFish();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("penguinFish"))
        {
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("baby"))
        {
            RegurgitateFish();
        }
    }

    private void EatFish(GameObject objectFish)
    {
        if (true == isFull)
            return;

        isFull = true;
        penguinArea.RemoveSpecificFish(objectFish);
        AddReward(1.0f);
    }

    private void RegurgitateFish()
    {
        if (false == isFull)
            return;

        isFull = false;
        GameObject regurgitateFish = Instantiate<GameObject>(prefabRegurgiatatedFish);
        regurgitateFish.transform.parent = transform.parent;
        regurgitateFish.transform.rotation = baby.transform.rotation;
        Destroy(regurgitateFish, 4.0f);

        GameObject heart =  ;
    }
}
