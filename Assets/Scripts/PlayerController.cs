using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SpriteLibrary spriteLibrary;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Vector2 movementInput;
    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteLibrary = this.transform.Find("hair").GetComponent<SpriteLibrary>();
        animator.enabled = false;
        
        UnityEditor.AssetDatabase.Refresh();

        SpriteLibraryAsset spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>("Hair_2");

        spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;

        animator.enabled = true;
        
    }

    private void FixedUpdate() {


        if(movementInput != Vector2.zero){  
            bool success = TryMove(movementInput);
            if(!success){
                success = TryMove(new Vector2(movementInput.x, 0));
                if(!success){
                    success = TryMove(new Vector2(0, movementInput.y));
                }
            }
            animator.SetBool("isMoving", success);
            // get all sprite library assets
            



        } else{
            
            animator.SetBool("isMoving", false);
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

}
