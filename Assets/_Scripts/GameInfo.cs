using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace GameInfo
{
    #region ACTORS
    public class Actor
    {
        public string Name;

        // combat stats
        public int Health;
        public int HealthCurrent;
        public int Mana;
        public int ManaCurrent;

        public bool IsDead => HealthCurrent <= 0;
        /// <summary>
        /// Change the current health of actor;
        /// </summary>
        /// <param name="value"></param>
        public void HealthChange(int value)
        {
            HealthCurrent = Mathf.Clamp(HealthCurrent + value, 0, Health);
        }
    }
    public class ActorEnemy : Actor
    {
        // stats
        public int Armor;
        public int MagicResist;
        public int Strength;
        public int Magic;
        public int Accuracy;
        public int Speed;

        // constructor
        public ActorEnemy(SOCombatEnemy enemy)
        {
            Name = enemy.Name;

            Health = enemy.Health;

            // stats
            Armor = enemy.Armor;
            MagicResist = enemy.MagicResist;
            Strength = enemy.Strength;
            Magic = enemy.Magic;
            Accuracy = enemy.Accuracy;
            Speed = enemy.Speed;
        }

        /// <summary>
        /// this is only for enemies, do not call on player.
        /// </summary>
        public void Startcombat()
        {
            HealthCurrent = Health;
        }
    }
    public class ActorPlayer : Actor
    {
        // stats
        public int Body;
        public int Mind;
        public int Dexterity;
        public int Luck;
        public int Charisma;

        public Inventory Inventory;

        // constructor for player initial state
        public ActorPlayer(SOPlayerInitialState playerInitialState)
        {
            Name = "Player";

            Health = (int)CombatMethods.Health(this, playerInitialState.Stats);
            Mana = (int)CombatMethods.Mana(this, playerInitialState.Stats);
            HealthCurrent = Health;
            ManaCurrent = Mana;

            // set inventory
            Inventory = new Inventory();
            foreach (var item in playerInitialState.StartingItems)
                Inventory.AddItem(item.GetItem());
            foreach (var bullet in playerInitialState.StartingBullets)
                Inventory.AddItem(bullet.GetBullet());
            foreach (var recipe in playerInitialState.StartingRecipes)
                Inventory.AddRecipe(recipe.GetRecipe());

            // set equipped drum
            Inventory.EquippedDrum = playerInitialState.EquippedDrum.Id;
        }
        public ActorPlayer (SaveDataPlayer playerSave) 
        {
            Health = playerSave.Health;
            HealthCurrent = playerSave.HealthCurrent;
            Mana = playerSave.Mana;
            ManaCurrent = playerSave.ManaCurrent;

            Body = playerSave.Body;
            Mind = playerSave.Mind;
            Dexterity = playerSave.Dexterity;
            Luck = playerSave.Luck;
            Charisma = playerSave.Charisma;

            // setup inventory
            InventoryItemReferences itemReferences = ManagerGameElements.Instance.ItemReferences;
            Inventory = new Inventory();

            // add inventory
            foreach (var item in playerSave.Items)
                Inventory.AddItem(itemReferences.GetItemReference(item.Id).GetItem());

            // add recipes
            foreach (var recipe in playerSave.Recipes)
                Inventory.AddRecipe(itemReferences.GetRecipeReference(recipe).GetRecipe());

            // equipped drum
            Inventory.EquippedDrum = playerSave.EquippedDrum;
        }


        /// <summary>
        /// Change the current mana of actor
        /// </summary>
        /// <param name="value"></param>
        public void ManaChange(int value)
        {
            ManaCurrent = Mathf.Clamp(ManaCurrent + value, 0, Mana);
        }
        /// <summary>
        /// Check if actor has enough mana to cast a spell
        /// </summary>
        /// <param name="cost"></param>
        /// <returns></returns>
        public bool CheckEnoughMana(int cost)
        {
            return ManaCurrent >= cost;
        }
    }
    #endregion

    #region GameSave
    [Serializable]
    public class SaveDataPlayer 
    {
        public int Health;
        public int HealthCurrent;
        public int Mana;
        public int ManaCurrent;

        public int Body;
        public int Mind;
        public int Dexterity;
        public int Luck;
        public int Charisma;

        public List<SaveDataItem> Items;
        public List<string> Recipes;
        public string EquippedDrum;
    }
    [Serializable]
    public class SaveDataItem
    {
        public string Id;
        public int Quantity;

        public SaveDataItem (InventoryItem item)
        {
            Id = item.Id;
            Quantity = item.Quantity;
        }
    }


    [Serializable]
    public class SaveDataWorld
    {
        public string CurrentLocation;
        public List<SaveDataFlag> Flags;

        public List<SaveDataEvent> Events;
        public List<SaveDateLocation> Locations;
    }
    [Serializable]
    public class SaveDataEvent
    {
        public string Id;
        public EventState State;

        public SaveDataEvent (SOEvent eventReference, EventState state)
        {
            Id = eventReference.Id;
            State = state;
        }
    }
    [Serializable]
    public class SaveDateLocation
    {
        public string Id;
        public LocationVisibilityType Visibility;

        public SaveDateLocation (MapLocationButtonInfo location)
        {
            Id = location.LocationReference.Id;
            Visibility = location.Visibility;
        }
    }
    [Serializable]
    public class SaveDataFlag
    {
        public string Id;
        public Flags.FlagStateValue FlagState;

        public SaveDataFlag(string id, Flags.FlagStateValue state) 
        {
            Id = id;
            FlagState = state;
        }
    }
    #endregion


    #region WORLD
    public class MapState
    {
        public Flags Flags;
        public SOLocation WorldLocation;

        public MapState()
        {
            Flags = new Flags();
            WorldLocation = null;
        }

        public MapState (List<SaveDataFlag> flags)
        {
            Flags = new Flags(flags);
        }
    }
    public enum LocationVisibilityType { Normal, ForceHide, ForceShow }
    #endregion

    #region EVENTS
    public class Flags
    {
        public Dictionary<string, FlagStateValue> FlagList;


        public Flags() 
        {
            FlagList = new Dictionary<string, FlagStateValue>();
        }
        public Flags (List<SaveDataFlag> saveData)
        {
            FlagList = new Dictionary<string, FlagStateValue>();

            foreach (var flagSaveData in saveData)
            {
                if (flagSaveData.FlagState.Type == FlagValueType.Bool)
                    SetFlag(flagSaveData.Id, flagSaveData.FlagState.BoolValue);
                if (flagSaveData.FlagState.Type == FlagValueType.Int)
                    AddProgress(flagSaveData.Id, flagSaveData.FlagState.IntValue);
            }
        }


        public void SetFlag(string key, bool value)
        {
            if (!FlagList.ContainsKey(key))
            {
                FlagList[key] = new FlagStateValue
                {
                    Type = FlagValueType.Bool,
                    BoolValue = value
                };
            }

            FlagList[key].BoolValue = value;
        }
        public void AddProgress(string key, int amount = 1)
        {
            if (!FlagList.ContainsKey(key))
            {
                FlagList[key] = new FlagStateValue
                {
                    Type = FlagValueType.Int,
                    IntValue = 0
                };
            }

            FlagList[key].IntValue += amount;
        }

        public bool GetBool(string key) => FlagList.ContainsKey(key) && FlagList[key].BoolValue;
        public int GetInt(string key) => FlagList.ContainsKey(key) ? FlagList[key].IntValue : 0;

        public enum FlagValueType { Bool, Int }

        public class FlagStateValue
        {
            public FlagValueType Type;
            public bool BoolValue;
            public int IntValue;
        }
    }

    [Serializable]
    public class EventState
    {
        public bool IsLocked;
        public bool IsActive;
        public bool IsComplete;

        public bool IsAvailable => !IsLocked && IsActive && !IsComplete;

        public EventState(bool isLocked) 
        {
            IsLocked = isLocked;
            IsActive = true;
            IsComplete = false;
        }
    }
    public enum CombatEndType {Win, Lose, Special }
    #endregion

    #region INVENTORY
    public enum ItemType { INGREDIENT, BULLET, DRUM, KEY, CONSUMABLE }

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
    public class Bullet : InventoryItem
    {
        public int ManaCost;
        public int Damage;
        public Color BulletColor;

        public Bullet(SOInventoryItemBullet bullet) : base(bullet)
        {
            ManaCost = bullet.ManaCost;
            Damage = bullet.Damage;
            BulletColor = bullet.BulletColor;
        }
    }
    [Serializable]
    public struct LootItem
    {
        public SOInventoryItem Loot;
        [Range(1, 100)]
        public int Chances;
    }
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
