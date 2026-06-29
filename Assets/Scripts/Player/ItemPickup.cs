using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    public ItemSO item;
    private int amount = 1;

    public void Interact()
    {
        Inventory.Instance.AddItem(item, amount);

        Destroy(gameObject);
    }

    public string GetInteractionText()
    {
        return $"{item.name} 줍기";
    }
}