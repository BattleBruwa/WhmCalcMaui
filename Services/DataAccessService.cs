using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services
{
    public static class DataAccessService
    {
        readonly static string dbPath = FileSystem.Current.AppDataDirectory;

        readonly static string dbName = "TargetsDb.db3";

        static SQLiteAsyncConnection db;

        static async Task Init()
        {
            if (db is not null)
            {
                return;
            }

            var dbFullPath = Path.Combine(dbPath, dbName);

            db = new SQLiteAsyncConnection(dbFullPath);

            var res = await db.CreateTableAsync<TargetModel>();

#if DEBUG
            Debug.WriteLine($"DB is {res} in {dbFullPath}");
#endif
        }

        public static async Task AddTargetAsync(TargetModel target)
        {
            await Init();

            // Проверить существует ли цель с таким именем
            var tCheck = await GetTargetByNameAsync(target.TargetName);

            if (tCheck != null)
            {
                // Добавить попап: цель с таким именем существует!
                // ------------------
                // Если пользователь хочет обновить

                await db.UpdateAsync(target);
                return;
            }

            await db.InsertAsync(target);
        }

        public static async Task RemoveTargetAsync(string name)
        {
            await Init();

            var targetToDel = await GetTargetByNameAsync(name);

            if (targetToDel != null)
            {
                await db.DeleteAsync(targetToDel);
            }
        }

        public static async Task<IEnumerable<TargetModel>> GetTargetsAsync()
        {
            await Init();

            var result = await db.Table<TargetModel>().ToListAsync();

            return result;
        }

        public static async Task<TargetModel?> GetTargetByNameAsync(string name)
        {
            await Init();
            // метод GetAsync выдает эксепшен, если не находит соотв. цель, по-этому ищем через запрос.
            var result = await db.Table<TargetModel>().Where(t => t.TargetName == name).ToListAsync();

            if(result.Count != 1)
            {
                // Ошибка при поиске цели по имени.
                return null;
            }

            return result[0];
        }
    }
}
