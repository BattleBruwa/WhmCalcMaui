﻿using System.Diagnostics;
using System.Text.Json;
using SQLite;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services
{
    public class DataAccessService : IDataAccessService
    {
        private readonly string dbPath = FileSystem.Current.AppDataDirectory;

        private readonly string dbName = "TargetsDb.db3";

        private SQLiteAsyncConnection db;

        private async Task Init()
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

        public async Task<int> AddTargetAsync(TargetModel target)
        {
            await Init();

            // Проверить существует ли цель с таким именем
            var tCheck = await GetTargetByNameAsync(target.TargetName);

            if (tCheck != null)
            {
                // Цель с таким именем существует
                return 0;
            }

            return await db.InsertAsync(target);
        }

        public async Task<int> UpdateTargetAsync(TargetModel target)
        {
            await Init();

            return await db.UpdateAsync(target);
        }

        public async Task<int> RemoveTargetAsync(string name)
        {
            await Init();

            var targetToDel = await GetTargetByNameAsync(name);

            if (targetToDel != null)
            {
                return await db.DeleteAsync(targetToDel);
            }

            return 0;
        }

        public async Task<IEnumerable<TargetModel>> GetTargetsAsync()
        {
            await Init();

            var result = await db.Table<TargetModel>().ToListAsync();

            return result;
        }

        public async Task<TargetModel?> GetTargetByNameAsync(string name)
        {
            await Init();
            // метод GetAsync выдает эксепшен, если не находит соотв. цель, по-этому ищем через запрос.
            var result = await db.Table<TargetModel>().Where(t => t.TargetName == name).ToListAsync();

            if (result.Count != 1)
            {
                // Ошибка при поиске цели по имени.
                return null;
            }

            return result[0];
        }
    }
}
