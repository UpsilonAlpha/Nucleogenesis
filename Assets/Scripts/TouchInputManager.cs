using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchInputManager : MonoBehaviour
{
    private GM gm;
    private float fingerStartTime;
    private Vector2 fingerStartPos;
    private bool isSwipe = false;
    private float maxSwipeTime = 1.5f;
    private float minSwipeDist = 50f;

    private void Start()
    {
        gm = GetComponent<GM>();    
    }
    private void Update()
    {
        if (gm.state == GameState.Playing && Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isSwipe = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;
                    case TouchPhase.Canceled:
                        isSwipe = false;
                        break;
                    default:
                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;
                        if(isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist)
                        {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                            {
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }
                            else
                            {
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if(swipeType.x != 0.0f)
                            {
                                if(swipeType.x > 0.0f)
                                {
                                    gm.Shift(Dir.Right);
                                }
                                else
                                    gm.Shift(Dir.Left);
                            }
                            if(swipeType.y != 0.0f)
                            {
                                if(swipeType.y > 0.0f)
                                {
                                    gm.Shift(Dir.Up);
                                }
                                else
                                    gm.Shift(Dir.Down);
                            }
                        }
                        break;
                }
            }

        }
    }
}
