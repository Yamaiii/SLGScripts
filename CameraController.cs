using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5f;
    public float scrollSpeed = 1;

    private Transform m_transform;
    Camera m_camera;

    public float zoomSpeed = 20f;
    public float rotaYSpeed = 5f;

    void Start()
    {
        m_transform = transform;
        m_camera = Camera.main;
    }
    void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X");

        if (Input.GetMouseButton(2))
        {
            //绕世界坐标中的y轴旋转
            transform.Rotate(Vector3.up * x * rotaYSpeed, Space.World);
        }
        if (m_camera.gameObject.activeInHierarchy&&Input.GetKey(KeyCode.LeftControl))
            RotaTheCamera();
        else
            MoveTheCamera();
    }

    private void MoveTheCamera()
    {
        #region 控制镜头前后左右移动
        if (Input.GetAxis("Vertical") > 0 || Input.mousePosition.y > Screen.height - 10)
        {
            m_transform.Translate(Vector3.forward*speed*Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") < 0 || Input.mousePosition.y < 10)
        {
            m_transform.Translate(Vector3.back * speed*Time.deltaTime);
        }
        if (Input.GetAxis("Horizontal") < 0 || Input.mousePosition.x < 10)
        {
            m_transform.Translate(Vector3.left* speed*Time.deltaTime);
        }
        if (Input.GetAxis("Horizontal") > 0 || Input.mousePosition.x > Screen.width -10)
        {
            m_transform.Translate(Vector3.right *speed*Time.deltaTime);
        }
        #endregion

        #region 鼠标滚轮控制镜头缩放
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0)
        {
            m_transform.Translate(Vector3.down * zoomSpeed * Time.deltaTime);
        }
        if (scrollWheel < 0)
        {
            m_transform.Translate(Vector3.up * zoomSpeed * Time.deltaTime);
        }
        #endregion
    }
    private void RotaTheCamera()
    {
        #region 鼠标滚轮控制镜头缩放
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel > 0)
        {
            m_camera.transform.Rotate(m_transform.right * scrollWheel * zoomSpeed*0.3f, Space.World);
        }
        if (scrollWheel < 0)
        {
            m_camera.transform.Rotate(m_transform.right * scrollWheel * zoomSpeed*0.3f, Space.World);
        }
        #endregion
    }
}

