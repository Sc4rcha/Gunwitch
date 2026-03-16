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
            if (!Enemy.IsDead)
                TurnBegin();
            else
                TurnFinish();
        }
        public void TurnFinish()
        {
            manager.EnemyTurnNext();
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
            Act();

            yield return new WaitForSeconds(Enemy.TurnCooldown);

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