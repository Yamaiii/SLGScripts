using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShopBase : MonoBehaviour
{
    public Transform doorTransform;

    public string shopName;

    public int dailyRevenue = 0;
    public int unitPrice = 10;

    //¸¨Öú×é¼þ
    public GameObject promptPlane;
    public GameObject checkRoad;
    public GameObject checkOtherObj;

    public abstract void Weclcome(NPCBase npc);

}
