using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface Player 
{

    public void Die() {}

    public void OnSwitch() {}

    public void TakeDamage(int d)
    {
        
    }

    public void setPlayerBeingPushed(bool pushed) {
        
    }
 
}