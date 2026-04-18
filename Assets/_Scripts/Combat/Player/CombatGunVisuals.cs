using UnityEngine;
using UnityEngine.InputSystem;

public class CombatGunVisuals : MonoBehaviour
{
    public Transform Crosshair;
    public Animator Animator;
    public Transform GunStencil;
    [Space]
    public float CorsshairMaxOffset;

    private float cursorFrontDampTime = 0.05f;
    private Vector2 cursorFrontDampVelocity;


    public void Shoot() 
    {
        Animator.Play("Shoot");
    }
    public void Show(bool isShow) 
    {
        Crosshair.gameObject.SetActive(isShow);
    }
    public void UpdatePosition(Vector2 position) 
    {
        if (Vector2.Distance(Crosshair.position, position) > CorsshairMaxOffset)
            Crosshair.position = Crosshair.position + (Vector3)(position - (Vector2)Crosshair.position).normalized * CorsshairMaxOffset;
        Crosshair.position = Vector2.SmoothDamp(position, Crosshair.position, ref cursorFrontDampVelocity, cursorFrontDampTime);
    }


    public void ShowStencil(Vector2 position)
    {
        GunStencil.gameObject.SetActive(true);
        GunStencil.position = position;
    }
    public void HideStencil() 
    {
        GunStencil.gameObject.SetActive(false);
    }
}
