using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 2f; // 움직이는 속도
    [SerializeField] private float jumpForce = 2f; // 점프 힘
    [SerializeField] public float runSpeed = 4f; // 달리는 속도
    [SerializeField] private float runCost = 0.02f; // 스태미나 소비
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어 체크
    private Vector2 _curMoveInput; // 현재 누른 이동버튼
    private float curSpeed;

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
    private PlayerCondition _playerCondition;
    private Rigidbody _rigidbody;
    private bool canLook = true;
    private bool isRunning;

    private void Awake()
    {
        // 컴포넌트에서 리지드바디
        _rigidbody = GetComponent<Rigidbody>();
        _playerCondition = CharacterManager.Instance.Player.condition;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
        RunStamina();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context) // 마우스 입력
    {
        _mouseDelta = context.ReadValue<Vector2>();
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

    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRunning = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // 점프 버튼 눌렀을때 
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        curSpeed = isRunning ? runSpeed : moveSpeed;

        // y는 앞뒤 x는 좌우
        Vector3 dir = (transform.forward * _curMoveInput.y) + (transform.right * _curMoveInput.x);
        dir *= curSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        // 마우스가 위아래로 얼마나 움직였는지
        _camCurXRot += _mouseDelta.y * lookSensitivity;
        _camCurXRot = Mathf.Clamp(_camCurXRot, minXLook, maxXLook); // 시야각 제한
        cameraContainer.localEulerAngles = new Vector3(-_camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, _mouseDelta.x * lookSensitivity, 0);
    }
    
    /// <summary>
    /// 바닥 체크
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 달리기 시 소모되는 스태미나
    /// </summary>
    void RunStamina()
    {
        if (isRunning && _curMoveInput != Vector2.zero)
        {
            bool canRun = _playerCondition.UseStamina(runCost);

            if (!canRun)
            {
                isRunning = false;
            }
        }
    }
}