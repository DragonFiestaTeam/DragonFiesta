using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapleShark
{
    public sealed class MapleStream
    {
        private const int DEFAULT_SIZE = 4096;

        private bool mOutbound = false;
        private MapleAES mAES = null;
        private byte[] mBuffer = new byte[DEFAULT_SIZE];
        private int mCursor = 0;

        public MapleStream(bool pOutbound, ushort pBuild, byte[] pIV) { mOutbound = pOutbound; mAES = new MapleAES(pBuild, pIV); }

        public void Append(byte[] pBuffer) { Append(pBuffer, 0, pBuffer.Length); }
        public void Append(byte[] pBuffer, int pStart, int pLength)
        {
            if (mBuffer.Length - mCursor < pLength)
            {
                int newSize = mBuffer.Length * 2;
                while (newSize < mCursor + pLength) newSize *= 2;
                Array.Resize<byte>(ref mBuffer, newSize);
            }
            Buffer.BlockCopy(pBuffer, pStart, mBuffer, mCursor, pLength);
            mCursor += pLength;
        }

        public MaplePacket Read(DateTime pTransmitted, ushort pBuild)
        {
            if (mCursor < 4) return null;
            if (!mAES.ConfirmHeader(mBuffer, 0)) throw new Exception("Failed to confirm packet header");
            ushort packetSize = mAES.GetHeaderLength(mBuffer, 0);
            if (mCursor < (packetSize + 4)) return null;
            byte[] packetBuffer = new byte[packetSize];
            Buffer.BlockCopy(mBuffer, 4, packetBuffer, 0, packetSize);
            Decrypt(packetBuffer);
            mCursor -= (packetSize + 4);
            if (mCursor > 0) Buffer.BlockCopy(mBuffer, packetSize + 4, mBuffer, 0, mCursor);
            ushort opcode = (ushort)(packetBuffer[0] | (packetBuffer[1] << 8));
            Buffer.BlockCopy(packetBuffer, 2, packetBuffer, 0, packetSize - 2);
            Array.Resize(ref packetBuffer, packetSize - 2);
            Definition definition = Config.Instance.Definitions.Find(d => d.Build == pBuild && d.Outbound == mOutbound && d.Opcode == opcode);
            return new MaplePacket(pTransmitted, mOutbound, pBuild, opcode, definition == null ? "" : definition.Name, packetBuffer);
        }

        private void Decrypt(byte[] pBuffer)
        {
            mAES.Transform(pBuffer);
            for (int index1 = 1; index1 <= 6; ++index1)
            {
                byte firstFeedback = 0;
                byte secondFeedback = 0;
                byte length = (byte)(pBuffer.Length & 0xFF);
                if ((index1 % 2) == 0)
                {
                    for (int index2 = 0; index2 < pBuffer.Length; ++index2)
                    {
                        byte temp = pBuffer[index2];
                        temp -= 0x48;
                        temp = (byte)(~temp);
                        temp = temp.RollLeft(length & 0xFF);
                        secondFeedback = temp;
                        temp ^= firstFeedback;
                        firstFeedback = secondFeedback;
                        temp -= length;
                        temp = temp.RollRight(3);
                        pBuffer[index2] = temp;
                        --length;
                    }
                }
                else
                {
                    for (int index2 = pBuffer.Length - 1; index2 >= 0; --index2)
                    {
                        byte temp = pBuffer[index2];
                        temp = temp.RollLeft(3);
                        temp ^= 0x13;
                        secondFeedback = temp;
                        temp ^= firstFeedback;
                        firstFeedback = secondFeedback;
                        temp -= length;
                        temp = temp.RollRight(4);
                        pBuffer[index2] = temp;
                        --length;
                    }
                }
            }
        }
    }
}
