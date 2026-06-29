using UnityEngine;

[CreateAssetMenu(fileName = "item", menuName = "New Item")]
public class ItemSO : ScriptableObject
{
    public string ItemName;
    public int maxItemStack;
    public Sprite icon;
    public GameObject itemPrefab;
    public GameObject handItemPrefab;//  손에 들었을때의 오브젝트 
}
