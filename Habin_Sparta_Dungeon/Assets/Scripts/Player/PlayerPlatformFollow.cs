using UnityEngine;

public class PlayerPlatformFollow : MonoBehaviour
{
    private MovePlatform currentPlatform; // 플레이어가 닿은 발판 저장

    void OnCollisionStay(Collision collision)
    {
        // 닿은 오브젝트에 MovePlatform 컴포넌트가 있으면 저장
        if (collision.gameObject.TryGetComponent(out MovePlatform platform))
        {
            currentPlatform = platform;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // 현재 발판에서 떨어졌을 경우 null로 초기화
        if (collision.gameObject.GetComponent<MovePlatform>() == currentPlatform)
        {
            currentPlatform = null;
        }
    }

    void FixedUpdate()
    {
        // 현재 발판이 존재할 경우, 발판의 이동량만큼 플레이어 위치도 함께 이동
        if (currentPlatform != null)
        {
            transform.position += currentPlatform.GetPlatformDelta();
        }
    }
}
