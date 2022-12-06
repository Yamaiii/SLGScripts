using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public delegate void DoNext(NPCBase npc);
public abstract class NPCBase : MonoBehaviour
{
    public List<ShopBase> targetShops;
    public NavMeshAgent navAgent;
    protected bool sw = false;
    protected float dis = 0.5f;
    protected Vector3 tPos;
    protected DoNext next;
    public Animator ani;

    public Transform seat;
    /// <summary>
    /// 记录npc所在队列
    /// </summary>
    public int locationNum;
    protected virtual void Awake(){
        navAgent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
    }
    protected virtual void Start(){

    }

    /// <summary>
    /// 导航到目的地 并 处理接来来要做的事情
    /// </summary>  
    /// <param name="targetPos">导航目的地</param>
    /// <param name="doNext">到达目的地后要做的事</param>
    public virtual void ToTargetAndDoNext(Vector3 targetPos,DoNext doNext){
        ani.SetBool("isWalking",true);
        navAgent.SetDestination(targetPos);
        tPos = targetPos;
        next = doNext;
        dis = 0.5f;
        sw = true;
    }
    public virtual void ToTargetAndDoNext(Vector3 targetPos,DoNext doNext,float distance){
        ani.SetBool("isWalking",true);
        navAgent.SetDestination(targetPos);
        tPos = targetPos;
        next = doNext;
        dis = distance;
        sw = true;
    }

    protected virtual void Update() {
        if(sw)
            CheckIsTarget(tPos,next);
    }

    protected void CheckIsTarget(Vector3 targetPos,DoNext doNext){
        if(Vector3.Distance(transform.position,targetPos)<dis)
        {
            sw = false;
            ani.SetBool("isWalking",false);
            doNext(this);
        }
    }
    public void LeaveShop(){
        targetShops[0].dailyRevenue += targetShops[0].unitPrice;
        targetShops.Remove(targetShops[0]);
        if(targetShops.Count>0)
            ToTargetAndDoNext(targetShops[0].transform.position,targetShops[0].Weclcome);
        else //到达退出点 加入对象池
        {
            ToTargetAndDoNext(SceneManager.manager.spawnPoints[Random.Range(0,SceneManager.manager.spawnPoints.Count)].position,NPCManager.manager.EnterPoll);
        }
    }

}
