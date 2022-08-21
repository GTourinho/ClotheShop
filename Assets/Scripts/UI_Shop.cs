using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    
    private Transform container;
    private Transform shopItemTemplate;

    private void Awake()
    {
        container = transform.Find("container");
        shopItemTemplate = container.Find("shopItemTemplate");
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start(){
        Sprite hair_2 = Resources.Load<Sprite>("Icons/Hair2");
        CreateItemButton(hair_2, "Long hair", 100, 0);
    }

    private void CreateItemButton(Sprite sprite, string name, int price, int position)
    {
        Transform itemTransform = Instantiate(shopItemTemplate, container);
        itemTransform.gameObject.SetActive(true);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        float itemHeight = 30f;
        itemRectTransform.anchoredPosition = new Vector2(0, -itemHeight * position);
        itemRectTransform.Find("itemName").GetComponent<TextMeshProUGUI>().text = name;
        itemRectTransform.Find("itemPrice").GetComponent<TextMeshProUGUI>().text = "$"+price.ToString();
        itemRectTransform.Find("itemImage").GetComponent<Image>().sprite = sprite;
    }

}
