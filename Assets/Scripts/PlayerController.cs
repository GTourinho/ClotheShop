using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IShopCustomer
{

    private int inventoryLength = 0;
    private Transform container;
    private Transform inventoryItemTemplate;
    TextMeshProUGUI moneyText;
    TextMeshProUGUI notEnoughMoneyTip;
    private int money = 150;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SpriteLibrary hairSpriteLibrary;
    public SpriteLibrary shirtSpriteLibrary;
    public SpriteLibrary pantSpriteLibrary;
    public SpriteLibrary shoeSpriteLibrary;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Vector2 movementInput;
    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        
        getVariableComponents();
        
        addUsedItemsToInventory();

    }

    private void getVariableComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hairSpriteLibrary = this.transform.Find("hair").GetComponent<SpriteLibrary>();
        pantSpriteLibrary = this.transform.Find("pants").GetComponent<SpriteLibrary>();
        shirtSpriteLibrary = this.transform.Find("shirt").GetComponent<SpriteLibrary>();
        shoeSpriteLibrary = this.transform.Find("shoes").GetComponent<SpriteLibrary>();
        moneyText = GameObject.Find("moneyText").GetComponent<TextMeshProUGUI>();
        notEnoughMoneyTip = GameObject.Find("notEnoughMoneyTip").GetComponent<TextMeshProUGUI>();
        notEnoughMoneyTip.gameObject.SetActive(false);
        // Inventory
        container = GameObject.Find("PlayerCanvas").transform.Find("inventory").transform;
        inventoryItemTemplate = container.Find("inventoryItemTemplate");
        inventoryItemTemplate.gameObject.SetActive(false);
        container.gameObject.SetActive(false);

    }
    
    private void addUsedItemsToInventory(){
        Sprite usedShorts = Resources.Load<Sprite>("Icons/Used Shorts");
        Sprite usedShirt = Resources.Load<Sprite>("Icons/Used Shirt");
        Sprite usedShoes = Resources.Load<Sprite>("Icons/Used Shoes");
        CreateItemButton(usedShorts, "Used Shorts", 5, 0);
        CreateItemButton(usedShirt, "Used Shirt", 5, 1);
        CreateItemButton(usedShoes, "Used Shoes", 5, 2);
    }
    private void CreateItemButton(Sprite sprite, string name, int price, int position)
    {

        Transform itemTransform = Instantiate(inventoryItemTemplate, container);
        itemTransform.gameObject.SetActive(true);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        float itemHeight = 100f;
        itemRectTransform.anchoredPosition = new Vector2(0, -itemHeight * position);
        itemTransform.Find("itemName").GetComponent<TextMeshProUGUI>().text = name;
        itemTransform.Find("itemPrice").GetComponent<TextMeshProUGUI>().text = "$"+price.ToString();
        itemTransform.Find("itemImage").GetComponent<Image>().sprite = sprite;
        inventoryLength++;
        itemTransform.GetComponent<Button>().onClick.AddListener(() => {
            SellItem(name, price);
        });
        
    }
    private void SellItem(string name, int price)
    {
        money += price;
        moneyText.text = "$"+money.ToString();
        inventoryLength--;
        killItemTemplate(name);
    }

    private void killItemTemplate(string name)
    {
        foreach (Transform child in container)
        {
            int index = child.GetSiblingIndex();
            foreach(Transform grandChild in child)
            {
                if (grandChild.name == "itemName")
                {
                    if (grandChild.GetComponent<TextMeshProUGUI>().text == name)
                    {
                        Destroy(child.gameObject);
                        rearrangeInventory(index - 1);
                        unequipIfEquiped(name);
                        return;
                    }
                }
            }
        }
    }
    private void unequipIfEquiped(string name){
        if(pantSpriteLibrary.spriteLibraryAsset.name == name){
            pantSpriteLibrary.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLib/Naked Pants");
        }
        else if(shirtSpriteLibrary.spriteLibraryAsset.name == name){
            shirtSpriteLibrary.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLib/Naked Shirt");;
        }
        else if(shoeSpriteLibrary.spriteLibraryAsset.name == name){
            shoeSpriteLibrary.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLib/Naked Shoes");;
        }
    }

    private void rearrangeInventory(int index)
    {
        foreach (Transform child in container)
        {
            int newIndex = child.GetSiblingIndex();
            if (newIndex > index)
            {
                RectTransform childRectTransform = child.GetComponent<RectTransform>();
                childRectTransform.anchoredPosition = new Vector2(0, -100 * (newIndex - 2));
            }
        }
    }

    private void FixedUpdate() {
        if(movementInput == Vector2.zero){
            animator.SetBool("isMoving", false);
        }
        else{
            CheckCollision();
        }
    }
    private void CheckCollision(){
        if(TryMove(movementInput) || TryMove(new Vector2(movementInput.x, 0))){
                animator.SetBool("isMoving", true);
                FaceWhichAxis();
            }
        else if (TryMove(new Vector2(0, movementInput.y))){
            FaceUpOrDown();    
        }
    }
    private void FaceWhichAxis(){
        if(movementInput.x == 0){
            FaceUpOrDown();
        }
        else{
            FaceLeftOrRight();
        }
    }

    private void FaceLeftOrRight(){
        if(movementInput.x < 0){
            transform.localScale = new Vector3(-1,1,1);
        } else if(movementInput.x > 0){
            transform.localScale = new Vector3(1,1,1);
        }
        animator.SetBool("FaceUp", false);
        animator.SetBool("FaceDown", false);
    }
    private void FaceUpOrDown(){
        if(movementInput.y > 0){
            animator.SetBool("FaceUp", true);
            animator.SetBool("FaceDown", false);
        }
        else{
            animator.SetBool("FaceDown", true);
            animator.SetBool("FaceUp", false);
        }
    }

    private bool TryMove(Vector2 direction){
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);
        if(count == 0){
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else{
            return false;
        }
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void BoughtItem(string itemName, int itemPrice)
    {
        TryBuyItem(itemName, itemPrice);
    }

    public void TryBuyItem(string itemName, int itemPrice){
        if(money >= itemPrice){
            Sprite itemSprite = Resources.Load<Sprite>("Icons/"+itemName);
            money -= itemPrice;
            moneyText.text = "$" + money;
            SpriteLibraryAsset spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLib/"+itemName);
            if(itemName == "Long Haircut" || itemName == "Short Haircut"){       
                hairSpriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
                return;
            }
            else if(itemName == "Blue Shorts" || itemName == "Pink Pants"){
                pantSpriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
            }
            else if(itemName == "Orange Shirt" || itemName == "Green Shirt LS"){
                shirtSpriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
            }
            else if(itemName == "Purple Shoes"){
                shoeSpriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
            }
            int sellPrice = itemPrice - itemPrice / 3;
            CreateItemButton(itemSprite, itemName, sellPrice, inventoryLength);
        }
        else{
            notEnoughMoneyTip.gameObject.SetActive(true);
            StartCoroutine(clearNotEnoughMoneyTip());  
        }
    }
    public IEnumerator clearNotEnoughMoneyTip(){
        yield return new WaitForSeconds (2);
        notEnoughMoneyTip.gameObject.SetActive(false);
    }

    public void showInventory(){
        container.gameObject.SetActive(true);
    }
    public void hideInventory(){
        container.gameObject.SetActive(false);
    }

}
