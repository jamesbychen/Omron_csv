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

        //��¼������
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTLogin(string NTpassword);

        //�洢�����ݶ�ȡ
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTRead(int address, int Length, byte[] pData);

        //�洢������д��
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTWrite(int address, int Length, byte[] pData);

        //3DES����
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NT3DESCBCDecrypt(byte[] vi, byte[] pDataBuffer, int Length);
        
        //3DES����
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NT3DESCBCEncrypt(byte[] vi, byte[] pDataBuffer, int Length);

        //��֤���֤
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int NTCheckLicense(int licenseCode);




        //�ǳ�������
        [DllImport("NT88.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int NTLogout();
        

    }
}
