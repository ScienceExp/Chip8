using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Emulates all the opCodes of the Chip8
/// </summary>
public class OpCodes : MonoBehaviour
{
    public bool ShowOpCodesInConsole = false;
    Dictionary<ushort, Action> op;

    void Start()
    {
        //Add all opCode functions to the Dictionary
        op = new Dictionary<ushort, Action>();
        op.Add(0x0000, _0NNN);
        op.Add(0x00E0, _00E0);
        op.Add(0x00EE, _00EE);

        op.Add(0x1000, _1NNN);
        op.Add(0x2000, _2NNN);
        op.Add(0x3000, _3XNN);
        op.Add(0x4000, _4XNN);
        op.Add(0x5000, _5XY0);
        op.Add(0x6000, _6XNN);
        op.Add(0x7000, _7XNN);

        op.Add(0x8000, _8XY0);
        op.Add(0x8001, _8XY1);
        op.Add(0x8002, _8XY2);
        op.Add(0x8003, _8XY3);
        op.Add(0x8004, _8XY4);
        op.Add(0x8005, _8XY5);
        op.Add(0x8006, _8XY6);
        op.Add(0x8007, _8XY7);
        op.Add(0x800E, _8XYE);

        op.Add(0x9000, _9XY0);
        op.Add(0xA000, _ANNN);
        op.Add(0xB000, _BNNN);
        op.Add(0xC000, _CXNN);
        op.Add(0xD000, _DXYN);
        op.Add(0xE000, _EXNN);

        op.Add(0xF000, _FXNN);
        op.Add(0xF007, _FX07);
        op.Add(0xF00A, _FX0A);
        op.Add(0xF015, _FX15);
        op.Add(0xF018, _FX18);
        op.Add(0xF01E, _FX1E);
        op.Add(0xF029, _FX29);
        op.Add(0xF033, _FX33);
        op.Add(0xF055, _FX55);
        op.Add(0xF065, _FX65);
    }

    /// <summary>
    /// Executes opCode function at emulates the opCode command
    /// </summary>
    /// <param name="opCode">uShort Chip9 opCode to execute</param>
    public void Execute()
    {
        op[(ushort)(Chip8.opcode & 0xF000)](); //only use left most value to find function
    }

    #region 0x0???
    /// <summary>
    /// 0NNN Call SYS addr [???]
    /// </summary>
    void _0NNN()
    {
        switch (Chip8.opcode)
        {
            #region 00E0 cls Clears the screen
            case 0x00E0:
                op[0x00E0]();
                break;
            #endregion
            #region 00EE rts Returns from a subroutine
            case 0x00EE:
                op[0x00EE]();
                break;
            #endregion
            #region 0NNN SYS addr - Calls RCA 1802 program at address NNN. (ignored by modern interpreters)
            default:
                ushort NNN = (ushort)(Chip8.opcode & 0x0FFF);     //address
                Chip8.pc = NNN; //TODO: Not sure if this is right???
#if DEBUG
                if (ShowOpCodesInConsole) Debug.Log("0x0NNN RCA:" + NNN.ToString());
#endif 
                break;
                #endregion
        }
    }

    /// <summary>
    /// 00E0 Clears the screen [disp_clear()]
    /// </summary>
    void _00E0()
    {
        for (int i = 0; i < Chip8.gfx.Length; i++)
            Chip8.gfx[i] = false;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("0x00E0 cls");
#endif 
    }

    /// <summary>
    /// 00EE rts Returns from a subroutine [return;]
    /// </summary>
    void _00EE()
    {
        --Chip8.sp;                                   //Decrement stack position
        Chip8.pc = Chip8.stack[Chip8.sp];             //Place stack position in program counter
        Chip8.pc += 2;                                //Increment position since we already did call
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("0x00EE rts");
#endif 
    }
    #endregion

    /// <summary>
    /// 1NNN jmp Jumps to address NNN [goto NNN;]
    /// </summary>
    void _1NNN()
    {
        ushort NNN = (ushort)(Chip8.opcode & 0x0FFF);     //address
        Chip8.pc = NNN;                                   //Jump to address
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("1NNN jmp:" + NNN.ToString());
#endif 
    }

