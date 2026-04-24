using System.Collections;
using UnityEditor;
using UnityEngine;

public class LockButtonCircle : MonoBehaviour
{
    [Header ("Rotation Varialbes")]
    public float Radius;
    public float Speed;
    public int Direction;
    [Header("References")]
    public CombatAgentLockButton[] Buttons;

    private bool isMoving;
    private float angularSpeed;
    private CombatAgentLock lockAgent;

    public void Setup(ManagerCombat manager, CombatAgentLock lockAgent) 
    {
        // stup buttons
        foreach (var button in Buttons)
        {
            button.Setup(manager);
            button.Circle = this;
        }

        // set lock agent reference
        this.lockAgent = lockAgent;

        // get angular speed for rotation
        angularSpeed = (Speed / Radius) * Mathf.Rad2Deg;

        isMoving = true;
    }


    private void Update()
    {
        // rotate
        if (isMoving)
            transform.Rotate(0f, 0f, angularSpeed * Direction * Time.deltaTime);
    }


    public void ButtonHit (int rotateAmount)
    {
        // send rotate event to lock agent
        lockAgent.Rotate(rotateAmount * Direction);
    }


    public void MovePause(float time)
    {
        StopAllCoroutines();
        StartCoroutine(MoveUnpause(time));
    }
    IEnumerator MoveUnpause(float time) 
    {
        isMoving = false;
        yield return new WaitForSeconds(time);
        isMoving = true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.magenta;
        Handles.DrawLine(transform.position + Vector3.up * Radius, transform.position + Vector3.up * Radius + Vector3.right * -Direction);
        Handles.DrawLine(transform.position + Vector3.up * Radius + Vector3.right * -Direction, transform.position + Vector3.up * (Radius + 0.5f));
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, Vector3.forward, Radius);
    }
#endif
}
