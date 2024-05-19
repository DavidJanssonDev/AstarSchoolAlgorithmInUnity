using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    public bool userMouseClick;
    public Vector2 userMousePos;

    private Vector2 RAWuserMousePosition = Vector2.zero;

    public void PlayerInputPos(InputAction.CallbackContext context)
    {
        RAWuserMousePosition = context.ReadValue<Vector2>();
    }

    public void PlayerInputClick(InputAction.CallbackContext context)
    {
            userMouseClick = context.ReadValueAsButton();
    }

    // Update is called once per frame
    void Update()
    {
        userMousePos = m_Camera.ScreenToWorldPoint(new(RAWuserMousePosition.x, RAWuserMousePosition.y, m_Camera.transform.position.z));
    }
}
