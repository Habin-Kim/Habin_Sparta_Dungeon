using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f; // 움직이는 속도
    [SerializeField] private float jumpForce = 10f; // 점프 힘

    private Vector2 _curMoveInput; // 현재 누른 이동버튼
    private LayerMask groundLayer; // 바닥 레이어 체크

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    // 시야각
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    // 시야 민감도
    [SerializeField] private float lookSensitivity;

    private float _camCurXRot; // 카메라 회전값
    private Vector2 _mouseDelta; // 마우스 값

    [Header("HideInspector")]
    private Rigidbody _rigidbody;
    private bool _canLook = true;

    private void Awake()
    {
        // 컴포넌트에서 리지드바디
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // 이동 버튼 누르고 있을때
        if (context.phase == InputActionPhase.Performed)
        {
            _curMoveInput = context.ReadValue<Vector2>();
        }
        // 손을 땔때
        else if (context.phase == InputActionPhase.Canceled)
        {
            _curMoveInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // 점프 버튼 눌렀을때 
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        // 벡터 좌표로 생각 y는 앞뒤 x는 좌우
        Vector3 dir = (transform.forward * _curMoveInput.y) + (transform.right * _curMoveInput.x);
    }

    bool IsGrounded()
    {
        // 레이 4개 생성
        Ray[] rays = new Ray[4]
        {
            // 플레이어 앞뒤좌우 0.2, 0.01만큼 띄우기
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        // Ray와 Ground 충돌 확인
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayer))
            {
                return true;
            }
        }

        return false;
    }
}
