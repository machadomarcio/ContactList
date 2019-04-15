using System;
using System.Runtime.InteropServices;

namespace ContactList.Domain.Base
{
    public static class NativeMethods
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreateSequential(out Guid guid);
    }

    public static class GuidGenerator
    {
        public static Guid NewId()
        {
            //Code is released into the public domain; no attribution required
            const int RPC_S_OK = 0;

            Guid guid;
            int result = NativeMethods.UuidCreateSequential(out guid);
            if (result != RPC_S_OK)
                return Guid.NewGuid();

            //Endian swap the UInt32, UInt16, and UInt16 into the big-endian order (RFC specified order) that SQL Server expects
            //See https://stackoverflow.com/a/47682820/12597
            //Short version: UuidCreateSequential writes out three numbers in litte, rather than big, endian order
            var s = guid.ToByteArray();
            var t = new byte[16];

            //Endian swap UInt32
            t[3] = s[0];
            t[2] = s[1];
            t[1] = s[2];
            t[0] = s[3];
            //Endian swap UInt16
            t[5] = s[4];
            t[4] = s[5];
            //Endian swap UInt16
            t[7] = s[6];
            t[6] = s[7];
            //The rest are already in the proper order
            t[8] = s[8];
            t[9] = s[9];
            t[10] = s[10];
            t[11] = s[11];
            t[12] = s[12];
            t[13] = s[13];
            t[14] = s[14];
            t[15] = s[15];

            return new Guid(t);
        }
    }
}