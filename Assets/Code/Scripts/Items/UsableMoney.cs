using UnityEngine;
using UnityEngine.Events;



public class UsableMoney : UsableWithOutline
{
    public int value = 1;
    

    public override void OnUseUsable()
    {
        PlayerSingleton.Instance.AddMoney(value);
        
        if (PlayerSingleton.Instance.GetMoney == 5)
        {
            PlayerSingleton.Instance.AddTheme("Curiosity", 2);
        }

        Destroy(gameObject);
    }
}