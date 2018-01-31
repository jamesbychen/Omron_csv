using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace NT88Test
{
    class NT88API
    {
        //Check Code
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTFindFirst(string NTCode);

        //Get Hardware ID
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTGetHardwareID(StringBuilder hardwareID);

        //登录加密锁
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTLogin(string NTpassword);

        //存储区数据读取
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTRead(int address, int Length, byte[] pData);

        //存储区数据写入
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTWrite(int address, int Length, byte[] pData);

        //3DES解密
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NT3DESCBCDecrypt(byte[] vi, byte[] pDataBuffer, int Length);
        
        //3DES加密
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NT3DESCBCEncrypt(byte[] vi, byte[] pDataBuffer, int Length);

        //验证许可证
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTCheckLicense(int licenseCode);




        //登出加密锁
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NTLogout();
        

    }
}
