using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;
    private bool isTrigger = false; // 중복 방지

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        // CharacterManager.Instance.Player.itemData = data;
        // CharacterManager.Instance.Player.addItem?.Invoke();

        // 아이템 타입에 따라 작동 구분
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTrigger == true) return;

        if (other.CompareTag("Player") == false) return;

        if (data.type == ItemType.Instant)
        {
            isTrigger = true;
            ApplyInstantEffects();
            Destroy(gameObject);
        }
    }

    void ApplyInstantEffects()
    {
        Player player = CharacterManager.Instance.Player;

        for (int i = 0; i < data.Instants.Length; i++)
        {
            ItemDataInstant instantEffect = data.Instants[i];
            if (instantEffect.type == InstantType.Stamina)
            {
                player.condition.stamina.Add(instantEffect.value);
            }
            else if (instantEffect.type == InstantType.Speed)
            {
                float multiplier = instantEffect.value;
                float duration = instantEffect.duration;

                player.StartCoroutine(player.SpeedBuff(multiplier, duration));
            }
        }
    }
}
