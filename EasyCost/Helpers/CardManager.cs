using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using System.Collections.Generic;
using System.Linq;

namespace EasyCost.Helpers
{
    public static class CardManager
    {
        public static List<CardMaster> GetCardList()
        {
            return DBConnHandler.DbConnection.Table<CardMaster>().OrderBy(x => x.CardName).ToList();
        }

        public static void SaveCardMaster(CardMaster aCardMaster)
        {
            DBConnHandler.DbConnection.Insert(aCardMaster);
        }

        public static void UpdateCardMaster(CardMaster aCardMaster)
        {
            DBConnHandler.DbConnection.Update(aCardMaster);
        }

        public static void DeleteCardMaster(string aCardName)
        {
            DBConnHandler.DbConnection.Execute($"DELETE FROM CardMaster WHERE CardName = '{aCardName}' ");
        }
    }
}
