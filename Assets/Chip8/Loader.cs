using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Rom))]
[RequireComponent(typeof(Processor))]

public class Loader : MonoBehaviour
{
    Rom rom;
    Processor processor;
    public InputField input;
    public string Rom_Path = "Assets/Resources/Roms/INVADERS";

    void Initialize()
    {
        Chip8.pc = (ushort)rom.memoffset; // Program counter starts at 0x200
        Chip8.opcode = 0;             // Reset current opcode	
        Chip8.I = 0;                  // Reset index register
        Chip8.sp = 0;                 // Reset stack pointer

        // Clear display	
        // Clear stack
        // Clear registers V0-VF
        // Clear memory

        // Load fontset
        for (int i = 0; i < 80; ++i)
            Chip8.memory[i] = Chip8.fontset[i];

        // Reset timers
        Chip8.delay_timer = 0;
        Chip8.sound_timer = 0;
    }
    
    void Update()
    {
        ////For showing input box to load roms over the net
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    processor.PowerOn = false;
        //    Chip8.iReset();
        //    input.gameObject.SetActive(true);
        //}
    }

    private void Start()
    {
        LoadInput();
    }

    public void LoadInput()
    {
        input.gameObject.SetActive(false);
        rom = GetComponent<Rom>();
        processor = GetComponent<Processor>();
        Initialize();

        rom.LoadRom(Rom_Path);
        processor.PowerOn = true;
        StartCoroutine(processor.Run());
    }
}
