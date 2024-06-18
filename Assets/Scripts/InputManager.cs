using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputController inputController;

    public Vector2 movementInput;
    public bool isJump;
    public bool isFire;

    private void OnEnable()
    {
        if (inputController == null)
        {
            inputController = new InputController();

            inputController.Movement.Move.performed += i => movementInput = i.ReadValue<Vector2>(); //WASD

            inputController.Movement.Jump.performed += i => isJump = true; //Jump
            inputController.Movement.Jump.canceled += i => isJump = false;

            inputController.Movement.Fire.performed += i => isFire = true; //Shoot
            inputController.Movement.Fire.canceled += i => isFire = false;
            
        }

        inputController.Enable();
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
}
