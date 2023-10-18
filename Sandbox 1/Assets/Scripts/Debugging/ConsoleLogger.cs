using TMPro;
using UnityEngine;

public class ConsoleLogger : MonoBehaviour
{

    //Singleton
    public static ConsoleLogger Instance { get; private set; }
    TextMeshProUGUI console;
    int num = 0;
    string maintext = string.Empty;
    PlayerMovementComponent playerMovementComponent;

    private void Awake()
    {
        //playerMovementComponent = PlayerManager.Instance.playerScript.movementComponent;

        console = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void Start()
    {
        playerMovementComponent = PlayerManager.Instance.getPlayerScript().playerMovementComponent;
    }

    // Update is called once per frame
    void Update()
    {
        maintext =
            "Y Velocity............" + playerMovementComponent.velocity + "\n" +
            "Move Direction..." + playerMovementComponent.moveDir + "\n" +
            "Move Speed......." + playerMovementComponent.currentSpeed + "\n" +
            "Move State........" + playerMovementComponent.movementStateMachine.CurrentState.ToString() + "\n" +
            "Sprint Timer......." + playerMovementComponent.sprintTimer + "\n" +
            "Jumps..............." + playerMovementComponent.jumpCount + "/" + playerMovementComponent.maxJumps;
        console.text = maintext;
    }

}
