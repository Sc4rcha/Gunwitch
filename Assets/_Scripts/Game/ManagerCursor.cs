using UnityEngine;

public class ManagerCursor : MonoBehaviour
{
    public Texture2D CursorDefault;
    public Vector2 CursorDefaultHotSpot;
    [Space]
    public Texture2D CursorHidden;

    public void CursorShow(bool isShow) 
    {
        if (isShow)
            Cursor.SetCursor(CursorDefault, CursorDefaultHotSpot, CursorMode.Auto);
        else
            Cursor.SetCursor(CursorHidden, Vector2.zero, CursorMode.Auto);
    }
}
