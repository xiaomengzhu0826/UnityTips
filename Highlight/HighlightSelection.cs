using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace P3
{
    public class HighlightSelection : MonoBehaviour
    {
        private PlayerInputReader _playerInputReader;

        private GameObject lastClickedObject = null;
        private Color originalColor;
        private string materialPath = "Materials/SkyBlue";
        public float transparencyValue = 0.5f;
        private void OnEnable()
        {
            try
            {
                _playerInputReader.AttackEvent += OnAttack;
            }
            catch
            {

            }

        }

        private void OnAttack()
        {
            Invoke("CheckAttack", 0f);

        }
        private void CheckAttack()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Clicked on UI element, ignoring scene objects.");
                return; // 如果在 UI 上点击，直接返回，不做物体检测
            }

            // 从摄像机发射一条射线，基于鼠标点击的位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // 恢复上一个物体的透明度
                if (lastClickedObject != null && lastClickedObject != clickedObject)
                {
                    Renderer lastRenderer = lastClickedObject.GetComponent<Renderer>();
                    if (lastRenderer != null)
                    {
                        // 恢复到原来的颜色
                        Color resetColor = originalColor;
                        resetColor.a = 1f; // 不透明
                        lastRenderer.material.color = resetColor;
                    }
                }

                // 设置新点击的物体透明度
                Renderer renderer = clickedObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    
                    // 记录当前点击物体的原始颜色
                    originalColor = renderer.material.color;
                    SetSecondaryMaterial(renderer);
                    // 设置点击物体的透明度
                    Color newColor = originalColor;
                    newColor.a = transparencyValue; // 设置透明度为 transparencyValue
                    renderer.material.color = newColor;

                    // 更新记录的最后一个点击的物体
                    lastClickedObject = clickedObject;
                }
            } 
    }
        private void SetSecondaryMaterial(Renderer renderer)
        {
            if (renderer != null)
            {
                // 从 Resources 文件夹中加载新的材质
                Material newMaterial = Resources.Load<Material>(materialPath);

                if (newMaterial != null)
                {
                    // 获取当前物体的所有材质
                    Material[] currentMaterials = renderer.materials;

                    // 创建一个新的材质数组，比原数组多一个位置
                    Material[] newMaterials = new Material[currentMaterials.Length + 1];

                    // 将原有的材质复制到新的数组中
                    for (int i = 0; i < currentMaterials.Length; i++)
                    {
                        newMaterials[i] = currentMaterials[i];
                    }

                    // 将新的材质添加到数组的最后
                    newMaterials[newMaterials.Length - 1] = newMaterial;

                    // 将扩展后的材质数组赋给 Renderer
                    renderer.materials = newMaterials;
                }
                else
                {
                    Debug.LogError("Failed to load the new material from the given path.");
                }
            }
            else
            {
                Debug.LogError("Target object does not have a Renderer component.");
            }
        }
        private void OnDisable()
        {
            try
            {
                _playerInputReader.AttackEvent -= OnAttack;
            }
            catch
            {

            }
        }


    }
}

