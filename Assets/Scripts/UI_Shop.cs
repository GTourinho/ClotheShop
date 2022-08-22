using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    
    private Transform container;
    private Transform shopItemTemplate;
    private IShopCustomer customer;  

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start(){

        Sprite longHair = Resources.Load<Sprite>("Icons/Long Haircut");
        Sprite shortHair = Resources.Load<Sprite>("Icons/Short Haircut");
        Sprite blueShorts = Resources.Load<Sprite>("Icons/Blue Shorts");
        Sprite pinkPants = Resources.Load<Sprite>("Icons/Pink Pants");
        Sprite orangeShirt = Resources.Load<Sprite>("Icons/Orange Shirt");
        Sprite greenShirtLS = Resources.Load<Sprite>("Icons/Green Shirt LS");
        Sprite purpleShoes = Resources.Load<Sprite>("Icons/Purple Shoes");
        
        CreateItemButton(longHair, "Long Haircut", 20, 0);
        CreateItemButton(shortHair, "Short Haircut", 15, 1);
        CreateItemButton(blueShorts, "Blue Shorts", 35, 2);
        CreateItemButton(pinkPants, "Pink Pants", 30, 3);
        CreateItemButton(orangeShirt, "Orange Shirt", 25, 4);
        CreateItemButton(greenShirtLS, "Green Shirt LS", 40, 5);
        CreateItemButton(purpleShoes, "Purple Shoes", 45, 6);
        
        Hide();
    }

    private void CreateItemButton(Sprite sprite, string name, int price, int position)
    {
        Transform itemTransform = Instantiate(shopItemTemplate, container);
        itemTransform.gameObject.SetActive(true);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        float itemHeight = 100f;
        itemRectTransform.anchoredPosition = new Vector2(0, -itemHeight * position);
        itemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().text = name;
        itemTransform.Find("itemPrice").GetComponent<TextMeshProUGUI>().text = "$"+price.ToString();
        itemTransform.Find("itemImage").GetComponent<Image>().sprite = sprite;
        itemTransform.GetComponent<Button>().onClick.AddListener(() => {
            TryBuyItem(name, price);
        });
        
    }

    private void TryBuyItem(string name, int price)
    {
        customer.BoughtItem(name, price);
    }

    public void Show(IShopCustomer customer)
    {
        this.customer = customer;
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

}
