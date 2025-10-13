using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //ClearCounter  were not holding any kitchenObject

            if (player.HasKitchenObject())
            {
                // The player hand are holding the kitchenobject 
                player.GetKitchenObject().SetKitchenObjectParent(this);

            }
            else
            {
                //player did not carry kitchenobjet
            }
        }
        else
        {

            //ClearCounter has the kitchenobject
            if (!player.HasKitchenObject())
            {
                //player did not holding any kitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                //Player holding kitchenObject on hand 
            }
        }

    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            //There is kitchenObject On the counter
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}



