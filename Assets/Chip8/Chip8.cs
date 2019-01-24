using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the Chip8 Hardware
/// </summary>
public class Chip8 : MonoBehaviour
{
    /// <summary>Delay timer: This timer is intended to be used for timing the events of games. Its value can be set and read.</summary>
    public static System.Diagnostics.Stopwatch delayTimer = new System.Diagnostics.Stopwatch();
    /// <summary>Sound timer: This timer is used for sound effects. When its value is nonzero, a beeping sound is made.</summary>
    public static System.Diagnostics.Stopwatch soundTimer = new System.Diagnostics.Stopwatch();
    /// <summary>set when draw opcode is called</summary>
    public static bool drawFlag;
    /// <summary>Physical Screen Width</summary>
    public static int screenwidth = 64;
    /// <summary>Physical Screen Height</summary>
    public static int screenheight = 32;
    /// <summary>Best guess at how many cycles the Chip8 runs per second</summary>
    public static readonly double CYCLES_PER_SECOND = 500D;
//    public static readonly double _microSecPerTick = 1000000D / System.Diagnostics.Stopwatch.Frequency;

    #region create chip structure
    #region fontset
    /// <summary>Fontset built into chip</summary>
    public static byte[] fontset = new byte[80]
    {
          0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
          0x20, 0x60, 0x20, 0x20, 0x70, // 1
          0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
          0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
          0x90, 0x90, 0xF0, 0x10, 0x10, // 4
          0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
          0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
          0xF0, 0x10, 0x20, 0x40, 0x40, // 7
          0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
          0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
          0xF0, 0x90, 0xF0, 0x90, 0x90, // A
          0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
          0xF0, 0x80, 0x80, 0x80, 0xF0, // C
          0xE0, 0x90, 0x90, 0x90, 0xE0, // D
          0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
          0xF0, 0x80, 0xF0, 0x80, 0x80  // F
    };
    #endregion
    //The graphics system: 
    //The chip 8 has one instruction that draws sprite to the screen. 
    //Drawing is done in XOR mode and if a pixel is turned off as a result of drawing, the VF register is set. 
    //This is used for collision detection.

    //The systems memory map:
    //0x000-0x1FF - Chip 8 interpreter (contains font set in emu)
    //0x050-0x0A0 - Used for the built in 4x5 pixel font set (0-F)
    //0x200-0xFFF - Program ROM and work RAM
    /// <summary>The Chip 8 has 35 opcodes which are all two bytes long.</summary>
    public static ushort opcode = 0;
    /// <summary>The Chip 8 has 4K memory in total</summary>
    public static byte[] memory = new byte[4096];
    /// <summary>CPU registers: 15 8-bit general purpose registers named V0 to VE. VE = ‘carry flag’.</summary>
    public static byte[] V = new byte[16];
    /// <summary>Index (Address NNN) register (value from 0x000 to 0xFFF)</summary>
    public static ushort I = 0;
    /// <summary>Program Counter (value from 0x000 to 0xFFF)</summary>
    public static ushort pc = 0;
    /// <summary>The graphics are black and white and the screen has a total of 2048 pixels (64 x 32)</summary>
    public static bool[] gfx = new bool[screenwidth * screenheight];
    /// <summary>timer register that counts at 60 Hz. When set above zero it will count down to zero.</summary>
    public static byte delay_timer = 0;
    /// <summary>timer register that counts at 60 Hz. When set above zero it will count down to zero. The system’s buzzer sounds whenever the sound timer reaches zero.</summary>
    public static byte sound_timer = 0;
    /// <summary>opcodes allow the program to jump to a certain address or call a subroutine. Need stack to keep track</summary>
    public static ushort[] stack = new ushort[24]; //at least 16 original 24
    /// <summary>stack pointer</summary>
    public static ushort sp;
    /// <summary> Chip 8 has a HEX based keypad (0x0-0xF),</summary>
    public static bool[] hexInput = new bool[16];
    #endregion
}
