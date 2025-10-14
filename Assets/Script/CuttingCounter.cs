using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private CutingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;

    public event EventHandler<OnOnPorgressChangeEventArgs> OnPorgressChange;
    public class OnOnPorgressChangeEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    public event EventHandler OnCuttingVisualChanged;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //ClearCounter  were not holding any kitchenObject

            if (player.HasKitchenObject())
            {
                // The player hand are holding the kitchenobject 
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player Carry something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CutingRecipeSO cuttingRecipeSO = GetCutingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnPorgressChange?.Invoke(this, new OnOnPorgressChangeEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.CuttingProgressMax

                    }); 
               
                
                
                
                
                }



                

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
        if (HasKitchenObject()&&HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))   
        {
            //There is a KitchenObject here and can be cut
            cuttingProgress++;
    
            CutingRecipeSO cuttingRecipeSO = GetCutingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnPorgressChange?.Invoke(this, new OnOnPorgressChangeEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.CuttingProgressMax

            });

            OnCuttingVisualChanged?.Invoke(this,EventArgs.Empty);
            

            if (cuttingProgress >= cuttingRecipeSO.CuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());


                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            //    KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);

            }



        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CutingRecipeSO cuttingRecipeSO=GetCutingRecipeSOWithInput(inputKitchenObjectSO);

        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;

        }
        else
        {
            return null;
        }

        
    }


    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CutingRecipeSO cuttingRecipeSO = GetCutingRecipeSOWithInput(inputKitchenObjectSO);

        return cuttingRecipeSO != null;

    }
    private CutingRecipeSO GetCutingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO )
    {
        foreach (CutingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (inputKitchenObjectSO == cuttingRecipeSO.input)
            {
                return cuttingRecipeSO;
            }

        }
        return null;
    }
            
}



