using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public IEnumerator SpeedBuff(float multiplier, float duration)
    {
        // 버프 전 원래 속도 저장
        float originalRun = this.controller.runSpeed;
        float originalWalk = this.controller.moveSpeed;

        // multiplier만큼 속도 증가
        this.controller.runSpeed = originalRun * multiplier;
        this.controller.moveSpeed = originalWalk * multiplier;

        // duration 초만큼 대기
        yield return new WaitForSeconds(duration);

        // 원래 속도로 복원
        this.controller.runSpeed = originalRun;
        this.controller.moveSpeed = originalWalk;
    }
}
