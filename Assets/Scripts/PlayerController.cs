using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class PlayerController : MonoBehaviour, IShopCustomer
{

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
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hairSpriteLibrary = this.transform.Find("hair").GetComponent<SpriteLibrary>();
        pantSpriteLibrary = this.transform.Find("pants").GetComponent<SpriteLibrary>();
        shirtSpriteLibrary = this.transform.Find("shirt").GetComponent<SpriteLibrary>();
        shoeSpriteLibrary = this.transform.Find("shoes").GetComponent<SpriteLibrary>();
        moneyText = GameObject.Find("moneyText").GetComponent<TextMeshProUGUI>();
        notEnoughMoneyTip = GameObject.Find("notEnoughMoneyTip").GetComponent<TextMeshProUGUI>();
        notEnoughMoneyTip.gameObject.SetActive(false);
        
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
            money -= itemPrice;
            moneyText.text = "$" + money;
            SpriteLibraryAsset spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("SpriteLib/"+itemName);
            if(itemName == "Long Hair" || itemName == "Short Hair"){       
                hairSpriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
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
        }
        else{
            notEnoughMoneyTip.gameObject.SetActive(true);
            StartCoroutine(changeBackMoneyText());  
        }
    }
    public IEnumerator changeBackMoneyText(){
        yield return new WaitForSeconds (2);
        notEnoughMoneyTip.gameObject.SetActive(false);
    }
}
