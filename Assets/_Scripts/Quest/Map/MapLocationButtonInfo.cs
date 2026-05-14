using UnityEngine;

[RequireComponent (typeof (MapLocationButton))]
public class MapLocationButtonInfo : MonoBehaviour
{
    [Header ("References Scene")]
    public GameObject LocationNameHolder;
    public TMPro.TMP_Text LocationNameText;
    [Header ("References Project")]
    public SOLocation LocationReference;

    public GameInfo.LocationVisibilityType Visibility;

    public void Setup ()
    {
        // set normal visibility by default
        ChangeVisibility(LocationReference.InitialVisibility);
    }


    public void LocationNameShow(bool isShow) 
    {
        LocationNameHolder.SetActive (isShow);
        LocationNameText.text = LocationReference.Name;
    }
    public void LocationInteract() 
    {
        ManagerGameElements.Instance.ManagerQuest.ButtonLocation(LocationReference);
    }

    public void ChangeVisibility(GameInfo.LocationVisibilityType newVisiblity) 
    {
        Visibility = newVisiblity;
    }
}
