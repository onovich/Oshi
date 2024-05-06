using System;
using System.Collections.Generic;
using MortiseFrame.Capsule;
using MortiseFrame.LitIO;

namespace Oshi {

    public class GameStageEntity {

        public List<int> unlockedMapTypeIDList;
        public int lastPlayedMapTypeID;

        public GameStageEntity() {
            unlockedMapTypeIDList = new List<int>();
            lastPlayedMapTypeID = -1;
        }

        public bool HasUnlocked(int mapTypeID) {
            return unlockedMapTypeIDList.Contains(mapTypeID);
        }

        public void Load(DBGameStageModel model) {
            unlockedMapTypeIDList.Clear();
            for (int i = 0; i < model.unlockedMapTypeIDsArr.Length; i++) {
                unlockedMapTypeIDList.Add(model.unlockedMapTypeIDsArr[i]);
            }
            lastPlayedMapTypeID = model.lastPlayedMapTypeID;
        }

        public DBGameStageModel Save() {
            DBGameStageModel model = new DBGameStageModel();
            model.unlockedMapTypeIDsArr = unlockedMapTypeIDList.ToArray();
            model.lastPlayedMapTypeID = lastPlayedMapTypeID;
            return model;
        }

        public void Clear() {
            unlockedMapTypeIDList.Clear();
            lastPlayedMapTypeID = -1;
        }

    }

}

