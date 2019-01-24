using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    #region Singleton
    private static Keyboard _instance;

    public static Keyboard Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public KeyCode lastKeyPressed;
    public bool keypress;

    private void Update()
    {
        //TODO, must be a faster way to set flags
        CheckDown();
        CheckUp();
    }
    public void CheckDown()
    {
        keypress = false;
        lastKeyPressed = KeyCode.None;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Chip8.hexInput[0x01] = true;
            keypress = true;
            lastKeyPressed = KeyCode.Alpha1;

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Chip8.hexInput[0x02] = true;
            keypress = true;
            lastKeyPressed = KeyCode.Alpha2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Chip8.hexInput[0x03] = true;
            keypress = true;
            lastKeyPressed = KeyCode.Alpha3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Chip8.hexInput[0x0C] = true;
            keypress = true;
            lastKeyPressed = KeyCode.Alpha4;

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Chip8.hexInput[0x04] = true;
            keypress = true;
            lastKeyPressed = KeyCode.Q;

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Chip8.hexInput[0x05] = true;
            keypress = true;
            lastKeyPressed = KeyCode.W;

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Chip8.hexInput[0x06] = true;
            keypress = true;
            lastKeyPressed = KeyCode.E;

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Chip8.hexInput[0x0D] = true;
            keypress = true;
            lastKeyPressed = KeyCode.R;

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Chip8.hexInput[0x07] = true;
            keypress = true;
            lastKeyPressed = KeyCode.A;

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Chip8.hexInput[0x08] = true;
            keypress = true;
            lastKeyPressed = KeyCode.S;

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            Chip8.hexInput[0x09] = true;
            keypress = true;
            lastKeyPressed = KeyCode.D;

        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Chip8.hexInput[0x0E] = true;

            keypress = true;
            lastKeyPressed = KeyCode.F;

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Chip8.hexInput[0x0A] = true;
            keypress = true;
            lastKeyPressed = KeyCode.Z;

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Chip8.hexInput[0x00] = true;
            keypress = true;
            lastKeyPressed = KeyCode.X;

        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Chip8.hexInput[0x0B] = true;
            keypress = true;
            lastKeyPressed = KeyCode.C;

        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Chip8.hexInput[0x0F] = true;
            keypress = true;
            lastKeyPressed = KeyCode.V;

        }

    }

    private void CheckUp()
    {

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Chip8.hexInput[0x01] = false;            
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Chip8.hexInput[0x02] = false;
            
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Chip8.hexInput[0x03] = false;
            
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            Chip8.hexInput[0x0C] = false;

        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Chip8.hexInput[0x04] = false;

        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            Chip8.hexInput[0x05] = false;

        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            Chip8.hexInput[0x06] = false;

        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Chip8.hexInput[0x0D] = false;

        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            Chip8.hexInput[0x07] = false;

        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Chip8.hexInput[0x08] = false;

        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Chip8.hexInput[0x09] = false;

        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            Chip8.hexInput[0x0E] = false;


        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            Chip8.hexInput[0x0A] = false;
           
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            Chip8.hexInput[0x00] = false;
            
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            Chip8.hexInput[0x0B] = false;
            
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            Chip8.hexInput[0x0F] = false;

        }
    }
}
