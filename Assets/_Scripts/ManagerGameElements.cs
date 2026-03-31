using UnityEngine;

public class ManagerGameElements : MonoBehaviour
{
    private static ManagerGameElements _instance;
    public static ManagerGameElements Instance { get { return _instance; } }

    private void Awake()
    {
        // singletion initialization
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            Setup();
        }
    }


    public SOPlayerInitialState StartingPlayer;
    public SOEventList StartingEvents;
    public Map StartingMap;
    [Space]
    public ManagerEvents ManagerEvents;
    public ManagerMap ManagerOverworld;
    public ManagerDialogue ManagerDialogue;
    [Space]
    public ManagerPlayer Player;

    private void Setup()
    {
        // setup dialogue
        ManagerDialogue.Setup();

        // setup event manager
        ManagerEvents.Setup();
        ManagerEvents.EventAddList(StartingEvents.Events);

        // setup overworld manager
        ManagerOverworld.Setup();
        ManagerOverworld.OpenNewMap(StartingMap);
        ManagerOverworld.Refresh();

        // setup player
        Player.Info = StartingPlayer.GetPlayer();
        Player.Setup();
    }
}