    /// <summary>
    /// 2NNN jsr calls subroutine at NNN [*(0xNNN)()]
    /// </summary>
    void _2NNN()
    {
        ushort NNN = (ushort)(Chip8.opcode & 0x0FFF);     //address
        Chip8.stack[Chip8.sp] = Chip8.pc;                 //Place return position in stack
        ++Chip8.sp;                                       //Increment stack position
        Chip8.pc = NNN;                                   //Jump to address
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("2NNN jsr:" + NNN.ToString());
#endif
    }

    /// <summary>
    /// 3XNN skeq skips the next instruction if VX equals NN [if(Vx==NN)]
    /// </summary>
    void _3XNN()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        byte NN = (byte)(Chip8.opcode & 0x00FF);          //8-bit constant
        Chip8.pc += Chip8.V[X] == NN ? (ushort)4 : (ushort)2; //if vx=nn pc+=4 else pc+=2
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("3XNN skeq:" + NN.ToString());
#endif
    }

    /// <summary>
    /// 4XNN skne skips the next instruction if VX doesn't equal NN [if(Vx!=NN)]
    /// </summary>
    void _4XNN()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        byte NN = (byte)(Chip8.opcode & 0x00FF);          //8-bit constant
        Chip8.pc += Chip8.V[X] != NN ? (ushort)4 : (ushort)2; //if vx != NN pc+=4 else pc+=2
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("4XNN skne:" + NN.ToString());
#endif 
    }

    /// <summary>
    /// 5XY0 skeq skips the next instruction if VX equals VY [if(Vx==Vy)]
    /// </summary>
    void _5XY0()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        Chip8.pc += Chip8.V[X] == Chip8.V[Y] ? (ushort)4 : (ushort)2; //if vx=vy pc+=4 else pc+=2
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("5XY0 skeq");
#endif
    }

    /// <summary>
    /// 6XNN mov sets VX to NN [Vx = NN]
    /// </summary>
    void _6XNN()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        byte NN = (byte)(Chip8.opcode & 0x00FF);          //8-bit constant
        Chip8.V[X] = NN;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("0x6XNN mov:" + NN.ToString());
#endif
    }

    /// <summary>
    /// 7XNN adds NN to VX [Vx += NN]
    /// </summary>
    void _7XNN()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        byte NN = (byte)(Chip8.opcode & 0x00FF);          //8-bit constant
        Chip8.V[X] += NN;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("0x7XNN add:" + NN.ToString());
#endif 
    }

    #region 0x8XY?
    /// <summary>
    /// 8XY0 VX & VY opperations 
    /// [C Pseudo] Vx=Vy
    /// </summary>
    void _8XY0()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        byte N = (byte)(Chip8.opcode & 0x000F);           //4-bit constant

        #region 8XY0 mov sets VX to the value of VY
        if (N == 0)
        {
            Chip8.V[X] = Chip8.V[Y];
            Chip8.pc += 2;
#if DEBUG
            if (ShowOpCodesInConsole) Debug.Log("8XY0 mov");
#endif
        }
        #endregion
        #region Call other 0x8000 functions
        else
        {
            ushort N00N = (ushort)(Chip8.opcode & 0xF00F);           //4-bit constant
            op[N00N]();
        }
        #endregion
    }

    /// <summary>
    /// 8XY1 or sets VX to VX or VY [Vx=Vx|Vy]
    /// </summary>
    void _8XY1()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        Chip8.V[X] |= Chip8.V[Y];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XY1 or");
#endif
    }

    /// <summary>
    /// 8XY2 and sets VX to VX and VY [Vx=Vx&Vy]
    /// </summary>
    void _8XY2()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        Chip8.V[X] &= Chip8.V[Y];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XY2 and");
#endif
    }

    /// <summary>
    /// 8XY3 xor sets VX to VX xor VY [Vx=Vx^Vy]
    /// </summary>
    void _8XY3()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        Chip8.V[X] ^= Chip8.V[Y];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XY3 xor");
#endif
    }

    /// <summary>
    /// 8XY4 add adds VY to VX. VF is set to 1 when there's a carry, and to 0 when there isn't. [Vx += Vy]
    /// </summary>
    void _8XY4()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        if (Chip8.V[Y] > (Byte.MaxValue - Chip8.V[X]))    //y > 255-x
            Chip8.V[0xF] = 1;                             //carry (0x000F = 1)
        else
            Chip8.V[0xF] = 0;
        Chip8.V[X] += Chip8.V[Y];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XY4 add");
