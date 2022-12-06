using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevenueManager : MonoBehaviour
{
    public static RevenueManager manager;
    private void Awake()
    {
        manager = this;
    }

    public int dailyRevenue_Sum;
}
