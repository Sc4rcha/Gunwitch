using GameInfo;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGun : MonoBehaviour
{
    public TMPro.TMP_Text Name;
    public TMPro.TMP_Text Description;
    public Button ButtonLeft;
    public Button ButtonRight;
    public Button ButtonEquip;

    private int drumIndex;
    private List<SOInventoryItem> drums;

    private bool isOpen;

    private ActorPlayer player;

    public void Setup(ActorPlayer player) 
    {
        this.player = player;
        drums = new List <SOInventoryItem>();

        Hide();
    }

    public void Show() 
    {
        isOpen = true;
        gameObject.SetActive(true);

        // setup drum list
        drums.Clear();
        foreach (var drum in player.Inventory.Drums)
            drums.Add(ManagerGameElements.Instance.ItemReferences.GetItemReference(drum.Value.Id));

        // set buttons interactable if more than one drum
        ButtonLeft.interactable = drums.Count > 1;
        ButtonRight.interactable = drums.Count > 1;

        // get index of currently equipped drum
        drumIndex = drums.FindIndex(x => x.Id == player.Inventory.EquippedDrum);

        // display drum info
        DisplayDrum();
    }
    public void Hide() 
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
    public void OpenClose() 
    {
        isOpen = !isOpen;
        if (isOpen)
            Show();
        else
            Hide();
    }
    
    public void DisplayDrum() 
    {
        // show drum info
        Name.text = drums[drumIndex].Name;
        Description.text = drums[drumIndex].Description;

        // equip button interactable if drum is not equipped
        ButtonEquip.interactable = drums[drumIndex].Id != player.Inventory.EquippedDrum;
    }
    public void Select(int direction) 
    {
        // add direction to drum index
        drumIndex += direction;

        // loop around drum index
        if (drumIndex < 0)
            drumIndex = drums.Count - 1;
        else if (drumIndex == drums.Count)
            drumIndex = 0;

        // display drum info
        DisplayDrum();
    }
    public void Equip() 
    {
        player.Inventory.EquipDrum(drums[drumIndex].Id);
    }
}
