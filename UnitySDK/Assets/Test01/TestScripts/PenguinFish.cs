using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinFish : MonoBehaviour 
{
    public float speedFish;

    private float speedRandomized = 0.0f;
    private float timeNextAction = -1.0f;
    private Vector3 positionTarget;

    private void FixedUpdate()
    {
        if (speedFish > 0.0f)
        {
            Swim();
        }
    }

    private void Swim()
    {
        if (Time.fixedTime >= timeNextAction)
        {
            // Randomize the speed.
            speedRandomized = speedFish * UnityEngine.Random.Range(0.5f, 1.5f);

            // Pick a random target.
            positionTarget = PenguinArea.ChooseRandomPosition(transform.parent.position, 100.0f, 260.0f, 2.0f, 13.0f);

            // Rotate toward the target.
            transform.rotation = Quaternion.LookRotation(positionTarget - transform.position, Vector3.up);

            // Calculate the time to get there.
            float timeToGetThere = Vector3.Distance(transform.position, positionTarget) / speedRandomized;
            timeNextAction = Time.fixedTime + timeToGetThere;
        }
        else
        {
            // Make sure that the fish does not swim past the target.
            Vector3 moveVector = speedRandomized * transform.forward * Time.fixedDeltaTime;
            if (moveVector.magnitude <= Vector3.Distance(transform.position, positionTarget))
            {
                transform.position += moveVector;
            }
            else
            {
                transform.position = positionTarget;
                timeNextAction = Time.fixedTime;
            }
        }
    }
}
