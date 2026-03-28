using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace GameInfo
{

    #region ACTORS
    [Serializable]
    public class Actor
    {
        public string Name;

        // combat stats
        public int Health;
        public int Mana;

        // stats
        public int Body;
        public int Magic;
        public int Dexterity;
        public int Luck;
        public int Charisma;
    }

    public enum ActorStat { BODY, MAGIC, DEX, LUCK, CHAR }
    #endregion

    #region COMBAT
    public class CombatActor : Actor
    {
        public int HealthCurrent;
        public int ManaCurrent;

        public bool IsDead => HealthCurrent <= 0;

        // constructor
        public CombatActor(SOCombatEnemy enemy) 
        {
            Name = enemy.Name;
            Health = enemy.Health;
            Mana = enemy.Mana;

            Body = enemy.Body;
            Magic = enemy.Magic;
            Dexterity = enemy.Dexterity;
            Luck = enemy.Luck;
            Charisma = enemy.Charisma;
        }

        // set actor ready for combat
        public void Startcombat() 
        {
            HealthCurrent = Health;
            ManaCurrent = Mana;
        }

        // damage or heal actor
        public void HealthChange(int value) 
        {
            HealthCurrent = Mathf.Clamp(HealthCurrent + value, 0, Health);
        }
    }

    public class Bullet
    {
        public InventoryItem ItemInfo;

        public int Damage;

        public Bullet(SOInventoryItemBullet bullet) 
        {
            ItemInfo = new InventoryItem(bullet);
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

    public enum ItemType { INGREDIENT, BULLET, CONSUMABLE, DRUM, KEY}
    #endregion

    #region CRAFTING
    public class CraftingRecipe 
    {
        public string Id;
        public string Name;

        public InventoryItem[] Ingredients;
        public InventoryItem[] IngredientsStacked;
        public InventoryItem Bullet;

        public CraftingRecipe (SOCraftingRecipe recipe)
        {
            Id = recipe.Id;
            Name = recipe.Name;
            Bullet = recipe.Bullet.GetItem();

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
