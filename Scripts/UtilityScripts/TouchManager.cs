using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManager : MonoBehaviour
{
    public Text phaseDisplayText;
    public Text velocityDisplayText;
    private Touch theTouch;
    public Vector2 touchStartPosition, touchEndPosition, direction;
    public PlayerCharacter playerCharacter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
                direction = new Vector2(x, y);
                playerCharacter.Jump(direction);
            }
            
            phaseDisplayText.text = theTouch.phase.ToString();

            
        }

    }
}
