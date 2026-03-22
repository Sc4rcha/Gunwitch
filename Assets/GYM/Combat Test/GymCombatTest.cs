using System.Linq;
using UnityEngine;

namespace GymCombat
{
    public class GymCombatTest : MonoBehaviour
    {
        public GymCombatTestPlayer Player;
        public GymCombatTestEnemy[] Enemies;

        public bool IsEnemyRoundEnd;

        protected int enemyTurnIndex;
        protected bool allEnemiesDead => Enemies.All(x => x.Enemy.IsDead);


        private void Start()
        {
            Setup();
        }

        public virtual void Setup() 
        {
            // setup player
            Player.Setup(this);

            // setup enemies
            foreach (var enemy in Enemies)
                enemy.Setup(this);

            RoundEnemyFinish();
        }



        public virtual void RoundEnemyStart() 
        {
            IsEnemyRoundEnd = false;

            enemyTurnIndex = -1;
            EnemyTurnStart();
        }
        public virtual void RoundEnemyFinish() 
        {
            IsEnemyRoundEnd = true;

            Player.ReloadStart();
        }
        public virtual void EnemyTurnStart()
        {
            // advance index
            enemyTurnIndex++;

            // end enemy round
            if (enemyTurnIndex == Enemies.Length)
            {
                RoundEnemyFinish();
                return;
            }

            // enemy take turn
            if (!Enemies[enemyTurnIndex].Enemy.IsDead)
                Enemies[enemyTurnIndex].TurnStart();
            else
                Enemies[enemyTurnIndex].TurnFinish();

        }
        public virtual void EnemyTurnEnd()
        {
            // if all enemies dead player won the combat
            if (allEnemiesDead)
            {
                CombatOver(true);
                return;
            }

            EnemyTurnStart();
        }



        public virtual void PlayerShoot() 
        {
            if (Player.bulletsIndex == 3)
                Player.ReloadStart();
        }
        public virtual void PlayerReloadEnd()
        {
            RoundEnemyStart();
        }



        public void CombatOver(bool isWin) 
        {
            Debug.Log("Win");
        }
    }


    [System.Serializable]
    public class GymCharacter
    {
        public int HealthMax;
        public int HealthCurrent;

        public bool IsDead => HealthCurrent <= 0;

        public void HealthChange (int value) 
        {
            HealthCurrent = Mathf.Clamp(HealthCurrent + value, 0, HealthMax);
        }
    }

    [System.Serializable]
    public class GymCharacterPlayer : GymCharacter
    {
        public int ManaMax;
        public int ManaCurrent;

        public void ManaChange(int value) 
        {
            ManaCurrent = Mathf.Clamp(ManaCurrent + value, 0, ManaMax);
        }
    }

    [System.Serializable]
    public class GymCharacterEnemy : GymCharacter
    {
        public float TurnCooldown;
    }

    [System.Serializable]
    public class GymBullet
    {
        public enum Type { Normal, Heal, AOE}
        public Type BulletType;
        public int Value;
        public Color BulletColor;
        public int Mana;
    }
}