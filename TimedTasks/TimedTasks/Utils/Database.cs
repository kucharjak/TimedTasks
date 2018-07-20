using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using TimedTasks.Interfaces;
using Xamarin.Forms;
using SQLite;
using TimedTasks.ViewModels;

namespace TimedTasks.Utils
{
    public static class Database
    {
        /// <summary>
        /// Cesta k souboru databáze.
        /// </summary>
        private const string DatabaseName = "data.db3";

        public static string PersonalPath;

        public static string DatabasePath
        {
            get
            {
                return Path.Combine(PersonalPath, DatabaseName);
            }
        }

        /// <summary>
        /// Příznaky, které se použijí při otevření spojení s databází.
        /// </summary>
        private const SQLiteOpenFlags openFlags = SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.FullMutex;

        public static void InitDB()
        {
            IPlatformInfo platformInfo = DependencyService.Get<IPlatformInfo>();
            PersonalPath = platformInfo.GetPersonalPath();
        }

        /// <summary>
        /// Vytvoří databázi a všechny potřebné tabulky, pokud již existují, tak se příkazy automaticky ignorují.
        /// </summary>
        public static void CreateDB()
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.CreateTable<TaskViewModel>();
            }
        }

        /// <summary>
        /// Vrátí z databáze veškeré úkoly
        /// </summary>
        /// <param name="skipFinished">Příznak jestli chci vrátit všechny úkoly, nebo pouze nesplněné.</param>
        public static List<TaskViewModel> SelectAllTasks(bool skipFinished = false)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                if (skipFinished)
                    return db.Table<TaskViewModel>().Where(task => !task.Finished).ToList();
                else
                    return db.Table<TaskViewModel>().ToList();
            }
        }

        /// <summary>
        /// Vrátí z databáze veškeré úkoly z daného dne.
        /// </summary>
        /// <param name="skipFinished">Příznak jestli chci vrátit všechny úkoly, nebo pouze nesplněné.</param>
        public static List<TaskViewModel> SelectTasks(DateTime day, bool skipFinished = false)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                if (skipFinished)
                    return db.Table<TaskViewModel>().Where(task => task.DueDate == day.Date && !task.Finished).ToList();
                else
                    return db.Table<TaskViewModel>().Where(task => task.DueDate == day.Date).ToList();
            }
        }

        /// <summary>
        /// Vloží do databáze úkol.
        /// </summary>
        /// <param name="task"></param>
        public static void InsertTask(TaskViewModel task)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.Insert(task);
            }
        }

        /// <summary>
        /// Vloží do databáze všechny úkoly.
        /// </summary>
        /// <param name="tasks"></param>
        public static void InsertMultipleTasks(List<TaskViewModel> tasks)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.BeginTransaction();
                foreach (var task in tasks)
                    db.Insert(task);
                db.Commit();
            }
        }

        /// <summary>
        /// Aktualizuje záznam o úkolu v DB - je potřeba, aby měl úkol správné id!
        /// </summary>
        /// <param name="task"></param>
        public static void UpdateTask(TaskViewModel task)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.Update(task);
            }
        }

        /// <summary>
        /// Smaže z databáze všechny úkoly.
        /// </summary>
        /// <param name="taskID"></param>
        public static void DeleteAllTasks()
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.DeleteAll<TaskViewModel>();
            }
        }

        /// <summary>
        /// Smaže z databáze úkol s daným id.
        /// </summary>
        /// <param name="taskID"></param>
        public static void DeleteTask(TaskViewModel task)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.Delete(task);
            }
        }

        /// <summary>
        /// Smaže z databáze úkol s daným id.
        /// </summary>
        /// <param name="taskID"></param>
        public static void DeleteTask(int taskID)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.Delete<TaskViewModel>(taskID);
            }
        }

        /// <summary>
        /// Smaže z databáze hromadně několik úkolů podle id.
        /// </summary>
        /// <param name="taskIDs"></param>
        public static void DeleteMultipleTasks(int[] taskIDs)
        {
            using (var db = new SQLiteConnection(DatabasePath, openFlags))
            {
                db.BeginTransaction();
                foreach (var id in taskIDs)
                    db.Delete<TaskViewModel>(id);
                db.Commit();
            }
        }
    }
}
