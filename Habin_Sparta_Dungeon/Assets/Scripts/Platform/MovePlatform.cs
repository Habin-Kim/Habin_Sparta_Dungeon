using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public enum MoveAxis { X, Z } // 발판이 움직일 축을 정의하는 열거형
    [SerializeField] private MoveAxis moveAxis = MoveAxis.X; // 인스펙터에서 움직일 축을 설정

    [SerializeField] private float moveDistance = 3f; // 발판이 이동할 거리
    [SerializeField] private float moveSpeed = 2f; // 발판이 이동하는 속도

    private Vector3 _lastPosition; // 이전 프레임의 발판 위치
    private Vector3 _deltaMovement; // 현재 프레임에서 이전 프레임까지의 발판 이동량
    private Vector3 _startPos; // 발판이 처음 시작할 때의 위치
    private Rigidbody _rigdbody;

    void Start()
    {
        _startPos = transform.position;
        _lastPosition = _startPos; // 이동량 계산을 위한 초기 위치 저장
        _rigdbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 프레임 간 이동 거리 계산
        _deltaMovement = transform.position - _lastPosition;
        _lastPosition = transform.position;

        float offset = Mathf.PingPong(Time.fixedTime * moveSpeed, moveDistance) - moveDistance / 2f;
        Vector3 target = _startPos;

        if (moveAxis == MoveAxis.X)
            target.x += offset;
        else if (moveAxis == MoveAxis.Z)
            target.z += offset;

        _rigdbody.MovePosition(target);
    }
    
    // 외부에서 발판 이동량을 가져올 수 있게 함
    public Vector3 GetPlatformDelta()
    {
        return _deltaMovement;
    }
}
