using EasyCost.Databases;
using EasyCost.Databases.TableModels;
using System.Linq;

namespace EasyCost.Helpers
{
    public static class UpdateNoticeManager
    {
        public static bool? IsUpdated(string packageVersion)
        {
            var versionData = (from c in DBConnHandler.DbConnection.Table<UpdateNotice>()
                               where c.Version == packageVersion
                               select c).FirstOrDefault();

            if (versionData == null)
            {
                return null;
            }
            else
            {
                return versionData.Checked;
            }
        }

        public static void CheckNotice(string packageVersion)
        {
            DBConnHandler.DbConnection.Execute($"UPDATE UpdateNotice SET Checked = 1 WHERE Version = '{packageVersion}' ");
        }
    }
}
