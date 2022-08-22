using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer
{

    void BoughtItem(string itemName, int itemPrice);
    void showInventory();
    void hideInventory();

}
