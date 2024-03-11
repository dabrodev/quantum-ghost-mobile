using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class PlayerControls : MonoBehaviour
{
    private Camera _mainCam;
    private Vector3 _offset;

    private float _maxLeft;
    private float _maxRight;
    private float _maxDown;
    private float _maxUp;


    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;

        StartCoroutine(SetBoundaries());

    }

    // Update is called once per frame
    void Update()
    {
        if (Touch.fingers[0].isActive)
        { 
            Touch myTouch = Touch.activeTouches[0];
            Vector3 touchPos = myTouch.screenPosition;
            touchPos = _mainCam.ScreenToWorldPoint(touchPos);


            if (Touch.activeTouches[0].phase == TouchPhase.Began)
            {
                _offset = touchPos - transform.position;
            }

            if (Touch.activeTouches[0].phase == TouchPhase.Moved || Touch.activeTouches[0].phase == TouchPhase.Stationary )
            {
                transform.position = new Vector3(touchPos.x - _offset.x, touchPos.y - _offset.y, 0);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _maxLeft, _maxRight), Mathf.Clamp(transform.position.y, _maxDown, _maxUp), 0);
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();    
    }


    private IEnumerator SetBoundaries()
    {
        yield return new WaitForEndOfFrame();

        _maxLeft = _mainCam.ViewportToWorldPoint(new Vector2(0.15f, 0)).x;
        _maxRight = _mainCam.ViewportToWorldPoint(new Vector2(0.85f, 0)).x;

        _maxDown = _mainCam.ViewportToWorldPoint(new Vector2(0, 0.10f)).y;
        _maxUp = _mainCam.ViewportToWorldPoint(new Vector2(0, 0.90f)).y;
    }
}