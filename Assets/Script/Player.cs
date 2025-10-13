using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent
{

    public static Player Instance { get; private set; }


    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    private Vector3 lastInteractDirection;
    [SerializeField] private LayerMask countersLayerMask;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }




    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player Instance");
        }
        Instance = this;
    }





    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);


        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
       
        
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
            if(rayCastHit.transform.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                // Has clearcounter object 
                if (baseCounter != selectedCounter) {

                    SetSelectedCounter( baseCounter);
                }


            }
            else
            {
                SetSelectedCounter( null);

            }
        }
        else
        {
            SetSelectedCounter(null);
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
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh, playRadius, moveDirX, moveDistance);

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
                canMove =moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHigh, playRadius, moveDirZ, moveDistance);

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


    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
