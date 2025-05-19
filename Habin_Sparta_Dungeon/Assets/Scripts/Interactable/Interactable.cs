using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer; // 상호작용될 레이어
    [SerializeField] private float maxCheckDistance = 5f; // 확인 범위

    private GameObject curInteractObj;

    void Update()
    {
        CheckInfo();
    }

    void CheckInfo()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 카메라 중앙

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheckDistance, interactableLayer))
        {
            if (hit.collider.gameObject != curInteractObj) // 레이에 닿은 현재 오브젝트가 다르다면
            {
                curInteractObj = hit.collider.gameObject;
                Debug.Log(curInteractObj.name);
            }
        }
        else
        {
            if (curInteractObj != null)
            {
                // 이전 UI 초기화 로직
                curInteractObj = null;
            }
        }
    }
}
