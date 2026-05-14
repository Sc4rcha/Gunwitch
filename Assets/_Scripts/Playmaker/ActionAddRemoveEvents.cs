using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionAddRemoveEvents : FsmStateAction
    {
        public SOEvent[] Add; 
        public SOEvent[] Remove;

        public override void OnEnter()
        {
            base.OnEnter();

            ManagerGameElements.Instance.ManagerQuest.EventAddList(Add);
            ManagerGameElements.Instance.ManagerQuest.EventDeactivateList(Remove);

            Finish();
        }
    }
}