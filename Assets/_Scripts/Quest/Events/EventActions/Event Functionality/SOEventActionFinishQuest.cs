using UnityEngine;

[CreateAssetMenu(fileName = "Finish Quest", menuName = "Event/Actions Functionality/Finish Quest", order = 30)]
public class SOEventActionFinishQuest : SOEventAction
{
    public override void Execute(ManagerQuest manager)
    {
        manager.ManagerMap.ButtonFinishQuest.SetActive(true);
    }
}
