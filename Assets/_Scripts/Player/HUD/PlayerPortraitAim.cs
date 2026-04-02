using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPortraitAim : MonoBehaviour
{

    public RectTransform RectBody;
    public RectTransform RectArms;
    public RectTransform RectHands;
    public RectTransform RectGun;
    [Space]
    public float MoveDistance;
    [Space]
    public Vector2 MoveMultiBody;
    public Vector2 MoveMultiArms;
    public Vector2 MoveMultiHands;
    public Vector2 MoveMultiGun;

    private Vector2 initialPosBody;
    private Vector2 initialPosArms;
    private Vector2 initialPosHands;
    private Vector2 initialPosGun;

    private Vector2 mousePosition;
    private Camera aimCamera;

    public void Setup() 
    {
        initialPosBody = RectBody.anchoredPosition;
        initialPosArms = RectArms.anchoredPosition;
        initialPosHands = RectHands.anchoredPosition;
        initialPosGun = RectGun.anchoredPosition;

        aimCamera = Camera.main;
    }

    private void Update()
    {
        // get mouse relative position on screen
        mousePosition = aimCamera.ScreenToViewportPoint(Mouse.current.position.ReadValue()) - (Vector3.one * 0.5f);

        // move rects
        RectBody.anchoredPosition = initialPosBody + mousePosition * MoveDistance * MoveMultiBody;
        RectArms.anchoredPosition = initialPosArms + mousePosition * MoveDistance * MoveMultiArms;
        RectHands.anchoredPosition = initialPosHands + mousePosition * MoveDistance * MoveMultiHands;
        RectGun.anchoredPosition = initialPosGun + mousePosition * MoveDistance * MoveMultiGun;
    }
}
