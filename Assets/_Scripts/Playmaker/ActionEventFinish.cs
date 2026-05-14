using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionEventFinish : FsmStateAction
    {
        public override void OnEnter()
        {
            ManagerGameElements.Instance.ManagerQuest.EventFinish();
            Finish();
        }
 
    }
}