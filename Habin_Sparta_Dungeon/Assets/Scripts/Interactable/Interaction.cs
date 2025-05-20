using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;
    private float lastCheckTime;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float maxDistance = 5f;

    private GameObject curInteractObj;
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText; // 드래그 드롭 안하고 해보기
    private Camera mainCamera;

    void Awake()
    {
        // "PromptText" 찾아서 promptText에 할당
        if (promptText == null)
        {
            GameObject go = GameObject.Find("PromptText");
            if (go != null)
            {
                promptText = go.GetComponent<TextMeshProUGUI>();
            }
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;

            CheckInfo();
        }
    }

    void CheckInfo()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            if (hit.collider.gameObject != curInteractObj) // 현재 보는 오브젝트와 같지 않을때
            {
                curInteractObj = hit.collider.gameObject; // 레이에 닿은 오브젝트로
                curInteractable = hit.collider.GetComponent<IInteractable>(); // 데이터용
                if (curInteractable != null)
                {
                    SetPromptText();
                }
            }
        }
        else
        {
            curInteractObj = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractObj = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}

