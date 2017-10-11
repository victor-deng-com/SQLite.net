using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseOperation
{
    public class SQLiteHelper
    {

        //数据库连接
        static SQLiteConnection myDataConnection;

        //static void Main(string[] args)
        //{
        //    MySQLiteHelper p = new MySQLiteHelper();
        //}

        public static void SQLiteHelperEntry()
        {
            SQLiteHelper p = new SQLiteHelper();
        }



        public SQLiteHelper()
        {
            createTable();
            fillTable();
            printHighscores();
        }

        #region 判断是否已存在本地记录，不存在则新建文件并返回结果
        //创建一个空的数据库
        //如果是新的记录，返回true，反之false
        public static bool isNewUserRecord(string username)
        {
            bool isDownload = false;
            //总用户记录文件夹路径
            string DirectoryPath = @"UserRecords";
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
            //单个的用户记录文件夹路径
            string UserDirectoryPath = @"UserRecords\"+ username;
            if (!Directory.Exists(UserDirectoryPath))
            {
                Directory.CreateDirectory(UserDirectoryPath);
            }
            //用户保存的本地sqlite记录
            string SQLitePath = @"UserRecords\"+ username + @"\"+ username +".sqlite";
            if (!File.Exists(SQLitePath))
            {
                SQLiteConnection.CreateFile(SQLitePath);
                isDownload = true;
            }
            return isDownload;
        }
        #endregion

        //创建一个连接到指定数据库
        public static void connectToDatabase(string username)
        {
            myDataConnection = new SQLiteConnection("Data Source="+username+".sqlite;Version=3;");
            myDataConnection.Open();
        }

        //在指定数据库中创建一个table
        public static void createTable()
        {
            string sql = "create table highscores (name varchar(20), score int)";
            SQLiteCommand command = new SQLiteCommand(sql, myDataConnection);
            command.ExecuteNonQuery();
        }

        //插入一些数据
        void fillTable()
        {
            string sql = "insert into highscores (name, score) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, myDataConnection);
            command.ExecuteNonQuery();

            sql = "insert into highscores (name, score) values ('Myself', 6000)";
            command = new SQLiteCommand(sql, myDataConnection);
            command.ExecuteNonQuery();

            sql = "insert into highscores (name, score) values ('And I', 9001)";
            command = new SQLiteCommand(sql, myDataConnection);
            command.ExecuteNonQuery();
        }

        //使用sql查询语句，并显示结果
        void printHighscores()
        {
            string sql = "select * from highscores order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, myDataConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
            Console.ReadLine();
        }

    }
}
