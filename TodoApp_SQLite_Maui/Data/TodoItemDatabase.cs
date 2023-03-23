using SQLite;
using TodoApp_SQLite_Maui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* Modifica senza merge dal progetto 'TodoApp_SQLite_Maui (net7.0-android)'
Prima:
using System.Threading.Tasks;
Dopo:
using System.Threading.Tasks;
using TodoApp_SQLite_Maui;
using TodoApp_SQLite_Maui.Models;
using TodoApp_SQLite_Maui.Controls;
*/
using System.Threading.Tasks;
using TodoApp_SQLite_Maui.Models;

/* Modifica senza merge dal progetto 'TodoApp_SQLite_Maui (net7.0-android)'
Prima:
using SQLiteNetExtensionsAsync.Extensions;
Dopo:
using SQLiteNetExtensionsAsync.Extensions;
using TodoApp_SQLite_Maui;
using TodoApp_SQLite_Maui.Controls;
using TodoApp_SQLite_Maui.Data;
*/
using SQLiteNetExtensionsAsync.Extensions;

namespace TodoApp_SQLite_Maui.Data
{
    public class TodoItemDatabase
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<TodoItemDatabase> Instance =
            new AsyncLazy<TodoItemDatabase>(async () =>
            {
                var instance = new TodoItemDatabase();

                try
                {
                    CreateTableResult result = await Database.CreateTableAsync<TodoItem>();
                }
                catch (Exception ex)
                {
                    throw;
                }

                return instance;
            });

        public TodoItemDatabase()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabaseFileName, Constants.Flags);
        }


        public Task<List<TodoItem>> GetTodoItemsAsync()
        {
            return Database.Table<TodoItem>().ToListAsync();
        }

        public Task<List<TodoItem>> GetTodoItemsNotDoneAsync()
        {
            return Database.QueryAsync<TodoItem>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        }

        public Task<int> SaveItemAsync(TodoItem item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(TodoItem item)
        {
            return Database.DeleteAsync(item);
        }

    }
}