#endif
    }

    /// <summary>
    /// 8XY5 sub VY is subtracted from VX. VF is set to 0 when there's a borrow, and 1 when there isn't [Vx -= Vy]
    /// </summary>
    void _8XY5()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        if (Chip8.V[X] < Chip8.V[Y])                    //y > x (todo check)
            Chip8.V[0xF] = 0;                           //borrow (0x000F = 0)
        else
            Chip8.V[0xF] = 1;
        Chip8.V[X] -= Chip8.V[Y];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XY5 sub");
#endif
    }

    /// <summary>
    /// 8XY6 shr shifts VX right by one. VF is set to the value of the least significant bit of VX before the shift. 
    /// [C Pseudo] Vx>>=1
    /// </summary>
    void _8XY6()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        Chip8.V[0xF] = (byte)(Chip8.V[X] & 0x1);
        Chip8.V[X] >>= 1;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XY6 shr");
#endif
    }

    /// <summary>
    /// 8XY7 rsb sets VX to VY minus VX. VF is set to 0 when there's a borrow, and 1 when there isn't
    /// [C Pseudo] Vx=Vy-Vx 
    /// </summary>
    void _8XY7()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        if (Chip8.V[Y] < Chip8.V[X])                      //y < x (todo check)
            Chip8.V[0xF] = 0;                             //borrow (0x000F = 0)
        else
            Chip8.V[0xF] = 1;
        Chip8.V[X] = (byte)(Chip8.V[Y] - Chip8.V[X]);
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XY7 rsb");
#endif 
    }

    /// <summary>
    /// 8XYE shl shifts VX left by one. VF is set to the value of the most significant bit of VX before the shift.
    /// [C Pseudo] Vx<<=1
    /// </summary>
    void _8XYE()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        Chip8.V[0xF] = (byte)((Chip8.V[X] & 0x80) >> 7);  //0x80= 1000 0000
        Chip8.V[X] <<= 1;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("8XYE shl");
#endif 
    }
    #endregion

    /// <summary>
    /// 9XY0 skne skips the next instruction if VX doesn't equal VY
    /// [C Pseudo] if(Vx!=Vy)
    /// </summary>
    void _9XY0()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;             //4-bit register identifier
        Chip8.pc += Chip8.V[X] != Chip8.V[Y] ? (ushort)4 : (ushort)2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("9XY0 skne");
