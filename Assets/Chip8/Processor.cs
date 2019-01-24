using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(Monitor))]
[RequireComponent(typeof(Keyboard))]
[RequireComponent(typeof(Rom))]
[RequireComponent(typeof(OpCodes))]
public class Processor : MonoBehaviour
{
    #region Components
    Monitor monitor;
    Keyboard keyboard;
    Sound sound;
    Rom rom;
    OpCodes opCodes;
    #endregion
    //Timers count down at 60 hertz
    float _60HZ = (1.0f / 60.0f) * 1000.0f; //60HZ

    public bool PowerOn = true;

    private void Start()
    {
        #region Components
        opCodes = GetComponent<OpCodes>();
        rom = GetComponent<Rom>();
        sound = GetComponent<Sound>();
        keyboard = GetComponent<Keyboard>();
        monitor = GetComponent<Monitor>();
        monitor.CreateTexture(Chip8.screenwidth, Chip8.screenheight);
        #endregion

        Chip8.delayTimer.Stop();
        Chip8.soundTimer.Stop();
    }

    public IEnumerator Run()
    {
        Stopwatch cycleTimer = new Stopwatch(); //Tracks time between cycles
        TimeSpan elapsedSpan;                   //Keeps track time between cycles
        int cycles_to_catch_up = 0;             //Keeps track of how many cycles need to be run to catch up in time
        double TimeRemainder = 0;               //Holds the non int part of cycleTimer
        cycleTimer.Start();        

        while (PowerOn)
        {
            elapsedSpan = new TimeSpan(cycleTimer.ElapsedTicks);
            cycles_to_catch_up = (int)(Chip8.CYCLES_PER_SECOND * (elapsedSpan.TotalSeconds + TimeRemainder));
            TimeRemainder = elapsedSpan.TotalSeconds - (int)elapsedSpan.TotalSeconds;
            int cycles = 0;

            while (cycles_to_catch_up > cycles)
            {
                cycles += 1;
                ExecuteCycle();
                if (Chip8.drawFlag)
                    monitor.SetPixels32(Chip8.gfx);
            }

            if (cycles > 0)
                cycleTimer.Restart();
            yield return null;
        }
        monitor.Clear(Color.black); //make it look like power is off
    }

    void ExecuteCycle()
    {
        Chip8.drawFlag = false;
        // Fetch Opcode
        byte[] parseRaw = new byte[2] { Chip8.memory[Chip8.pc + 1], Chip8.memory[Chip8.pc] };
        Chip8.opcode = BitConverter.ToUInt16(parseRaw, 0);
        //opcode = (ushort)(memory[(int)pc] << 8 | memory[(int)pc + 1]);
       //Debug.Log(Chip8.opcode.ToString("X"));
        opCodes.Execute();

        // Update timers
        if (Chip8.delay_timer > 0)
        {
            //Counts down at 60 hertz, until it reaches 0
            if (Chip8.delayTimer.Elapsed.Milliseconds >= _60HZ)
            {
                Chip8.delayTimer.Restart();
                --Chip8.delay_timer;
            }
        }
        else
        {
            Chip8.delayTimer.Start();
        }

        //sound is played when the value of the sound timer is nonzero.
        if (Chip8.sound_timer > 0)
        {
            //Counts down at 60 hertz, until it reaches 0
            if (Chip8.soundTimer.Elapsed.Milliseconds >= _60HZ)
            {
                Chip8.soundTimer.Restart();
                if (Chip8.sound_timer != 0 && !sound.IsPlaying())
                    sound.Play();
                --Chip8.sound_timer;
            }
        }
        else
        {
            Chip8.soundTimer.Start();
            sound.Stop();
        }
    }
}
