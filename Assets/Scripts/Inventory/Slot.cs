using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool hovering;

    public ItemSO currentItem { get; private set; }
    public int itemAmount { get; private set; }

    private Image iconImage;
    private TextMeshProUGUI amountText;

    private void Awake()
    {
        iconImage = transform.GetChild(0).GetComponent<Image>();
        amountText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetItem(ItemSO item, int amount = 1)
    {
        currentItem = item;
        itemAmount = amount;

        UpdateSlot();
    }

    public void UpdateSlot()
    {
        if(currentItem != null)//없으면 받아들여라
        {
            iconImage.enabled = true;
            iconImage.sprite = currentItem.icon;
            amountText.text = itemAmount.ToString();
        }
        else
        {
            iconImage.enabled = false;
            amountText.text = "";
        }
    }
    public void ClearSlot()
    {
        currentItem = null;
        itemAmount = 0;
        UpdateSlot();
    }
    public int AddAmount(int addAmount)
    {
        itemAmount += addAmount;
        UpdateSlot();
        return itemAmount;
    }
    public int SubtractAmount(int subtractAmount)
    {
        itemAmount -= subtractAmount;
        if (itemAmount <= 0)
            ClearSlot();
        else
            UpdateSlot();

        return itemAmount;
    }

    //포인터 관리
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

   public bool HasItem()
    {
        return currentItem != null;
    }


}
