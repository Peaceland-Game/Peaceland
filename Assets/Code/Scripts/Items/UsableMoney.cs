using UnityEngine;
using UnityEngine.Events;



public class UsableMoney : UsableWithOutline
{
    public int value = 1;
    

    public override void OnUseUsable()
    {
        PlayerSingleton.Instance.AddMoney(value);
        
        Destroy(gameObject);
    }
}