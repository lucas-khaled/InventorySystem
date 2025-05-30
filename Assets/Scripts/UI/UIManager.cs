using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private InputAction toggleInput;
    [SerializeField] private InventoryUI inventoryUI;


    private bool isOpen;

    private void Start()
    {
        toggleInput.Enable();
        toggleInput.performed += OnInputClicked;
        inventoryUI.Close();
    }

    private void OnInputClicked(InputAction.CallbackContext context)
    {
        isOpen = !isOpen;
        Time.timeScale = isOpen ? 0 : 1;

        if (isOpen)
            inventoryUI.Open();
        else
            inventoryUI.Close();
    }


}
