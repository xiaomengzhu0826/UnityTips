using P3;
using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;




public class HighlightSelection : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
    [SerializeField] private CinemachineInputAxisController _controller;
    
    [SerializeField] private InputActionReference _look;
    [SerializeField] private InputActionReference _attack;
    [SerializeField] private InputActionReference _scroll;
    [SerializeField]private float _lensSensitivity;
    private GameObject _lastClickedObject = null;   
    private string _hexColorOne = "#004276";
    private string _hexColorTwo = "#000000";
    private GameObject _clickedObject;  
    private bool _isLookAround;


    //手动控制摄像机属性
    //[SerializeField] private Camera _cam;
    //private Vector3 _previousPosition; 
    //private GameObject _lastclickedObject;
    //private bool _isCamStop;
    //private float moveSpeed = 5f; // 移动速度
    //private float rotateSpeed = 5f; // 旋转速度
    //private float stoppingDistance = 10f; // 摄像机与目标的停止距离

    private void OnEnable()
    {
        _attack.action.performed += Attack;
        _look.action.started += Look;
        _look.action.canceled += LookCancel;
        _scroll.action.performed += Scroll;
    }

    private void Scroll(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();
        Debug.Log("Scroll Value: " + scrollValue);

       _camera.Lens.FieldOfView -= scrollValue.y*_lensSensitivity ; // 根据需要进行缩放
    }

    private void Attack(InputAction.CallbackContext context)
    {
        Invoke("CheckAttack", 0f);
    }

    private void LookCancel(InputAction.CallbackContext context)
    {
        _isLookAround = false;
        _controller.enabled = false;
    }

    private void Look(InputAction.CallbackContext context)
    {
        _isLookAround = true;
        _controller.enabled = true;
    }


    private void Start()
    {
        _controller.enabled=false;
       
    }


    private void Update()
    {
        #region 手动控制摄像机方法
        //if (_clickedObject != null)
        //{
        //    if (_lastclickedObject != _clickedObject)
        //    {
        //         // 获取摄像机与目标的方向向量
        //         Vector3 directionToTarget = _clickedObject.transform.position - _cam.transform.position;

        //         // 计算旋转（使摄像机朝向目标）
        //         Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        //         // 平滑旋转摄像机朝向目标
        //         _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        //         // 计算摄像机到目标的距离
        //         float distance = directionToTarget.magnitude;
        //         Vector3 moveDirection = directionToTarget.normalized;
        //         _cam.transform.position += moveDirection * moveSpeed * Time.deltaTime;

        //         // 如果距离大于设定的停止距离，移动摄像机朝向目标
        //         if (distance < stoppingDistance)
        //         {
        //              _isCamStop = true;
        //               _lastclickedObject = _clickedObject;
        //         }
        //    }

        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        _previousPosition = _cam.ScreenToViewportPoint(Input.mousePosition);
        //    }
        //    if (Input.GetMouseButton(1))
        //    {
        //        Vector3 direction = _previousPosition - _cam.ScreenToViewportPoint(Input.mousePosition);

        //        _cam.transform.position = new Vector3();

        //        _cam.transform.Rotate(new Vector3(1, 0, 0), direction.y * 180);
        //        _cam.transform.Rotate(new Vector3(0, 1, 0), -direction.x * 180, Space.World);
        //        _cam.transform.Translate(new Vector3(0, 0, -10));

        //        _previousPosition = _cam.ScreenToViewportPoint(Input.mousePosition);
        //    }
        //}
        #endregion
    }


    private void CheckAttack()
    {
        if (EventSystem.current.IsPointerOverGameObject() )
        {
            Debug.Log("Clicked on UI element, ignoring scene objects.");
            return; // 如果在 UI 上点击，直接返回，不做物体检测
        }   

        // 从摄像机发射一条射线，基于鼠标点击的位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _clickedObject = hit.collider.gameObject;
            Debug.Log(_clickedObject.name);
            _camera.Follow=_clickedObject.transform;
            // 恢复上一个物体的透明度
            if (_lastClickedObject != null && _lastClickedObject != _clickedObject)
            {
                Renderer lastRenderer = _lastClickedObject.GetComponent<Renderer>();
                if (lastRenderer != null)
                {
                    Color color;
                    if (ColorUtility.TryParseHtmlString(_hexColorTwo, out color))
                    {
                        Material material = lastRenderer.material;
                        material.SetColor("_MixColor", color);
                    }
                    else
                    {
                        Debug.LogError("Invalid color string");
                    }
                }
            }

            // 设置新点击的物体透明度
            Renderer renderer = _clickedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                
                Color color;
                if (ColorUtility.TryParseHtmlString(_hexColorOne, out color))
                {
                    Material material = renderer.material;
                    material.SetColor("_MixColor", color);
                }
                else
                {
                    Debug.LogError("Invalid color string");
                }

                // 更新记录的最后一个点击的物体
                _lastClickedObject = _clickedObject;
            }
        }
    }



    private void OnDisable()
    {
       _attack.action.performed -= Attack;
       _look.action.started -= Look;
       _look.action.canceled -= LookCancel;
        _scroll.action.performed -= Scroll;
    }


}


