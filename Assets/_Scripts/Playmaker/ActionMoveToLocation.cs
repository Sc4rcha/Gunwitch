using UnityEngine;


namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionMoveToLocation : FsmStateAction
    {
        public SOLocation Location;

        public override void OnEnter()
        {
            ManagerGameElements.Instance.ManagerMap.LocationSetInfo(Location);
            ManagerGameElements.Instance.ManagerMap.LocationEnter();

            Finish();
        }
    }
}