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
        }
    }


    public SOEventList StartingEvents;
    public OverworldMap StartingMap;
    [Space]
    public ManagerEvents ManagerEvents;
    public ManagerOverworld ManagerOverworld;

    private void Start()
    {
        // setup event manager
        ManagerEvents.Setup();
        ManagerEvents.EventAddList(StartingEvents.Events);

        // setup overworld manager
        ManagerOverworld.Setup();
        ManagerOverworld.OpenNewMap(StartingMap);
        ManagerOverworld.Refresh();
    }
}