#endif 
    }

    /// <summary>
    /// ANNN mvi sets I to the address NNN
    /// [C Pseudo] I = NNN
    /// </summary>
    void _ANNN()
    {
        ushort NNN = (ushort)(Chip8.opcode & 0x0FFF);     //address
        Chip8.I = NNN;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("ANNN mvi:" + NNN.ToString());
#endif
    }

    /// <summary>
    /// BNNN jmi jumps to the address NNN plus V0
    /// [C Pseudo] PC=V0+NNN 
    /// </summary>
    void _BNNN()
    {
        ushort NNN = (ushort)(Chip8.opcode & 0x0FFF);     //address
        Chip8.pc = (ushort)(NNN + Chip8.V[0]);
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("BNNN jmi:" + NNN.ToString());
#endif
    }

    /// <summary>
    /// CXNN rand sets VX to a random number & NN
    /// [C Pseudo] Vx=rand()&NN 
    /// </summary>
    void _CXNN()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        byte NN = (byte)(Chip8.opcode & 0x00FF);          //8-bit constant
        Chip8.V[X] = (byte)(Random.Range(0, byte.MaxValue) & NN);
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("CXNN rand:" + NN.ToString());
#endif
    }

    /// <summary>
    /// DXYN sprite Draws a sprite at coordinate (VX, VY) that has a width of 8 pixels and a height of N pixels. 
    /// Each row of 8 pixels is read as bit-coded (with the most significant bit of each byte displayed on the left) 
    /// starting from memory location I; I value doesn't change after the execution of this instruction. 
    /// As described above, VF is set to 1 if any screen pixels are flipped from set to unset when the sprite is drawn, 
    /// and to 0 if that doesn't happen.
    /// [C Pseudo] draw(Vx,Vy,N)
    /// </summary>
    void _DXYN()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;           //4-bit register identifier
        int Y = (Chip8.opcode & 0x00F0) >> 4;           //4-bit register identifier
        byte N = (byte)(Chip8.opcode & 0x000F);         //4-bit constant
        ushort height = N;                              //height
        ushort spriteWidth = 8;                         //width of sprite image in memory
        byte RowData = 0;                               //8 bits of 1 row of sprite data
        int screenPixel = 0;                            //current pixel being edited
        int x = 0;
        int y = 0;

        Chip8.V[0xF] = 0;                               //reset flipped flag
        for (int h = 0; h < height; h++)                //row
        {
            RowData = Chip8.memory[Chip8.I + h];        //get sprite row of pixels from memory
            for (int w = 0; w < spriteWidth; w++)       //col
            {                                           //0x80 = 1000 0000
                if ((RowData & (0x80 >> w)) != 0)       //check 1 bit at a time, see if pixel and line bit are 1  
                {
                    x = Chip8.V[X] + w;
                    y = Chip8.V[Y] + h;

                    //If the sprite is positioned so part of it is outside the coordinates of the display, it wraps around to the opposite side of the screen.
                    //if (x > screenwidth-1)
                    //    while (x > screenwidth-1)
                    //        x = x - screenwidth;
                    //if (y > screenheight-1)
                    //    while (y > screenheight-1)
                    //        y = y - screenheight;                                        

                    screenPixel = (x + y * Chip8.screenwidth);
                    if (screenPixel < Chip8.gfx.Length)
                    {
                        if (Chip8.gfx[screenPixel])        //if that pixel is set on screen
                            Chip8.V[0xF] = 1;              //set collision flag
                        Chip8.gfx[screenPixel] ^= true;    //exclusive-OR (gfx=gfx^1) gfx must = 0 to become 1    
                    }
                }
            }
        }

        Chip8.drawFlag = true;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("DXYN sprite:" + N.ToString());
#endif
    }

    /// <summary>
    /// EX9E skpr skips the next instruction if the key stored in VX is pressed.
    /// [C Pseudo] if(key()==Vx)
    /// 
    /// EXA1 Skips the next instruction if the key stored in VX isn't pressed.
    /// [C Pseudo] if(key()!=Vx) 
    /// </summary>
    void _EXNN()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        byte NN = (byte)(Chip8.opcode & 0x00FF);          //8-bit constant
        switch (NN)
        {
            #region EX9E skpr skips the next instruction if the key stored in VX is pressed.
            case 0x009E:
                if (Chip8.hexInput[Chip8.V[X]])
                    Chip8.pc += 2;
                Chip8.pc += 2;
#if DEBUG
                if (ShowOpCodesInConsole) Debug.Log("EX9E skpr");
#endif
                break;
            #endregion
            #region EXA1 skup skips the next instruction if the key stored in VX isn't pressed
            case 0x00A1:
                if (!Chip8.hexInput[Chip8.V[X]])
                    Chip8.pc += 2;
                Chip8.pc += 2;
#if DEBUG
                if (ShowOpCodesInConsole) Debug.Log("EXA1 skup");
#endif 
                break;
            #endregion
            default:                        //Unknown opcode
#if DEBUG
                Debug.Log("Unknown opcode: " + Chip8.opcode.ToString("X"));
#endif 
                break;
        }
    }

    #region 0xFX??
    /// <summary>
    /// FX07 gdelay sets VX to the value of the delay timer
    /// [C Psuedo] Vx = get_delay()
    /// </summary>
    void _FX07()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        Chip8.V[X] = Chip8.delay_timer;
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX07 gdelay");
#endif 
    }

    /// <summary>
    /// FX0A a key press is awaited, and then stored in VX
    /// [C Psuedo] Vx = get_key() 
    /// </summary>
    void _FX0A()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        //yield return StartCoroutine(Keyboard.Instance.WaitForKeyDown());
        #region store V[X] = keypress
        switch (Keyboard.Instance.lastKeyPressed)
        {
            case KeyCode.Alpha1:
                Chip8.V[X] = 0x01;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.Alpha2:
                Chip8.V[X] = 0x02;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.Alpha3:
                Chip8.V[X] = 0x03;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.Alpha4:
                Chip8.V[X] = 0x0C;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.Q:
                Chip8.V[X] = 0x04;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.W:
                Chip8.V[X] = 0x05;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.E:
                Chip8.V[X] = 0x06;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.R:
                Chip8.V[X] = 0x0D;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.A:
                Chip8.V[X] = 0x07;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.S:
                Chip8.V[X] = 0x08;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.D:
                Chip8.V[X] = 0x09;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.F:
                Chip8.V[X] = 0x0E;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.Z:
                Chip8.V[X] = 0x0A;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.X:
                Chip8.V[X] = 0x00;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.C:
                Chip8.V[X] = 0x0B;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            case KeyCode.V:
                Chip8.V[X] = 0x0F;
                Chip8.pc += 2; //only increment if key is pressed
                break;
            default:
#if Debug
            Debug.Log("Error reading input");
#endif
                break;
        }
        #endregion
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX0A key");
#endif 
    }

    /// <summary>
    /// FX15 sdelay sets the delay timer to VX.
    /// [C Psuedo] delay_timer(Vx) 
    /// </summary>
    void _FX15()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        Chip8.delay_timer = Chip8.V[X];
        Chip8.delayTimer.Restart(); //reset timer for first tick
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX15 sdelay");
#endif 
    }

    /// <summary>
    /// FX18 sound sets the sound timer to VX.
    /// [C Psuedo] sound_timer(Vx)
    /// </summary>
    void _FX18()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        Chip8.sound_timer = Chip8.V[X];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX18 ssound");
