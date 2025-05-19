using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController _controller;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        _controller = GetComponent<PlayerController>();
    }
}
