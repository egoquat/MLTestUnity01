﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;
using System;

public class PenguinArea : Area 
{
    public PenguinAgent penguinAgent;
    public GameObject penguinBaby;
    public PenguinFish prefabFish;
    public TextMeshPro cumulativeRewardText;

    [HideInInspector]
    public float speedFish = 0.0f;

    [HideInInspector]
    public float radiusFeed = 1.0f;

    private List<GameObject> listFish = new List<GameObject>();

    public override void ResetArea()
    {
        RemoveAllFish();
        PlacePenguin();
        PlaceBaby();
        SpawnFish(4, speedFish);
    }

    public void RemoveSpecificFish(GameObject objectFish)
    {
        listFish.Remove(objectFish);
        Destroy(objectFish);
    }

    public static Vector3 ChooseRandomPosition(Vector3 center, float angleMin, float angleMax, float radiusMin, float radiusMax)
    {
        float radius = radiusMin;
        if (radiusMax > radiusMin)
        {
            radius = UnityEngine.Random.Range(radiusMin, radiusMax);
        }

        return center + Quaternion.Euler(0.0f, UnityEngine.Random.Range(angleMin, angleMax), 0.0f) * Vector3.forward * radius;
    }

    private void RemoveAllFish()
    {
        if (null != listFish)
        {
            for (int i = 0; i < listFish.Count; ++i)
            {
                Destroy(listFish[i]);
            }
        }

        listFish = new List<GameObject>();
    }

    private void PlacePenguin()
    {
        penguinAgent.transform.position = ChooseRandomPosition(transform.position, 0.0f, 360.0f, 0.0f, 9.0f) + Vector3.up * 5.0f;
        penguinAgent.transform.rotation = Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
    }

    private void PlaceBaby()
    {
        penguinBaby.transform.position = ChooseRandomPosition(transform.position, -45.0f, 45.0f, 4.0f, 9.0f) + Vector3.up * 5.0f;
        penguinBaby.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }

    private void SpawnFish(int count, float speedFish)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject objectFish = Instantiate<GameObject>(prefabFish.gameObject);
            objectFish.transform.position = ChooseRandomPosition(transform.position, 100.0f, 260.0f, 2.0f, 13.0f) + Vector3.up * 5.0f;
            objectFish.transform.rotation = Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
            objectFish.transform.parent = transform;
            listFish.Add(objectFish);
            objectFish.GetComponent<PenguinFish>().speedFish = speedFish;
        }
    }

    private void Update()
    {
        cumulativeRewardText.text = penguinAgent.GetCumulativeReward().ToString();  
    }
}
