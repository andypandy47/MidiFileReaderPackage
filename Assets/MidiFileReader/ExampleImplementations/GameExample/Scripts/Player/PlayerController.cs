using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Direction
{
    Up, Down, Left, Right
}

public class PlayerController : MonoBehaviour {

    public static PlayerController instance;

    //Movement variables
    public float dodgeSpeed;
    public float returnDelay;
    private float returnDelayTimer;
    public float returnSpeed;
    private int currentPositionIndex;
    private Rigidbody2D rb;
    private Tweener playerMoveTween;

    private bool returning = false;
    private List<bool> keysHeld;

    [SerializeField] private float boxWidth = 5;
    [SerializeField] private float boxHeight = 5;
    private Vector3[] dodgePositions;
    private Vector3 startingPos; //always going to be 0,0
    private Vector3 fromPos; //value to pass to line renderer

    //Create singelton of player class
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0.0f;

        //Create area around the player and define the positions in which they can dodge to
        Rect boxArea = new Rect(0, 0, boxWidth, boxHeight);
        boxArea.center = transform.position;
        dodgePositions = new Vector3[9];
        dodgePositions[0] = new Vector3(boxArea.position.x, boxArea.position.y);
        dodgePositions[1] = new Vector3(0, boxArea.position.y);
        dodgePositions[2] = new Vector3(-boxArea.position.x, boxArea.position.y);
        dodgePositions[3] = new Vector3(boxArea.position.x, 0);
        dodgePositions[4] = new Vector3(0, 0);
        dodgePositions[5] = new Vector3(-boxArea.position.x, 0);
        dodgePositions[6] = new Vector3(boxArea.position.x, -boxArea.position.y);
        dodgePositions[7] = new Vector3(0, -boxArea.position.y);
        dodgePositions[8] = new Vector3(-boxArea.position.x, -boxArea.position.y);

        //Declare starting pos to anchor player to when they dodge
        startingPos = transform.position;

        //this is the center dodgePosition variable index
        currentPositionIndex = 4;

        returnDelayTimer = returnDelay;

        keysHeld = new List<bool>();
    }

    private void Update()
    {
        if (CheckForReturnToStart())
        {
            returnDelayTimer -= Time.deltaTime;
            //and return delay has not reached zero
            if (returnDelayTimer <= 0)
            {
                MoveTo(startingPos, returnSpeed);
                returning = true;
                returnDelayTimer = returnDelay;
                fromPos = transform.position;
                currentPositionIndex = 4;
            }
        }

        if (transform.position == startingPos)
            returning = false;

        if (playerMoveTween != null)
        {
            if (playerMoveTween.IsActive())
            {
                PlayerLineHandler.instance.UpdateLine(fromPos);
            }
            else
            {
                PlayerLineHandler.instance.FadeLine();
            }
        }
    }

    //Handles dodge mechanics
	public void Dodge(Direction dir)
    {
        returning = false;
        returnDelayTimer = returnDelay;
        fromPos = transform.position;
        bool newBool = true;
        keysHeld.Add(newBool);//Adding a bool to keys held so we know another key has been pressed
        PlayerLineHandler.instance.ResetLine();

        //First check we can actually move in the selected direction
        switch(dir)
        {
            case Direction.Up:
                //dodge position is out of bounds so return
                if ((currentPositionIndex + 3) > dodgePositions.Length - 1)
                    return;
                currentPositionIndex += 3;
                MoveTo(dodgePositions[currentPositionIndex], dodgeSpeed);
                break;
            case Direction.Down:
                if ((currentPositionIndex - 3) < 0)
                    return;
                currentPositionIndex -= 3;
                MoveTo(dodgePositions[currentPositionIndex], dodgeSpeed);
                break;
            case Direction.Left:
                {
                    int newIndexPos = currentPositionIndex - 1;
                    if (newIndexPos < 0 || newIndexPos == 5 || newIndexPos == 2)
                        return;
                    currentPositionIndex--;
                    MoveTo(dodgePositions[currentPositionIndex], dodgeSpeed);
                    break;
                }
            case Direction.Right:
                {
                    int newIndexPos = currentPositionIndex + 1;
                    if (newIndexPos > dodgePositions.Length - 1 || newIndexPos == 6 || newIndexPos == 3)
                        return;
                    currentPositionIndex++;
                    MoveTo(dodgePositions[currentPositionIndex], dodgeSpeed);
                    break;
                }
                
        }
    }

    private void MoveTo(Vector3 newDodgePos, float speed)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0.0f;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        if (playerMoveTween != null)
            playerMoveTween.Kill();

        playerMoveTween = rb.DOMove(newDodgePos, speed).SetEase(Ease.InOutQuad);
    }

    //removes a bool from keys held
    public void SetKeyUp()
    {
        keysHeld.RemoveAt(keysHeld.Count - 1);
    }

    private bool CheckForReturnToStart()
    {
        if (!keysHeld.Contains(true) && transform.position != startingPos && !returning)
        {
            return true;
        }
        return false;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector2(boxWidth, boxHeight));

        Gizmos.color = Color.blue;
        if (dodgePositions != null)
        {
            for (int i = 0; i < dodgePositions.Length; i++)
            {
                if (dodgePositions[i] == null)
                    break;

                Gizmos.DrawCube(dodgePositions[i], Vector3.one / 3);

            }
        }
        
    }*/

    private void OnCollisionEnter2D(Collision2D other)
    { 
        ScreenShake.Shake(0.2f);
        PlayerLineHandler.instance.FadeLine();
    }
}
