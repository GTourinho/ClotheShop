using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerCollider : MonoBehaviour
{

    [SerializeField] private UI_Shop shop;
    private void OnTriggerEnter2D(Collider2D collider) {
        IShopCustomer customer = collider.GetComponent<IShopCustomer>();
        if(customer != null){
            shop.Show(customer);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        IShopCustomer customer = collider.GetComponent<IShopCustomer>();
        if(customer != null){
            shop.Hide();
        }
    }
}
