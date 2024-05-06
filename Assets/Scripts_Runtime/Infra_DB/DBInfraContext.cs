using System;
using System.Collections.Generic;
using MortiseFrame.Capsule;

namespace Oshi {

    public class DBInfraContext {

        public SaveCore saveCore;
        Dictionary<Type, byte> saveDict;

        public DBInfraContext() {
            saveDict = new Dictionary<Type, byte>();
            saveCore = new SaveCore();

            var key = saveCore.Register(typeof(DBGameStageModel), "DBGameStageSave");
            saveDict.Add(typeof(DBGameStageModel), key);
        }

        public byte GetKey(Type type) {
            return saveDict[type];
        }

    }

}