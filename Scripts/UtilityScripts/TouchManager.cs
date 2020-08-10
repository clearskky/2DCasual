using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public PlayerCharacter playerCharacter;
    public Joystick joystick;
    public Text phaseDisplayText;
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition, lastKnownDirection, JumpDirection;
    private float x, y;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleTouchInput();
        //SlingshotPlayer();
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Moved)
            {
                lastKnownDirection = joystick.Direction;
            }
            else if (theTouch.phase == TouchPhase.Stationary)
            {
                lastKnownDirection = joystick.Direction;
            }
            else if (theTouch.phase == TouchPhase.Ended)
            {
                x = lastKnownDirection.x *(-1);
                y = lastKnownDirection.y * (-1);
                JumpDirection = new Vector2(x, y);
                //Debug.Log(lastKnownDirection);
                playerCharacter.Jump(JumpDirection);
            }

            phaseDisplayText.text = theTouch.phase.ToString();
        }
    }
    private void SlingshotPlayer()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }
            else if (theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;

                float x = touchStartPosition.x - touchEndPosition.x;
                float y = touchStartPosition.y - touchEndPosition.y;
                lastKnownDirection = new Vector2(x, y);
                playerCharacter.Jump(lastKnownDirection);
            }

            phaseDisplayText.text = theTouch.phase.ToString();
        }
    }
}
