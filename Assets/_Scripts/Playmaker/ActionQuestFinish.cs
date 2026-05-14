using HutongGames.PlayMaker;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Events")]
    public class ActionQuestFinish : FsmStateAction
    {

        public override void OnEnter()
        {
            base.OnEnter();

            ManagerGameElements.Instance.ManagerQuest.ManagerMap.ButtonFinishQuest.SetActive(true);
        }

    }
}