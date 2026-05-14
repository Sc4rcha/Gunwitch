using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionLocationLock : FsmStateAction
    {
        public bool IsLocked;

        public override void OnEnter()
        {
            ManagerGameElements.Instance.ManagerQuest.ManagerMap.LocationLock(IsLocked);

            Finish();
        }
    }
}