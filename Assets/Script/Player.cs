using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    private Vector3 lastInteractDirection;
    [SerializeField] private LayerMask countersLayerMask;

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        float interactionDistance = 2f;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDirection = moveDir;
        }

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit rayCastHit, interactionDistance, countersLayerMask))
        {
            if (rayCastHit.transform.TryGetComponent<ClearCounter>(out ClearCounter clearCounter))
            {
                // Has clearcounter object 
                clearCounter.Interact();
            }
        }

    }

    private void Update()
    {
       HandleMovement();
       HandleInteraction();

    }



    //Animation condition detection for the player is moving 
    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteraction()
    {
        float interactionDistance = 2f;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) { 
        lastInteractDirection = moveDir;
        }

        if(Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit rayCastHit, interactionDistance, countersLayerMask))
        {
            if(rayCastHit.transform.TryGetComponent<ClearCounter>(out ClearCounter clearCounter))
            {
                // Has clearcounter object 
                 
            }
        }
        

  

    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playRadius = .7f;
        float playerHigh = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh, playRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //Cannot move toward this direction 
            //Attemp only x movement

            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh, playRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //Can Move only on the X
                moveDir = moveDirX;

            }
            else
            {
                //Can not move only on the x
                //Attemp only Z movement

                Vector3 moveDirZ = new Vector3(0f, 0f, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh, playRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Can move only on the Z 
                    moveDir = moveDirZ;
                }
                else
                {
                    // Can not move on any direction ; 

                }

            }

        }


        if (canMove)
        {
            transform.position += moveDir * moveDistance;

        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
    }
  
}