#endif 
    }

    /// <summary>
    /// FX1E adds VX to I.
    /// [C Psuedo] I +=Vx 
    /// </summary>
    void _FX1E()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        Chip8.I += Chip8.V[X];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX1E adi");
#endif 
    }

    /// <summary>
    /// FX29 font sets I to the location of the sprite for the character in VX. 
    /// Characters 0-F (in hexadecimal) are represented by a 4x5 font.
    /// [C Psuedo] I=sprite_addr[Vx]
    /// </summary>
    void _FX29()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        Chip8.I = (ushort)(Chip8.V[X] * 0x5);     //5 high
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX29 font");
#endif 
    }

    /// <summary>
    /// FX33 bcd stores the Binary-coded decimal representation of VX, 
    /// with the most significant of three digits at the address in I, 
    /// the middle digit at I plus 1, and the least significant digit at I plus 2.
    /// [C Psuedo] 
    /// set_BCD(Vx); 
    /// *(I+0)=BCD(3);
    /// *(I+1)=BCD(2);
    /// *(I+2)=BCD(1);
    /// </summary>
    void _FX33()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        Chip8.memory[Chip8.I + 0] = (byte)((Chip8.V[X] / 100));      //234 / 100 = 2
        Chip8.memory[Chip8.I + 1] = (byte)((Chip8.V[X] / 10) % 10);  //234 / 10  = 23  (23 % 10 = 3)
        Chip8.memory[Chip8.I + 2] = (byte)((Chip8.V[X] % 10));       //234 % 10  = 4
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX33 bcd");
#endif 
    }

    /// <summary>
    /// FX55 str stores V0 to VX in memory starting at address I 
    /// (I is incremented to point to the next location on. e.g. I = I + X + 1)
    /// [C Psuedo] reg_dump(Vx,&I)
    /// </summary>
    void _FX55()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        for (int i = 0; i < X + 1; i++)   //includes X
            Chip8.memory[Chip8.I + i] = Chip8.V[i];
        Chip8.I += (ushort)(X + 1);
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX55 str");
#endif 
    }

    /// <summary>
    /// FX65 ldr fills V0 to VX with values from memory starting at address I
    /// [C Psuedo] reg_load(Vx,&I) 
    /// </summary>
    void _FX65()
    {
        int X = (Chip8.opcode & 0x0F00) >> 8;             //4-bit register identifier
        for (int i = 0; i < X + 1; i++) //includes X
            Chip8.V[i] = Chip8.memory[Chip8.I + i];
        Chip8.pc += 2;
#if DEBUG
        if (ShowOpCodesInConsole) Debug.Log("FX65 ldr");
#endif 
    }

    /// <summary>
    /// Calls all the HF0 functions
    /// </summary>
    void _FXNN()
    {
        ushort N0NN = (ushort)(Chip8.opcode & 0xF0FF);        //8-bit constant
        op[N0NN]();
    }
    #endregion
}
