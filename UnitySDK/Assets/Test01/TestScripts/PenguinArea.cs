using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;
using System;
using JetBrains.Annotations;

public class PenguinArea : Area
{
    public PenguinAgent penguinAgent;
    public GameObject penguinBaby;
    public PenguinFish prefabFish;
    public TextMeshPro cumulativeRewardText;

    [HideInInspector]
    public float speedFish = 0.0f;
    
    [HideInInspector]
    public float radiusFeed = 0.0f;

    private List<GameObject> listFish = new List<GameObject>();

    public override void ResetArea()
    {
        RemoveAllFish();
        PlacePenguin();
        PlaceBaby();
        SpawnFishes(4, speedFish);
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

        return center + ((Quaternion.Euler(0.0f, UnityEngine.Random.Range(angleMin, angleMax), 0.0f) * Vector3.forward) * radius);
    }

    private void RemoveAllFish()
    {
        if (null == listFish)
            return;

        for(int i = 0; i < listFish.Count; ++i)
        {
            GameObject fish = listFish[i];
            if (null == fish)
                continue;
            Destroy(fish);
        }

        listFish = new List<GameObject>();
    }

    private void PlacePenguin()
    {
        penguinAgent.transform.position = ChooseRandomPosition(transform.position, 0.0f, 360.0f, 0.0f, 9.0f) + Vector3.up * 0.5f;
        penguinAgent.transform.rotation = Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
        
    }

    private void PlaceBaby()
    {
        penguinBaby.transform.position = ChooseRandomPosition(transform.position, -45.0f, 45.0f, 4.0f, 9.0f) + Vector3.up * 0.5f;
        penguinBaby.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }

    private void SpawnFishes(int count, float speedFish)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject newFish = Instantiate<GameObject>(prefabFish.gameObject);
            newFish.transform.position = ChooseRandomPosition(transform.position, 100f, 260.0f, 2.0f, 13.0f) + Vector3.up * 0.5f;
            newFish.transform.rotation = Quaternion.Euler(0.0f, UnityEngine.Random.Range(0.0f, 360.0f), 0.0f);
            newFish.transform.parent = transform;
            newFish.GetComponent<PenguinFish>().speedFish = speedFish;
            listFish.Add(newFish);
        }
    }

    private void Update()
    {
        cumulativeRewardText.text = penguinAgent.GetCumulativeReward().ToString("0.00");
    }
}
