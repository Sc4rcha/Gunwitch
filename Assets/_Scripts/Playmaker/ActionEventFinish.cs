using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Event")]
    public class ActionEventFinish : FsmStateAction
    {

        public override void OnEnter()
        {
            ManagerGameElements.Instance.ManagerEvents.EventFinish(true);
            Finish();
        }
 
    }
}