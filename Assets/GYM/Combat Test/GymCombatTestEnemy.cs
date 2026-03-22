using System.Collections;
using UnityEngine;

namespace GymCombat
{
    public class GymCombatTestEnemy : MonoBehaviour
    {
        public GymCharacterEnemy Enemy;
        public SpriteRenderer Renderer;

        private Animator animator;

        GymCombatTest manager;


        public virtual void Setup(GymCombatTest manager) 
        {
            this.manager = manager;
            animator = Renderer.GetComponent<Animator>();
        }

        public void TurnStart() 
        {
            TurnBegin();
        }
        public void TurnFinish()
        {
            manager.EnemyTurnEnd();
        }


        public void Damage(int value) 
        {
            Enemy.HealthChange(-value);

            if (Enemy.IsDead)
               animator.Play("Dead");
            else
               animator.Play("Hurt");
        }

        #region TURN
        protected virtual void TurnBegin() 
        {
            StartCoroutine(TurnBehaviour());
        }
        private IEnumerator TurnBehaviour()
        {
            yield return new WaitForSeconds(Enemy.TurnCooldown / 2);

            Act();

            yield return new WaitForSeconds(Enemy.TurnCooldown / 2);

            TurnEnd();
        }
        protected virtual void TurnEnd() 
        {
            TurnFinish();
        }
        #endregion

        protected virtual void Act() 
        {
            animator.Play("Attack");
            manager.Player.Damage(2);
        }
    }
}