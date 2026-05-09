using GameInfo;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaveLoad : MonoBehaviour
{
    private static string SavePathPlayer => Path.Combine(Application.persistentDataPath, "savePlayer.json");
    private static string SavePathWorld => Path.Combine(Application.persistentDataPath, "saveWorld.json");


    #region SAVE WORLD
    public void SaveGame(bool isQuestActive) 
    {
        SavePlayer(ManagerGameElements.Instance.Player.Info);

        if (!isQuestActive)
            DeleteWorldSave();
        else
            SaveWorld();
    }

    private void SavePlayer(ActorPlayer player) 
    {
        SaveDataPlayer playerSave = new SaveDataPlayer();

        // Hp and Mana
        playerSave.Health = player.Health;
        playerSave.HealthCurrent = player.HealthCurrent;
        playerSave.Mana = player.Mana;
        playerSave.ManaCurrent = player.ManaCurrent;

        // stats
        playerSave.Body = player.Body;
        playerSave.Mind = player.Mind;
        playerSave.Dexterity = player.Dexterity;
        playerSave.Luck = player.Luck;
        playerSave.Charisma = player.Charisma;

        // inventory
        playerSave.Items = new List<SaveDataItem>();
        // ingredients
        foreach (var ingredient in player.Inventory.Ingredients)
            playerSave.Items.Add(new SaveDataItem(ingredient.Value));
        // bullets
        foreach (var bullet in player.Inventory.Bullets)
            playerSave.Items.Add(new SaveDataItem(bullet.Value));
        // drums
        foreach (var drum in player.Inventory.Drums)
            playerSave.Items.Add(new SaveDataItem(drum.Value));
        // KeyItems
        foreach (var keyItems in player.Inventory.KeyItems)
            playerSave.Items.Add(new SaveDataItem(keyItems.Value));
        // Consumables
        foreach (var consumables in player.Inventory.Consumables)
            playerSave.Items.Add(new SaveDataItem(consumables));

        // consumables
        playerSave.Recipes = new List<string>();
        foreach (var recipe in player.Inventory.Recipes)
            playerSave.Recipes.Add(recipe.Id);



        // SAVE GAME
        string json = JsonUtility.ToJson(playerSave, true);
        File.WriteAllText(SavePathPlayer, json);

        Debug.Log($"Game saved at: {SavePathPlayer}");
    }

    private void SaveWorld() 
    {
        ManagerGameElements manager = ManagerGameElements.Instance;

        SaveDataWorld worldSave = new SaveDataWorld();

        // set current location
        worldSave.CurrentLocation = manager.ManagerMap.MapState.WorldLocation.Id;

        // flags
        worldSave.Flags = new List<SaveDataFlag>();
        foreach (var flag in manager.ManagerMap.MapState.Flags.FlagList)
            worldSave.Flags.Add(new SaveDataFlag(flag.Key, flag.Value));

        // events
        worldSave.Events = new List<SaveDataEvent>();
        foreach (var eventEntry in manager.ManagerEvents.AddedEvents)
            worldSave.Events.Add(new SaveDataEvent(eventEntry.Key, eventEntry.Value));

        // locations
        worldSave.Locations = new List<SaveDateLocation>();
        foreach (var location in manager.ManagerMap.Map.Locations)
            worldSave.Locations.Add(new SaveDateLocation(location));



        // SAVE GAME
        string json = JsonUtility.ToJson(worldSave, true);
        File.WriteAllText(SavePathWorld, json);

        Debug.Log($"Game saved at: {SavePathWorld}");
    }
    #endregion


    public void LoadGame() 
    {
        // LOAD PLAYER
        if (File.Exists(SavePathPlayer))
        {
            string json = File.ReadAllText(SavePathPlayer);
            SaveDataPlayer playerSave = JsonUtility.FromJson<SaveDataPlayer>(json);

            ManagerGameElements.Instance.Player.Info = new ActorPlayer(playerSave);
        }

        // LOAD WORLD
        if (File.Exists(SavePathWorld))
        {
            string json = File.ReadAllText(SavePathWorld);
            SaveDataWorld worldSaveSave = JsonUtility.FromJson<SaveDataWorld>(json);

            ManagerGameElements.Instance.ManagerMap.LoadSaveData(worldSaveSave);
            ManagerGameElements.Instance.ManagerEvents.LoadSaveFile(worldSaveSave);

            Debug.Log(worldSaveSave.CurrentLocation);
        }
    }


    private void DeleteWorldSave() 
    {
        if (File.Exists(SavePathWorld))
            File.Delete(SavePathWorld);
    }
}
