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
        Sprite hair_2 = Resources.Load<Sprite>("Icons/Hair2");
        CreateItemButton(hair_2, "Long hair", 100, 0);
        Hide();
    }

    private void CreateItemButton(Sprite sprite, string name, int price, int position)
    {
        Transform itemTransform = Instantiate(shopItemTemplate, container);
        itemTransform.gameObject.SetActive(true);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        float itemHeight = 30f;
        itemRectTransform.anchoredPosition = new Vector2(0, -itemHeight * position);
        itemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().text = name;
        itemTransform.Find("itemPrice").GetComponent<TextMeshProUGUI>().text = "$"+price.ToString();
        itemTransform.Find("itemImage").GetComponent<Image>().sprite = sprite;
        itemTransform.GetComponent<Button>().onClick.AddListener(() => {
            TryBuyItem(name);
        });
        
    }

    private void TryBuyItem(string name)
    {
        customer.BoughtItem(name);
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
