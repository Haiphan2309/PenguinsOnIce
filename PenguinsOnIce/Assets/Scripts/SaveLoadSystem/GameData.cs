using System.Collections.Generic;
using UnityEngine;
using System;
using GDC.Configuration;
using GDC.Enums;
using GDC.Constants;
using Unity.VisualScripting;

namespace GDC.Managers
{
    [Serializable]
    public struct GameData
    {
        public bool IsSaveLoadProcessing;

        //public int coin;
        //public string playerName;

        public void SetupData() //load
        {
            IsSaveLoadProcessing = true;
            GameDataOrigin gameDataOrigin = SaveLoadManager.Instance.GameDataOrigin;

            //coin = gameDataOrigin.coin;
            //playerName = gameDataOrigin.playerName;

            SaveLoadManager.Instance.GameDataOrigin = gameDataOrigin;
            IsSaveLoadProcessing = false;
        }
        public GameDataOrigin ConvertToGameDataOrigin() //save
        {
            IsSaveLoadProcessing = true;
            GameDataOrigin gameDataOrigin = new GameDataOrigin();

            //gameDataOrigin.coin = coin;
            //gameDataOrigin.playerName = playerName;

            IsSaveLoadProcessing = false;
            return gameDataOrigin;
        }

        #region support function
        
        #endregion
    }

    [Serializable]
    public struct GameDataOrigin
    {
        public bool IsHaveSaveData;
        //public int coin;
        //public string playerName;
    }

    [Serializable]
    public struct CacheData
    {

    }
}
