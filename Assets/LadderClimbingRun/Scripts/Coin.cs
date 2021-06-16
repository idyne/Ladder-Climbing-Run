using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Collectible
{
    public override void GetCollected()
    {
        ActivateEffect();
        levelManager.IncrementGold();
        DestroyMesh();
       
    }



}
