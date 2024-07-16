using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Extends the UsableWithOutline, represents a collectable coin object
/// By default will have the outline highlight on from the parent class
/// this will update the player money value when used
/// </summary>
public class UsableMoney : UsableWithOutline
{
    public int value = 1;
    
    /// <summary>
    /// Update player money then destroy the object
    /// </summary>
    public override void OnUseUsable()
    {
        PlayerSingleton.Instance.AddMoney(value);
        Destroy(gameObject);
    }
}