using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    // 싱글턴 패턴
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            // null일때 캐릭터 매니저 생성 후 스크립트 추가
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
