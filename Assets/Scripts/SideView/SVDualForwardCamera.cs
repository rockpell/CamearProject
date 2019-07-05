//Script by Florian Grundmann (IndieFlorianG)
//You can use and change this script as you like.
//Credit would be nice, but is not needed

using UnityEngine;
using System.Collections;

public class SVDualForwardCamera : MonoBehaviour
{
    public bool follow = true; //Whether the camera should follow the player
    private Vector3 moveTo; //Target vector
    private SVPlayerController player;

    public float speedX = 12f; //Speed of the camera
    public float speedY = 4f;
    public float yTolerance = 1.3f; //Tolerance in y direction
    public float xTolerance = 0.3f; //Tolerance in x direction

    private bool movingRight = true; //Whether the player is on the way right

    [SerializeField] private GameObject bottomBorder = null; //Border the player can't come bellow
    [SerializeField] private GameObject leftBorder = null; //Border the player can't cross if he is moving right
    [SerializeField] private GameObject leftTolerance = null; //Only if the player crosses this border he is viewed as moving left
    [SerializeField] private GameObject rightBorder = null; //Border the player can't cross if he is moving left
    [SerializeField] private GameObject rightTolerance = null; //Only if the player crosses this border he is viewed as moving right
    private float yOffset; //Offset of the camera
    private float xOffset; //Offset of the camera

    // Use this for initialization
    void Start()
    {
        //Find the player
        player = FindObjectOfType<SVPlayerController>();

        //Set the offsets
        yOffset = -bottomBorder.transform.localPosition.y;
        xOffset = leftBorder.transform.localPosition.x;

        //Set camera to start position
        moveTo = new Vector3(player.transform.position.x - xOffset, player.transform.position.y + yOffset * 0.9f, transform.position.z);
        transform.position = moveTo;
    }

    // Update is called once per frame
    void Update()
    {

        if (follow)
        {

            //Update x and y position
            UpdateXDirection();
            UpdateYDirection();

        }
    }

    private void UpdateYDirection()
    {

        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, moveTo.y, transform.position.z), speedY * Time.deltaTime);

        //If the player is bellow the bottom border move instantly to him in y direction 
        if (player.transform.position.y < bottomBorder.transform.position.y)
        {
            moveTo = new Vector3(moveTo.x, player.transform.position.y + yOffset, moveTo.z);
            transform.position = new Vector3(transform.position.x, moveTo.y, transform.position.z);
        }
        //else if (player.transform.position.y > bottomBorder.transform.position.y)
        //{
        //    transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        //}

        //If the player is standing on ground, set the target vector
        if (player.IsOnGround)
        {
            if (player.transform.position.y - (moveTo.y - yOffset) > yTolerance)
                moveTo = new Vector3(moveTo.x, player.transform.position.y + yOffset, moveTo.z);
        }
    }

    private void UpdateXDirection()
    {

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(moveTo.x, transform.position.y, transform.position.z), speedX * Time.deltaTime);

        if (movingRight)
        {
            //If player is far away from the left border (= If the direction changed shortly)
            //move to the player in x direction
            if (player.transform.position.x > leftBorder.transform.position.x + xTolerance)
            {
                moveTo = new Vector3(player.transform.position.x - xOffset, moveTo.y, moveTo.z);

                //If the player just crossed the left border, move instantly to the player in x direction
            }
            else if (player.transform.position.x > leftBorder.transform.position.x)
            {
                moveTo = new Vector3(player.transform.position.x - xOffset, moveTo.y, moveTo.z);
                transform.position = new Vector3(moveTo.x, transform.position.y, transform.position.z);
            }
            //If the player crossed the left tolerance border, change the direction
            if (player.transform.position.x < leftTolerance.transform.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            //If player is far away from the right border (= If the direction changed shortly)
            //move to the player in x direction
            if (player.transform.position.x < rightBorder.transform.position.x - xTolerance)
            {
                moveTo = new Vector3(player.transform.position.x + xOffset, moveTo.y, moveTo.z);
                //If the player just crossed the right border, move instantly to the player in x direction
            }
            else if (player.transform.position.x < rightBorder.transform.position.x)
            {
                moveTo = new Vector3(player.transform.position.x + xOffset, moveTo.y, moveTo.z);
                transform.position = new Vector3(moveTo.x, transform.position.y, transform.position.z);
            }
            //If the player crossed the right tolerance border, change the direction
            if (player.transform.position.x > rightTolerance.transform.position.x)
            {
                movingRight = true;
            }
        }
    }
}
