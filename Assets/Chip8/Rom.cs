using System;
using System.IO;
using UnityEngine;

public class Rom : MonoBehaviour
{
    public int memoffset = 0x200;    //but some begin at 0x600

    public void LoadRom(string filename)
    {
        try
        {
            using (FileStream fs = File.OpenRead(filename))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < buffer.Length; i++)
                {
                    //Load Into Chip8 Memory
                    Chip8.memory[i + memoffset] = buffer[i];
                }
                Debug.Log(filename + " read: " + buffer.Length + " bytes");
            }
        }
        catch(Exception ex)
        {
            Debug.Log("Error Loading Rom: " + ex.ToString());
        }
    }
    public void LoadRom(TextAsset txt)
    {
        try
        {
                byte[] buffer = txt.bytes;
                for (int i = 0; i < buffer.Length; i++)
                {
                    //Load Into Chip8 Memory
                    Chip8.memory[i + memoffset] = buffer[i];
                }
                Debug.Log("read: " + buffer.Length + " bytes");
        }
        catch (Exception ex)
        {
            Debug.Log("Error Loading Rom: " + ex.ToString());
        }
    }
}
