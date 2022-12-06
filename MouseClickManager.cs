using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickManager : MonoBehaviour
{
    public enum Model
    {
        General,
        Setting
    }
    public Model model = Model.General;

    bool isDoubleClick = false;
    private void Update()
    {
        if (model == Model.General) {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Camera.main != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f))
                {
                    switch (hit.transform.tag)
                    {
                        case "Customer":
                            UIManager.manager.UpdateInf(hit.transform.GetComponent<NPCBase>().targetShops[0].name);
                            print("Customer");
                            break;
                        case "Shop":
                            if (isDoubleClick)
                            { 
                                model = Model.Setting;
                                UIManager.manager.shopInf.SetActive(false);
                                print("Ë«»÷");
                            }
                            else
                                StartCoroutine("DoubleClick");
                            UIManager.manager.UpdateInf_Shop(hit.transform.GetComponent<ShopBase>().dailyRevenue);
                            print("Shop");
                            break;
                        default:
                            UIManager.manager.npcInf.SetActive(false);
                            UIManager.manager.shopInf.SetActive(false);
                            break;
                    }
                }
            }
        }
    }

    IEnumerator DoubleClick()
    {
        isDoubleClick = true;
        yield return new WaitForSeconds(0.5f);
        isDoubleClick = false;
    }
}
