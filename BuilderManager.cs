using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuilderManager : MonoBehaviour
{
    public static BuilderManager manager;
    private void Awake()
    {
        manager = this;
        navMeshSurface = GetComponent<NavMeshSurface>();
        mainCamera = Camera.main;
    }

    public Camera buildCamera;
    public Camera mainCamera;

    private GameObject buildPrefab=null;
    public bool isCloseRoad = false;
    public bool hasOtherObj = false;

    public Material mat_True;
    public Material mat_False;

    NavMeshSurface navMeshSurface;

    private void Update()
    {
        if (buildPrefab != null)
            MovePrefab();
    }

    private void FixedUpdate()
    {

    }

    public void SwitchBuildMod(GameObject shopList)
    {
        shopList.SetActive(!shopList.activeInHierarchy);
        mainCamera.gameObject.SetActive(buildCamera.gameObject.activeInHierarchy);
        buildCamera.gameObject.SetActive(!buildCamera.gameObject.activeInHierarchy);
    }

    public void StartBulidProcess(GameObject prefab)
    {
        buildPrefab = prefab;
    }

    void MovePrefab()
    {
        RaycastHit hit;
        //移动
        if (Physics.Raycast(buildCamera.ScreenPointToRay(Input.mousePosition),out hit,1000, 1<<6))
        {
            buildPrefab.transform.position = hit.point;
        }
        //旋转
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("turn");
            buildPrefab.transform.RotateAround(hit.point,Vector3.up,90);
        }
        //判断 （是否碰撞其他物体）（是否靠近道路）
        if (!hasOtherObj && isCloseRoad)
        {
            buildPrefab.GetComponent<ShopBase>().promptPlane.GetComponent<MeshRenderer>().material = mat_True;
            if (Input.GetMouseButtonDown(0))
            {
                Placement(buildPrefab);
                buildPrefab = null;
            }
        }
        else
        {
            buildPrefab.GetComponent<ShopBase>().promptPlane.GetComponent<MeshRenderer>().material = mat_False;
        }
    }

    void Placement(GameObject obj)
    {
        ShopManager.manager.shops.Add(obj.GetComponent<ShopBase>()); // 添加到商店列表，对客户打开
        navMeshSurface.BuildNavMesh(); // 烘焙navmesh；
        //给物体加上触发器占位
        obj.AddComponent<BoxCollider>().center = obj.GetComponent<ShopBase>().checkOtherObj.GetComponent<BoxCollider>().center;
        obj.GetComponent<BoxCollider>().size = obj.GetComponent<ShopBase>().checkOtherObj.GetComponent<BoxCollider>().size;
        obj.GetComponent<BoxCollider>().isTrigger = true;
        //删除辅助脚本和组件
        Destroy(obj.GetComponent<ShopBase>().checkOtherObj);
        Destroy(obj.GetComponent<ShopBase>().checkRoad);
        Destroy(obj.GetComponent<ShopBase>().promptPlane);
        //还原辅助检测bool
        hasOtherObj = false;
        isCloseRoad = false;
    }
}
