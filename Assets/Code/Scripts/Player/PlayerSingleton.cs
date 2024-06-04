using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralWorlds;


public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton Instance;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private UserInterface userInterface;
    private PlayerMovement playerMovement;
    

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        playerMovement = GetComponent<PlayerMovement>();    
        Gaia.GaiaAPI.SetRuntimePlayerAndCamera(gameObject, playerCamera, true);
        //FloraAutomationAPI.SetRenderCamera(newCamera);

    }

    // Update is called once per frame
    void Update()
    {
        HandleInterfaceInput();
    }

    void HandleInterfaceInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            userInterface.ToggleHistoryMenu(playerMovement.state);
        }
    }
}
