using System.Linq;
using UnityEngine;

namespace GymCombat
{
    public class GymCombatTest : MonoBehaviour
    {
        public GymCombatTestPlayer Player;
        public GymCombatTestEnemy[] Enemies;

        public bool IsEnemyRoundEnd;

        private int enemyTurnIndex;
        private bool allEnemiesDead => Enemies.All(x => x.Enemy.IsDead);

        private void Start()
        {
            Setup();
        }

        public void Setup() 
        {
            IsEnemyRoundEnd = true;

            // setup player
            Player.Setup(this);

            // setup enemies
            foreach (var enemy in Enemies)
                enemy.Setup(this);

        }


        public void RoundEnemyStart() 
        {
            IsEnemyRoundEnd = false;

            enemyTurnIndex = -1;
            EnemyTurnNext();
        }
        public void RoundEnemyEnd() 
        {
            IsEnemyRoundEnd = true;

            Player.ReloadStart();
        }
        public void EnemyTurnNext()
        {
            // advance index
            enemyTurnIndex++;

            // if all enemies dead player won the combat
            if (allEnemiesDead)
            {
                CombatOver(true);
                return;
            }

            // end enemy round
            if (enemyTurnIndex == Enemies.Length)
            {
                RoundEnemyEnd();
                return;
            }

            // enemy take turn
            Enemies[enemyTurnIndex].TurnStart();
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