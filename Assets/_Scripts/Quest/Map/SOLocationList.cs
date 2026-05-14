using UnityEditor.PackageManager;
using UnityEngine;

[CreateAssetMenu(fileName = "Location List", menuName = "Map/Location List")]
public class SOLocationList : ScriptableObject
{

    public SOLocation[] Locations;


    public SOLocation GetLocationReference (string locationId)
    {
        foreach (var locationReference in Locations)
        {
            if (locationReference.Id == locationId)
                return locationReference;
        }

        Debug.LogError("Could not find Location with given Identifier");

        return null;
    }

}