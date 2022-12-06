using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : NPCBase
{

    public int consumptionAbility = 2; // 消费能力

    int[] range; //随机数去重数组

    protected override void Start(){
        base.Start();
        InitialiazeCustomer();
    }
    public void InitialiazeCustomer(){
        range = new int[ShopManager.manager.shops.Count];
        for(int i=0;i<range.Length;i++)
            range[i] = i;
        consumptionAbility = Mathf.Min(consumptionAbility,ShopManager.manager.shops.Count);

        StartCoroutine("InitialiazeTarget",Random.Range(1,consumptionAbility+1));
        ToTargetAndDoNext(targetShops[0].doorTransform.position,targetShops[0].Weclcome);
    }
    /// <summary>
    /// 初始化顾客目标
    /// </summary>
    /// <param name="count">顾客想要去的店的数量</param>
    /// <returns>1s执行一次</returns>
    IEnumerator InitialiazeTarget(int count){
        for(int i =0;i<count;i++)
        {
            int sub = Random.Range(0,range.Length-i);
            targetShops.Add(ShopManager.manager.shops[range[sub]]);
            range[sub] = range[range.Length-i-1]; //随机数去重
            yield return new WaitForSeconds(1.0f); //缓冲 1s  顾客不可能1秒内到店里然后完事
        }
    }


}
