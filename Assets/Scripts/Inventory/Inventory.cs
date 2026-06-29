using StarterAssets;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Inventory : Singleton<Inventory>
{
    //인풋
    private StarterAssetsInputs _input;

    //인벤토리 게임 오브젝트
    public GameObject hotBarObj;
    public GameObject inventorySlotParent;
    
   
    //인벤토리 내부 데이터 컨테이너
    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotBarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    //아이템 드래그
    public Image dragIcon;

    private Slot draggedSlot = null;
    private bool isDragging;

    public float pickupRange = 3f;
    private Item lookedAtItem = null;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lookedAtRenderer = null;

    //테스트용
    public ItemSO[] Testitem = new ItemSO[2];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        //싱글턴화
        base.Awake();
        //인벤토리 갱신
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotBarSlots.AddRange(hotBarObj.GetComponentsInChildren<Slot>());
        //아이템은 핫바에 먼저, 후에 인벤토리로 들어간다. 
        allSlots.AddRange(hotBarSlots);
        allSlots.AddRange(inventorySlots);
    }

    public void Test()
    {
        if (Input.GetKeyDown(KeyCode.C))
            AddItem(Testitem[Random.Range(0, 10) % 2], 2);
    }
    // Update is called once per frame
    void Update()
    {
        
        Test();

        

        if (Input.GetMouseButtonDown(0))
        {
            if(isDragging)
                DropDrag();
            else
                StartDrag();
        }
        
        UpdateDragItemPositon();
        
    }

    public void AddItem(ItemSO addItem, int amount)
    {
        int remaining = amount;
        foreach(Slot slot in allSlots)//추가하려는 아이템이 있는 지에 대한 검증( 잘됨)
        {
            
            if (slot.currentItem == addItem)
            {
                int currentAmount = slot.itemAmount;
                int maxStack = addItem.maxItemStack;

                if(currentAmount < maxStack) {// 아이템이 있는 인벤토리에 추가할 양 결정
                    int spaceLeft = maxStack - currentAmount;
                    int addAmount = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(addItem, currentAmount + addAmount);
                    remaining -= addAmount;

                    if (remaining <= 0)
                    {
                        return;
                    }
                }
            }
        }
        //추가하려는 아이템이 없을 떄 빈 인벤토리 창에 추가되는 지에 대한 확인(잘 됨)
        foreach(Slot slot in allSlots)
        {
            if (!slot.HasItem())
            {

                int addAmount = Mathf.Min(addItem.maxItemStack, remaining);
                slot.SetItem(addItem,  addAmount);

                remaining -= addAmount;
                if (remaining <= 0)
                {
                    return;
                }
            }
        }

        if (remaining > 0)
        {
            Debug.Log("인벤이 꽉 참");
        }
    }

    private void StartDrag()
    {
        
         Slot hovered = GetHoveredSlot();
            
        if (hovered != null && hovered.HasItem())
            {
                Debug.Log("잡음");
                draggedSlot = hovered;
                isDragging = true;
                //드래그 보여주기
                dragIcon.sprite = draggedSlot.currentItem.icon;
                dragIcon.color = new Color(1, 1, 1, 0.5f);
                dragIcon.enabled = true;
            }
        
    }

    private void DropDrag()
    {

            Slot hovered = GetHoveredSlot();
            Debug.Log("놓음");
            if (hovered != null )
            {
                HandleDrap(draggedSlot, hovered);
                
                //드래그 숨기기
                dragIcon.enabled = false;
                isDragging = false;
                draggedSlot = null;
            }
        
    }

    private void HandleDrap(Slot from ,Slot to)
    {
        if(from == to) return;
        //같은 아이템일 경우에의 스택
        if(from.currentItem == to.currentItem)
        {
            int space = to.currentItem.maxItemStack - to.itemAmount;
            if (space > 0)
            {
                int move = Mathf.Min(space, from.itemAmount);
                to.SetItem(to.currentItem, to.itemAmount + move);
                from.SetItem(from.currentItem, from.itemAmount - move);

                if (from.itemAmount <= 0)
                {
                    from.ClearSlot();
                }

                return;
             
            }
        }

        //다른 템일 경우
        if (to.HasItem())
        {
            ItemSO tmp = to.currentItem;
            int tmpAmount = to.itemAmount;

            to.SetItem(from.currentItem, from.itemAmount);
            from.SetItem(tmp, tmpAmount);

            return;
        }

        //빈 슬롯

        to.SetItem(from.currentItem, from.itemAmount);
        from.ClearSlot(); 
    }

    private Slot GetHoveredSlot()
    {
        foreach(Slot slot in allSlots)
        {
            if (slot.hovering)
            {
                return slot;
            }
        }
        return null;
    }

    private void UpdateDragItemPositon()
    {
        if (isDragging)
        {
            dragIcon.rectTransform.position = Input.mousePosition;
        }
    }

    private void PickUp()
    {
        if(lookedAtRenderer != null && Input.GetKeyDown(KeyCode.F))
        {
            Item item = lookedAtRenderer.GetComponent<Item>();
            if(item!= null)
            {
                AddItem(item.item, item.amount);
                Destroy(item.gameObject);
            }
        }
    }
    private void DetectLookedItem()
    {
        if (lookedAtRenderer != null) ;
        {
            lookedAtRenderer.material = originalMaterial;
            lookedAtRenderer = null;
            originalMaterial = null;
        }
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            Item item = hit.collider.GetComponent<Item>();
            if(item != null)
            {
                Renderer rend = item.GetComponent<Renderer>();
                if(rend != null) {
                    originalMaterial = rend.material;
                    rend.material = highlightMaterial;
                    lookedAtRenderer = rend;
                }
            }
        }
    }
}
