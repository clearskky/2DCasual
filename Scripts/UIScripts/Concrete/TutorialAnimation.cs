using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAnimation : MonoBehaviour
{
    public Button nextButton;

    private Animator animator;

    private void Start()
    {
        Time.timeScale = 1;
        animator = GetComponent<Animator>();
        nextButton.enabled = false;
    }

    public void EndOfHandleAnimation()
    {
        animator.SetTrigger("playJumpAnim");
    }
    public void EndOfAnimations()
    {
        nextButton.enabled = true;
        animator.SetTrigger("playHandleAnim");
    }
}
