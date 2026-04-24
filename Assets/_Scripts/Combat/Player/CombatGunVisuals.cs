using UnityEngine;

public class CombatGunVisuals : MonoBehaviour
{
    public Transform Crosshair;
    public Animator Animator;
    [Space]
    public float CorsshairMaxOffset;

    private float cursorFrontDampTime = 0.05f;
    private Vector2 cursorFrontDampVelocity;

    public void Shoot() 
    {
        Animator.Play("Shoot");
    }
    public void CrosshairShow(bool isShow) 
    {
        // show hide croshair
        Crosshair.gameObject.SetActive(isShow);

        // hide show cursor
        ManagerGameElements.Instance.Cursor.CursorShow(!isShow);
    }
    public void UpdatePosition(Vector2 position) 
    {
        if (Vector2.Distance(Crosshair.position, position) > CorsshairMaxOffset)
            Crosshair.position = Crosshair.position + (Vector3)(position - (Vector2)Crosshair.position).normalized * CorsshairMaxOffset;
        Crosshair.position = Vector2.SmoothDamp(position, Crosshair.position, ref cursorFrontDampVelocity, cursorFrontDampTime);
    }
}
