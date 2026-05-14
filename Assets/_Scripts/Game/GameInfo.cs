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

        public List<InventoryItemSaveData> Items;
        public List<string> Recipes;
        public string EquippedDrum;
    }

    [Serializable]
    public class SaveDataWorld
    {
        public string CurrentLocation;
        public List<FlagSaveData> Flags;
        public List<EventSaveData> Events;
        public List<LocationSaveData> Locations;
    }
    #endregion


    #region WORLD
    public enum LocationVisibilityType { Normal, ForceHide, ForceShow }

    public class MapState
    {
        public FlagList Flags;
        public SOLocation WorldLocation;

        public MapState()
        {
            Flags = new FlagList();
            WorldLocation = null;
        }
    }

    [Serializable]
    public class LocationSaveData
    {
        public string Id;
        public LocationVisibilityType Visibility;

        public LocationSaveData(MapLocationButtonInfo location)
        {
            Id = location.LocationReference.Id;
            Visibility = location.Visibility;
        }
    }
    #endregion

    #region EVENTS
    public class FlagList
    {
        public Dictionary<string, StateValue> Saved;

        public FlagList() 
        {
            Saved = new Dictionary<string, StateValue>();
        }
        public FlagList (List<FlagSaveData> saveData)
        {
            Saved = new Dictionary<string, StateValue>();

            foreach (var flagSaveData in saveData)
            {
                if (flagSaveData.FlagState.Type == StateValue.ValueType.Bool)
                    SetFlag(flagSaveData.Id, flagSaveData.FlagState.BoolValue);
                if (flagSaveData.FlagState.Type == StateValue.ValueType.Int)
                    AddProgress(flagSaveData.Id, flagSaveData.FlagState.IntValue);
            }
        }

        public void SetFlag(string key, bool value)
        {
            if (!Saved.ContainsKey(key))
            {
                Saved[key] = new StateValue
                {
                    Type = StateValue.ValueType.Bool,
                    BoolValue = value
                };
            }

            Saved[key].BoolValue = value;
        }
        public void AddProgress(string key, int amount = 1)
        {
            if (!Saved.ContainsKey(key))
            {
                Saved[key] = new StateValue
                {
                    Type = StateValue.ValueType.Int,
                    IntValue = 0
                };
            }

            Saved[key].IntValue += amount;
        }

        public bool GetBool(string key) => Saved.ContainsKey(key) && Saved[key].BoolValue;
        public int GetInt(string key) => Saved.ContainsKey(key) ? Saved[key].IntValue : 0;

        public class StateValue
        {
            public enum ValueType { Bool, Int }
            public ValueType Type;
            public bool BoolValue;
            public int IntValue;
        }
    }
    /// <summary>
    /// Save data for Flag information
    /// </summary>
    [Serializable]
    public class FlagSaveData
    {
        public string Id;
        public FlagList.StateValue FlagState;

        public FlagSaveData(string id, FlagList.StateValue state)
        {
            Id = id;
            FlagState = state;
        }
    }

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
    /// <summary>
    /// Save data for Event information
    /// </summary>
    [Serializable]
    public class EventSaveData
    {
        public string Id;
        public EventState State;

        public EventSaveData(SOEvent eventReference, EventState state)
        {
            Id = eventReference.Id;
            State = state;
        }
    }

    public enum CombatEndType {Win, Lose, Special }
    #endregion

    #region INVENTORY
    public enum ItemType { INGREDIENT, BULLET, DRUM, KEY, CONSUMABLE }

    [Serializable]
    public class InventoryItemSaveData
    {
        public string Id;
        public int Quantity;

        public InventoryItemSaveData(InventoryItem item)
        {
            Id = item.Id;
            Quantity = item.Quantity;
        }
    }

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
    public struct ItemDrop
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
