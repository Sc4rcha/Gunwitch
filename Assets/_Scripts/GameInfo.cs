using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Audio;

namespace GameInfo
{

    #region ACTORS
    [Serializable]
    public class ActorStats
    {
        public string Name;

        // combat stats
        public int Health;
        public int Mana;
        public int HealthCurrent;
        public int ManaCurrent;

        // stats
        public int Body;
        public int Magic;
        public int Dexterity;
        public int Luck;
        public int Charisma;
    }

    public enum Ability { BODY, MAGIC, DEX, LUCK, CHAR }
    #endregion

    #region PLAYER
    public class PlayerInfo
    {
        public ActorStats Stats;
        public Inventory Inventory;

        // create player from Initial State
        public PlayerInfo (SOPlayerInitialState playerInitialState)
        {
            Stats = new ActorStats();

            Stats.Name = "Player";

            // set combat stats
            Stats.Health = playerInitialState.Health;
            Stats.Mana = playerInitialState.Mana;
            Stats.HealthCurrent = Stats.Health;
            Stats.ManaCurrent = Stats.Mana;

            // set stats
            Stats.Body = playerInitialState.Body;
            Stats.Magic = playerInitialState.Magic;
            Stats.Dexterity = playerInitialState.Dexterity;
            Stats.Luck = playerInitialState.Luck;
            Stats.Charisma = playerInitialState.Charisma;

            // set inventory
            Inventory = new Inventory();
            foreach (var item in playerInitialState.StartingItems)
                Inventory.AddItem(item.GetItem());
            foreach (var bullet in playerInitialState.StartingBullets)
                Inventory.AddItem(bullet.GetBullet());
        }
    }
    #endregion

    #region COMBAT
    public class CombatActor
    {
        public ActorStats Stats;

        public bool IsDead => Stats.HealthCurrent <= 0;

        // constructor for enemy
        public CombatActor(SOCombatEnemy enemy) 
        {
            Stats = new ActorStats();

            Stats.Health = enemy.Health;
            Stats.Mana = enemy.Mana;

            Stats.Body = enemy.Body;
            Stats.Magic = enemy.Magic;
            Stats.Dexterity = enemy.Dexterity;
            Stats.Luck = enemy.Luck;
            Stats.Charisma = enemy.Charisma;
        }
        // constructor for player
        public CombatActor (PlayerInfo player) 
        {
            Stats = player.Stats;
        }

        // set actor ready for combat
        public void Startcombat() 
        {
            Stats.HealthCurrent = Stats.Health;
            Stats.ManaCurrent = Stats.Mana;
        }

        /// <summary>
        /// Change the current health of actor;
        /// </summary>
        /// <param name="value"></param>
        public void HealthChange(int value) 
        {
            Stats.HealthCurrent = Mathf.Clamp(Stats.HealthCurrent + value, 0, Stats.Health);
        }
        /// <summary>
        /// Change the current mana of actor
        /// </summary>
        /// <param name="value"></param>
        public void ManaChange (int value)
        {
            Stats.ManaCurrent = Mathf.Clamp(Stats.ManaCurrent + value, 0, Stats.Mana);
        }
        /// <summary>
        /// Check if actor has enough mana to cast a spell
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool CheckEnoughMana(int cost) 
        {
            return Stats.ManaCurrent >= cost;
        }
    }

    public class Bullet : InventoryItem
    {
        public int ManaCost;
        public int Damage;

        public Bullet(SOBullet bullet) : base(bullet)
        {
            ManaCost = bullet.ManaCost;
            Damage = bullet.Damage;
        }
    }
    #endregion

    #region INVENTORY
    public class InventoryItem 
    {
        public string Id;
        public string Name;
        public int Quantity;
        public ItemType Type;

        public InventoryItem(SOInventoryItem item) 
        {
            Id = item.Id;
            Name = item.Name;
            Type = item.Type;

            Quantity = 1;
        }
    }
    // class for saving the game
    [Serializable]
    public class ItemSaveData 
    {
        public string Id;
        public int Quantity;
    }

    public enum ItemType { INGREDIENT, BULLET, DRUM, KEY, CONSUMABLE}
    #endregion

    #region CRAFTING
    public class CraftingRecipe 
    {
        public string Id;
        public string Name;

        public InventoryItem[] Ingredients;
        public InventoryItem[] IngredientsStacked;
        public InventoryItem Consumable;

        public CraftingRecipe (SOCraftingRecipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Consumable = recipe.Consumable.GetItem();

            // add ingredients not stacked
            Ingredients = new InventoryItem[recipe.Ingredients.Length];
            for (int i = 0; i < Ingredients.Length; i++)
            {
                Ingredients[i] = recipe.Ingredients[i].GetItem();
            }

            // create dictionary of sinlge ingredients with quanity
            Dictionary<string, InventoryItem> ingredientDict = new Dictionary<string, InventoryItem>();
            foreach (var ingredient in recipe.Ingredients)
            {
                if (ingredientDict.TryGetValue(ingredient.Id, out var existing))
                    existing.Quantity++;
                else
                    ingredientDict.Add(ingredient.Id, ingredient.GetItem());
            }

            IngredientsStacked = ingredientDict.Values.ToArray();
        }
    }
    #endregion

    #region DIALOGUE
    public enum DecisionOption { OptionA, OptionB, OptionC }
    public class Character
    {
        public string Name;
        public Sprite Portrait;
        public Sprite[] Expressions;
        public AudioResource Beep;

        public Character (SODialogueCharacter character)
        {
            Name = character.Name;
            Portrait = character.Portrait;
            Expressions = new Sprite[5];
            Expressions[0] = character.ExpressionNeutral;
            Expressions[1] = character.ExpressionHappy;
            Expressions[2] = character.ExpressionAngry;
            Expressions[3] = character.ExpressionSad;
            Expressions[4] = character.ExpressionSurprised;
            Beep = character.Beep;
        }
    }
    [Serializable]
    public class DialogueNode
    {
        // 0:player | 1+:other characters
        public int IndexCharacterRight;
        public int IndexCharacterLeft;

        // 0:none | 1:player | 2:characterRight | 3:characterLeft
        public int IndexCharacterFocus;
        // 0:Neutral | 1:Happy | 2:Angry | 3:Sad | 4:Surprised
        public int IndexExpression;
        // 0:none | 1:SweatDrop | 2:Veins | 3:Dots
        public int IndexEmotion;

        public string Text;
    }
    public class Dialogue
    {
        // variables for dialogue section
        public Character[] Characters;
        public DialogueNode[] Nodes;

        // constructors
        public Dialogue(SODialogue dialogue)
        {
            // set array of characters
            Characters = new Character[dialogue.Characters.Count + 2];
            Characters[0] = null;
            Characters[1] = Resources.Load<SODialogueCharacter>("Player").GetCharacter();
            for (int i = 0; i < dialogue.Characters.Count; i++)
                Characters[i + 2] = dialogue.Characters[i].GetCharacter();

            // set array of nodes
            Nodes = dialogue.Nodes.ToArray();
        }
    }
    #endregion
}
