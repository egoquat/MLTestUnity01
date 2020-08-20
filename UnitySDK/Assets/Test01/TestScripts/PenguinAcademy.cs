using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PenguinAcademy : Academy
{
    private PenguinArea[] penguinAreas;

    public override void AcademyReset()
    {
        if (null == penguinAreas)
        {
            penguinAreas = FindObjectsOfType<PenguinArea>();
        }

        for (int i = 0; i < penguinAreas.Length; ++i)
        {
            PenguinArea area = penguinAreas[i];
            area.speedFish = resetParameters["speed_fish"];
            area.radiusFeed = resetParameters["radius_feed"];
            area.ResetArea();
        }
    }
}
