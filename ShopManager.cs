using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager manager;
    private void Awake() {
        if(manager!=null)
            Destroy(gameObject);
        else
            manager = this;
    }

    public List<ShopBase> shops;
}
