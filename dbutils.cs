using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace NutriPlan
{
    class dbutils
    {
        public static MySqlConnection connectDB { set; get; }
        public static string TableAdmin = "admins";
        public static string TableAnswerVariant = "answer_variant";
        public static string TableDishes = "dishes";
        public static string TableIngridients = "ingredients";
        public static string TableIngridientsDish = "ingredients_dishes";
        public static string TableMeal = "meal_type";
        public static string TableMenu = "menu";
        public static string TableMenuDishes = "menu_dishes";
        public static string TableTab = "tables";
        public static string TableTypesDishes = "types_of_dishes";
        public static string TableTypesTables = "types_of_tables";
        public static string TableUsers = "users";
        public static DBConnectString connectString = new DBConnectString();

        public static MySqlConnection GetDBConnection(string host, int port, string database, string username, string password)
        {
            String connString = String.Format("Server={0};Database={1};port={2};User Id={3};password={4};convert zero datetime=True", host, database, port, username, password);
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }

        public static MySqlCommand QueryDB(string query)
        {
            MySqlCommand cmd = connectDB.CreateCommand();
            cmd.CommandText = query;
            return cmd;
        }

        public static MySqlConnection GetDataDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "nutriplan";
            string username = "viewer";
            string password = "GfhjkmJnUs88";

            return GetDBConnection(host, port, database, username, password);
        }

        public static void LoadReference(string TableName, List<ReferenceItem> list, ComboBox comboBox)
        {
            using (MySqlDataReader reader = dbutils.QueryDB("SELECT * FROM " + TableName).ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new ReferenceItem(reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }
            if (comboBox != null)
            {
                foreach (ReferenceItem item in list)
                {
                    comboBox.Items.Add(item.Name);
                }
            }
        }
    }

    class DBConnectString
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public DBConnectString()
        {
            Host = "localhost";
            Port = 3306;
            Database = "nutriplan";
            Username = "viewer";
            Password = "GfhjkmJnUs88";
        }
    }

    public class ReferenceItem
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public ReferenceItem(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
    public class Dish
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public float Cal { set; get; }
        public int TypeDish { set; get; }
        public int TypeMeal { set; get; }
        public List<ReferenceItem> ingredients = new List<ReferenceItem>();
    }
}
