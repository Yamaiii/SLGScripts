using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Eatery : ShopBase
{
    #region 排队算法相关属性
    protected int windowNum = 1;
    public int queueNumber = 6;
    public float numberDis = 1.1f;
    public List<Transform> window;
    public List<List<Transform>> pickupQueue=new List<List<Transform>>();
    public float pickupMealsTime = 2.0f;
    #endregion

    #region 就餐算法相关属性
    public List<Transform> seats;
    public float dineTime;
    #endregion

    private void Start() {
        windowNum = window.Count;
        for(int i=0;i<windowNum;i++)
        {
            List<Transform> temp = new List<Transform>();
            temp.Add(window[i]);
            pickupQueue.Add(temp);
        }
    }

    /// <summary>
    /// 顾客到店后调用
    /// </summary>
    public override void Weclcome(NPCBase npc)
    {
        EmplaceQueue(npc);
    }

    void EmplaceQueue(NPCBase npc){
        int No=0;
        int minCount = pickupQueue[0].Count;
        for(int i=1;i<windowNum;i++)
        {
            if(minCount>pickupQueue[i].Count)
            {
                minCount = pickupQueue[i].Count;
                No = i;
            }
        }
        if(minCount==1) //队头（前边没人
        {
            EmplaceQueueHead(npc,No);
        }
        else //前边有人，接着排
        {
            EmplaceQueueNext(npc,No,minCount);
        }
        pickupQueue[No].Add(npc.transform);
        npc.locationNum = No;
    }

    void EmplaceQueueHead(NPCBase npc,int No){
        npc.ToTargetAndDoNext(pickupQueue[No][0].position,Pickup);
    }
    void EmplaceQueueNext(NPCBase npc,int No,int minCount){
        Vector3 dir = pickupQueue[No][0].forward;
        Vector3 pos = pickupQueue[No][0].position -minCount*numberDis*dir;
        npc.navAgent.SetDestination(pos);
    }

    void Pickup(NPCBase npc){
        StartCoroutine("PickupMeals",npc);
    }

    IEnumerator PickupMeals(NPCBase npc){
        yield return new WaitForSeconds(pickupMealsTime);
        npc.seat = seats[Random.Range(0,seats.Count-1)];
        seats.Remove(npc.seat);
        //Tips: 拿上食物
        npc.ToTargetAndDoNext(npc.seat.position,Dine,2f);

        //离开队列，更新队列
        pickupQueue[npc.locationNum].Remove(npc.transform);
        yield return new WaitForSeconds(0.2f);
        if(pickupQueue[npc.locationNum].Count>1)
        {
            EmplaceQueueHead(pickupQueue[npc.locationNum][1].GetComponent<NPCBase>(),npc.locationNum); //第二位上前取餐
            if(pickupQueue[npc.locationNum].Count>2)
                for(int i=2;i<pickupQueue[npc.locationNum].Count;i++) //遍历剩下的队伍的人，上前一个位置
                {
                    yield return new WaitForSeconds(0.2f);
                    EmplaceQueueNext(pickupQueue[npc.locationNum][i].GetComponent<NPCBase>(),npc.locationNum,i-1);
                }
        }
    }

    void Dine(NPCBase npc){
        npc.GetComponent<NavMeshAgent>().enabled = false;
        npc.transform.position = npc.seat.position;
        Vector3 dir = npc.seat.forward;
        Vector3 pos = npc.seat.position -1f*dir;
        // Quaternion q = Quaternion.LookRotation(pos);
        npc.transform.LookAt(pos);
        StartCoroutine("Dining",npc);
    }

    IEnumerator Dining(NPCBase npc){
        npc.GetComponent<Animator>().SetBool("isDining",true);
        yield return new WaitForSeconds(dineTime);
        seats.Add(npc.seat);
        npc.GetComponent<NavMeshAgent>().enabled = true;
        npc.GetComponent<Animator>().SetBool("isDining",false);
        npc.LeaveShop();
    }

}
