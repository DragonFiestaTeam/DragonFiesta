﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MapleShark
{
    public sealed class FiestaPacket : ListViewItem
    {
        private DateTime mTimestamp;
        private bool mOutbound;
        private ushort mBuild = 1;
        private ushort mOpcode = 0;
        private byte[] mBuffer = null;
        private int  mType;
        private int  mheader;
        private int mCursor = 0;

        internal FiestaPacket(DateTime pTimestamp, bool pOutbound, ushort pOpcode,int type ,int Header, string pName, byte[] pBuffer)
            : base(new string[] {
                pTimestamp.ToShortTimeString(),
                pOutbound ? "Outbound" : "Inbound",
                pBuffer.Length.ToString(),
                "0x" + pOpcode.ToString("X4"),
                "Type "+type+"",
                "Header "+Header+"",
                pName })
        {
            mTimestamp = pTimestamp;
            mOutbound = pOutbound;
            mOpcode = pOpcode;
            mType = type;
            mheader = Header;
            mBuffer = pBuffer;
        }

        public DateTime Timestamp { get { return mTimestamp; } }
        public bool Outbound { get { return mOutbound; } }
        public ushort Build { get { return mBuild; } }
        public ushort Opcode { get { return mOpcode; } }
        public new string Name { set { SubItems[4].Text = value; } }
        public int Type { get { return mType; } }
        public int Header { get { return mheader; } }
        public byte[] InnerBuffer { get { return mBuffer; } }
        public int Cursor { get { return mCursor; } }
        public int Length { get { return mBuffer.Length; } }
        public int Remaining { get { return mBuffer.Length - mCursor; } }

        public void Rewind() { mCursor = 0; }

        public bool ReadByte(out byte pValue)
        {
            pValue = 0;
            if (mCursor + 1 > mBuffer.Length) return false;
            pValue = mBuffer[mCursor++];
            return true;
        }
        public bool ReadSByte(out sbyte pValue)
        {
            pValue = 0;
            if (mCursor + 1 > mBuffer.Length) return false;
            pValue = (sbyte)mBuffer[mCursor++];
            return true;
        }
        public bool ReadUShort(out ushort pValue)
        {
            pValue = 0;
            if (mCursor + 2 > mBuffer.Length) return false;
            pValue = (ushort)(mBuffer[mCursor++] |
                              mBuffer[mCursor++] << 8);
            return true;
        }
        public bool ReadShort(out short pValue)
        {
            pValue = 0;
            if (mCursor + 2 > mBuffer.Length) return false;
            pValue = (short)(mBuffer[mCursor++] |
                             mBuffer[mCursor++] << 8);
            return true;
        }
        public bool ReadUInt(out uint pValue)
        {
            pValue = 0;
            if (mCursor + 4 > mBuffer.Length) return false;
            pValue = (uint)(mBuffer[mCursor++] |
                            mBuffer[mCursor++] << 8 |
                            mBuffer[mCursor++] << 16 |
                            mBuffer[mCursor++] << 24);
            return true;
        }
        public bool ReadInt(out int pValue)
        {
            pValue = 0;
            if (mCursor + 4 > mBuffer.Length) return false;
            pValue = (int)(mBuffer[mCursor++] |
                           mBuffer[mCursor++] << 8 |
                           mBuffer[mCursor++] << 16 |
                           mBuffer[mCursor++] << 24);
            return true;
        }
        public bool ReadFloat(out float pValue)
        {
            pValue = 0;
            if (mCursor + 4 > mBuffer.Length) return false;
            pValue = BitConverter.ToSingle(mBuffer, mCursor);
            mCursor += 4;
            return true;
        }
        public bool ReadULong(out ulong pValue)
        {
            pValue = 0;
            if (mCursor + 8 > mBuffer.Length) return false;
            pValue = (ulong)(mBuffer[mCursor++] |
                             mBuffer[mCursor++] << 8 |
                             mBuffer[mCursor++] << 16 |
                             mBuffer[mCursor++] << 24 |
                             mBuffer[mCursor++] << 32 |
                             mBuffer[mCursor++] << 40 |
                             mBuffer[mCursor++] << 48 |
                             mBuffer[mCursor++] << 56);
            return true;
        }
        public bool ReadLong(out long pValue)
        {
            pValue = 0;
            if (mCursor + 8 > mBuffer.Length) return false;
            pValue = (long)(mBuffer[mCursor++] |
                            mBuffer[mCursor++] << 8 |
                            mBuffer[mCursor++] << 16 |
                            mBuffer[mCursor++] << 24 |
                            mBuffer[mCursor++] << 32 |
                            mBuffer[mCursor++] << 40 |
                            mBuffer[mCursor++] << 48 |
                            mBuffer[mCursor++] << 56);
            return true;
        }
        public bool ReadDouble(out double pValue)
        {
            pValue = 0;
            if (mCursor + 8 > mBuffer.Length) return false;
            pValue = BitConverter.ToDouble(mBuffer, mCursor);
            mCursor += 8;
            return true;
        }
        public bool ReadBytes(byte[] pBytes) { return ReadBytes(pBytes, 0, pBytes.Length); }
        public bool ReadBytes(byte[] pBytes, int pStart, int pLength)
        {
            if (mCursor + pLength > mBuffer.Length) return false;
            Buffer.BlockCopy(mBuffer, mCursor, pBytes, pStart, pLength);
            mCursor += pLength;
            return true;
        }

        public bool ReadPaddedString(out string pValue, int pLength)
        {
            pValue = "";
            if (mCursor + pLength > mBuffer.Length) return false;
            int length = 0;
            while (length < pLength && mBuffer[mCursor + length] != 0x00) ++length;
            if (length > 0) pValue = Encoding.ASCII.GetString(mBuffer, mCursor, length);
            mCursor += pLength;
            return true;
        }

        public byte[] Dump()
        {
            byte[] buffer = new byte[mBuffer.Length + 12];
            ushort size = (ushort)(mBuffer.Length);
            if (mOutbound) size |= 0x8000;
            long ticks = mTimestamp.Ticks;
            buffer[0] = (byte)ticks;
            buffer[1] = (byte)(ticks >> 8);
            buffer[2] = (byte)(ticks >> 16);
            buffer[3] = (byte)(ticks >> 24);
            buffer[4] = (byte)(ticks >> 32);
            buffer[5] = (byte)(ticks >> 40);
            buffer[6] = (byte)(ticks >> 48);
            buffer[7] = (byte)(ticks >> 56);
            buffer[8] = (byte)size;
            buffer[9] = (byte)(size >> 8);
            buffer[10] = (byte)mOpcode;
            buffer[11] = (byte)(mOpcode >> 8);
            Buffer.BlockCopy(mBuffer, 0, buffer, 12, mBuffer.Length);
            return buffer;
        }
    }
}
