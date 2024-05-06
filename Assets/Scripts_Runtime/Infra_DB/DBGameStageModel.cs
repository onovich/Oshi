using System;
using MortiseFrame.Capsule;
using MortiseFrame.LitIO;

namespace Oshi {

    public struct DBGameStageModel : ISave {

        public int[] unlockedMapTypeIDsArr;
        public int lastPlayedMapTypeID;

        public void FromBytes(byte[] src, ref int offset) {
            unlockedMapTypeIDsArr = ByteReader.ReadArray<Int32>(src, ref offset);
            lastPlayedMapTypeID = ByteReader.Read<Int32>(src, ref offset);
        }

        public void WriteTo(byte[] dst, ref int offset) {
            ByteWriter.WriteArray<Int32>(dst, unlockedMapTypeIDsArr, ref offset);
            ByteWriter.Write<Int32>(dst, lastPlayedMapTypeID, ref offset);
        }

    }

}

