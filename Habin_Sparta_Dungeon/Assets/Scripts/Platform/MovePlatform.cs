using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public enum MoveAxis { X, Z }
    [SerializeField] private MoveAxis moveAxis = MoveAxis.X;

    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 _lastPosition;
    private Vector3 _deltaMovement;
    private Vector3 _startPos;
    private Rigidbody _rigdbody;

    void Start()
    {
        _startPos = transform.position;
        _lastPosition = _startPos;
        _rigdbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _deltaMovement = transform.position - _lastPosition;
        _lastPosition = transform.position;

        float offset = Mathf.PingPong(Time.time * moveSpeed, moveDistance) - moveDistance / 2f;
        Vector3 target = _startPos;

        if (moveAxis == MoveAxis.X)
            target.x += offset;
        else if (moveAxis == MoveAxis.Z)
            target.z += offset;

        _rigdbody.MovePosition(target);
    }

    public Vector3 GetPlatformDelta()
    {
        return _deltaMovement;
    }
}
