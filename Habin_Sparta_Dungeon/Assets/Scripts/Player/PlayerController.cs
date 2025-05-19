using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f; // 움직이는 속도
    [SerializeField] private float jumpForce = 2f; // 점프 힘
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어 체크
    private Vector2 curMoveInput; // 현재 누른 이동버튼

    [Header("Look")]
    [SerializeField] private Transform cameraContainer;
    // 시야각
    [SerializeField] private float minXLook;
    [SerializeField] private float maxXLook;
    // 시야 민감도
    [SerializeField] private float lookSensitivity;

    private float camCurXRot; // 카메라 회전값
    private Vector2 mouseDelta; // 마우스 값

    [Header("HideInspector")]
    private Rigidbody _rigidbody;
    private bool canLook = true;

    private void Awake()
    {
        // 컴포넌트에서 리지드바디
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLook(InputAction.CallbackContext context) // 마우스 입력
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 이동 버튼 누르고 있을때
        if (context.phase == InputActionPhase.Performed)
        {
            curMoveInput = context.ReadValue<Vector2>();
        }
        // 손을 땔때
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
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
        Vector3 dir = (transform.forward * curMoveInput.y) + (transform.right * curMoveInput.x);
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        // 마우스가 위아래로 얼마나 움직였는지
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 시야각 제한
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
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
    
        public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
