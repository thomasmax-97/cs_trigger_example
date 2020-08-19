using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace v1
{
    class Program
    {

        private static int baseAdress;

        private static int dwLocalPlayer = 0xD30B84;
        private const int m_flags = 0x104;
        private const int dwForceJump = 0x51EE650;
        

        private static string process = "csgo";
        private static string dllName = "client_panorama.dll";
        //private static int address = 0;
        

        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);

        static bool getModule()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(process);

                if(p.Length > 0)
                {
                    foreach (ProcessModule module in p[0].Modules)
                    {
                        if(module.ModuleName == dllName)
                        {

                           
                            baseAdress = (int)module.BaseAddress;
                           
                            return true;
                        }
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }catch(Exception ex)
            {
                return false;
            }
        }

        static void Main(string[] args)
        {

            
            VAMemory vam = new VAMemory(process);
           

            if (getModule())
            {
                int fJump = baseAdress + dwForceJump;
  
                dwLocalPlayer = baseAdress + dwLocalPlayer;
              
                int localPlayer = vam.ReadInt32((IntPtr)dwLocalPlayer);
               
                int a_flags = localPlayer + m_flags;





                while (true)
                {

                    while (GetAsyncKeyState(32) > 0)
                    {
                        int flags = vam.ReadInt32((IntPtr)a_flags);


                        if (flags == 257)   //stay
                        {
                            vam.WriteInt32((IntPtr)fJump, 5);   //jump
                            Thread.Sleep(20);
                            vam.WriteInt32((IntPtr)fJump, 4); //stay

                            Console.Clear();
                            Console.WriteLine("JUMP", Console.ForegroundColor = ConsoleColor.Green);
                            
                        }

                    }
                    
                    Console.Clear();
                    Console.WriteLine("STAY", Console.ForegroundColor = ConsoleColor.Green);
                    
                    Thread.Sleep(20);

                }

            }

        }

    }
}
