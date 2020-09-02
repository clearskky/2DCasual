using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour, IInputHandler
{
    //public PlayerCharacter playerCharacter;
    public Joystick joystick;
    //public Text phaseDisplayText;
    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition, lastKnownDirection, JumpDirection;
    private float x, y;
    private int controlDirection;

    void Start()
    {
        controlDirection = PlayerPrefs.GetInt("controlDirection", 1);
    }

    void Update()
    {
        HandleInput();
    }

    public void HandleInput()
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
                x = lastKnownDirection.x * controlDirection;
                y = lastKnownDirection.y * controlDirection;
                JumpDirection = new Vector2(x, y);

                if (JumpDirection != Vector2.zero)
                {
                    PlayerCharacter.Instance.Jump(JumpDirection);
                }
            }

            //phaseDisplayText.text = theTouch.phase.ToString();
        }
    }
}
