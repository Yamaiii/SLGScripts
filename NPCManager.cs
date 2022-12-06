using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AI;

public class NPCManager : MonoBehaviour
{
    public static NPCManager manager;
    private void Awake() {
        if(manager!=null)
            Destroy(gameObject);
        else
            manager = this;
    }

    bool isCreat = true;

    private void Start() {
        TimeManager.manager.OnOpen += StartCreat;
        TimeManager.manager.OnClose += EndCreat;
        amplitued_efficiency = efficiency/3.0f;
        Addressables.LoadAssetsAsync<GameObject>(customerLabel,null).Completed += OnResourcesRetrieved;
        //异步加载顾客prefab
        //customer.LoadAssetAsync<GameObject>().Completed += (prefab)=>{
        //    if(prefab.Status.Equals(AsyncOperationStatus.Succeeded))
        //    {
        //        StartCoroutine(CreatCustomer(efficiency,amplitued_efficiency));
        //    }
        //};
    }

    private void OnResourcesRetrieved(AsyncOperationHandle<IList<GameObject>> obj)
    {
        customers = obj.Result;
        StartCoroutine(CreatCustomer(efficiency,amplitued_efficiency));
    }

    //tips: 用ab加载并且随机穿搭 避免人物过分一致
    public AssetLabelReference customerLabel;
    public IList<GameObject> customers;
    #region 顾客生成相关系数
    [Header("顾客生成间隔")]
    public float efficiency; //顾客生成效率  ： 具体体现（2个npc生成间隔
    [Tooltip("顾客生成间隔变化幅度")]
    private float amplitued_efficiency;
    public List<NPCBase> customerPool; // 顾客对象池
    #endregion

    IEnumerator CreatCustomer(float eff,float am_eff){
        //tips: 创建条件： 上班时间 晚上为升级改造时间
        while(isCreat)
        {
            Creat();
            yield return new WaitForSeconds(Random.Range(eff-am_eff,eff+am_eff));
        }
    }

    void Creat(){
        Transform creatPos = SceneManager.manager.spawnPoints[Random.Range(0,SceneManager.manager.spawnPoints.Count)];
        if(customerPool.Count!=0)
        {
            customerPool[0].gameObject.SetActive(true);
            customerPool[0].transform.position = creatPos.position;
            customerPool[0].GetComponent<NavMeshAgent>().enabled = true;
            customerPool[0].gameObject.GetComponent<Customer>().InitialiazeCustomer();
            customerPool.Remove(customerPool[0]);
        }
        else
        {
            //customer.InstantiateAsync(creatPos.position,Quaternion.identity);
            //customers[Random.Range(0,customers.Count)]
            Instantiate(customers[Random.Range(0, customers.Count)], creatPos.position,Quaternion.identity);
        }
    }
    public void EnterPoll(NPCBase npc){
        npc.GetComponent<NavMeshAgent>().enabled = false;
        npc.gameObject.SetActive(false);
        customerPool.Add(npc);
    }

    void StartCreat()
    {
        isCreat = true;
        StartCoroutine(CreatCustomer(efficiency, amplitued_efficiency));
        print("开门营业");
    }

    void EndCreat()
    {
        isCreat=false;
        print("关门歇业");
    }
}
