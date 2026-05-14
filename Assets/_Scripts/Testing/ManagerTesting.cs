using UnityEngine;
using UnityEngine.UI;

public class ManagerTesting : MonoBehaviour
{
    public SOEvent[] Events;
    public CombatEncounter[] Encounters;
    [Space]
    public Button[] ButtonEvents;
    public Button[] ButtonEncounters;

    private void Start()
    {
        foreach (var button in ButtonEvents)
            button.gameObject.SetActive(false);

        for (int i = 0; i < Events.Length; i++)
        {
            ButtonEvents[i].gameObject.SetActive(true);
            ButtonEvents[i].GetComponentInChildren<TMPro.TMP_Text>().text = Events[i].Name;
        }

        foreach (var button in ButtonEncounters)
            button.gameObject.SetActive(false);

        for (int i = 0; i < Encounters.Length; i++)
        {
            ButtonEncounters[i].gameObject.SetActive(true);
            ButtonEncounters[i].GetComponentInChildren<TMPro.TMP_Text>().text = Encounters[i].name;
        }
    }

    public void StartEvent(int index)
    {
        ManagerGameElements.Instance.ManagerQuest.ManagerEvents.EventStart(Events[index]);
        gameObject.SetActive(false);
    }
    public void StartFight(int index)
    {
        ManagerGameElements.Instance.CombatLoad(Encounters[index]);
        gameObject.SetActive(false);
    }
}
