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
    public CirlceButton[] Buttons;

    private bool isMoving;
    private float angularSpeed;
    private CombatAgentLock lockAgent;

    public void Setup(ManagerCombat manager, CombatAgentLock lockAgent) 
    {
        // stup buttons
        foreach (var button in Buttons)
        {
            button.Button.Setup(manager);
            button.Button.Circle = this;
            manager.Encounter.AddSubEnemyToEncounter(button.Button);
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


    [System.Serializable]
    public struct CirlceButton 
    {
        public CombatAgentLockButton Button;
        [Range (0,11)]
        public int ButtonPosition;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.magenta;
        Handles.DrawLine(transform.position + GetPosition(0, Radius + 0.3f), transform.position + GetPosition(Direction, Radius));
        Handles.DrawLine(transform.position + GetPosition(6, Radius + 0.3f), transform.position + GetPosition(Direction + 6, Radius));
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, Vector3.forward, Radius);
    }
    private void OnDrawGizmosSelected()
    {
        foreach (var button in Buttons)
            button.Button.transform.position = transform.position + GetPosition(button.ButtonPosition, Radius);
    }

    private Vector3 GetPosition(int step, float radius)
    {
        float theta = ((step * 360 / 12) + 90) * Mathf.Deg2Rad;

        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);

        return new Vector3(x, y, 0);
    }
#endif
}
