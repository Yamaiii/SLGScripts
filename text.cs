using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class text : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public float timeGapToSay = 10f;
    public float checkDis = 20f;
    public Vector3 aPoint;
    public Vector3 bPoint;
    int currentPos = 0;
    public Transform playerTf;
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        ToTarget(bPoint);
        currentPos = 0;
        StartCoroutine("Timer");
    }
    private void FixedUpdate()
    {
        if (Vector3.Distance(playerTf.position, transform.position) < checkDis)
        {
            int i =Random.Range(0,10);
            //修改对应的int值播放相应动画
        }
        if (navAgent.remainingDistance < 1f)
        {
            if (currentPos == 0)
            {
                currentPos = 1;
                ToTarget(aPoint);
            }
            else
            {
                ToTarget(bPoint);
                currentPos = 0;
            }
        }
    }
    public void ToTarget(Vector3 targetPos)
    {
        navAgent.SetDestination(targetPos);
    }

    IEnumerator Timer()
    {
        while (true)
        {
            navAgent.isStopped = false;
            yield return new WaitForSeconds(timeGapToSay);
            navAgent.isStopped = true;
            //说话:
            //调用UI
        }
    }
}
