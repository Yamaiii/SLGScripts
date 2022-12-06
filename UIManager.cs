using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager manager;
    private void Awake()
    {
        manager = this;
    }

    public GameObject npcInf;
    public GameObject shopInf;

    #region NPCINF
    public TMP_Text destination;
    #endregion
    #region ShopInf
    public TMP_Text dailyRevenue;
    #endregion

    public TMP_Text dateData;

    public void UpdateInf(string _destination)
    {
        npcInf.SetActive(true);
        destination.text ="want goint to:" + _destination;
    }

    public void UpdateInf_Shop(int revenue)
    {
        shopInf.SetActive(true);
        dailyRevenue.text = revenue.ToString();
    }

}
