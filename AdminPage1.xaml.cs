﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NutriPlan
{
    /// <summary>
    /// Логика взаимодействия для AdminPage1.xaml
    /// </summary>
    public partial class AdminPage1 : Window
    {
        public AdminPage1()
        {
            InitializeComponent();
            CountUsers();
            ShowAddDishes(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
            TimerUser();
        }

        List<ReferenceItem> indgredients = new List<ReferenceItem>();
        List<ReferenceItem> ingridChoise = new List<ReferenceItem>();


        private void NewDish(object sender, MouseButtonEventArgs e)
        {
            ShowAddDishes(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
            Addinfo();
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowAddDishes(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
            Addinfo();
        }

        private void ShowAddDishes(Visibility addDishes, Visibility tables, Visibility dishesTable)
        {
            PanelAddDishes.Visibility = addDishes;
            PanelTables.Visibility = tables;
            PanelDishesTable.Visibility = dishesTable;
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {

        }

        private void Closetabe(object sender, MouseButtonEventArgs e)
        {
            ShowAddDishes(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
        }

        private void exit_click(object sender, MouseButtonEventArgs e)
        {
            MainWindow toUseWindow = new MainWindow();
            toUseWindow.Show();
            this.Close();
        }

        private void CountUsers()
        {
            int countRows = 0;
            dbutils.connectDB = dbutils.GetDataDBConnection();
            dbutils.connectDB.Open();
            try
            {
                using (MySqlDataReader reader = dbutils.QueryDB(string.Format("SELECT COUNT(*) FROM {0}", dbutils.TableUsers)).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            countRows = reader.GetInt32(0);
                            UpdateUsers.Text = countRows.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
            finally
            {
                dbutils.connectDB.Close();
                dbutils.connectDB.Dispose();
            }
        }

        DispatcherTimer timer = null;

        void TimerUser()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(900);
            timer.Tick += new EventHandler(timer_Elapsed);
            timer.Start();
        }
        void timer_Elapsed(object sender, EventArgs e)
        {
            CountUsers();
        }

        private void Save_btnDishBD(object sender, MouseButtonEventArgs e)
        {

        }

        private void Addinfo()
        {
            IngredientsBDSearch.Items.Clear();
            dbutils.connectDB = dbutils.GetDataDBConnection();
            dbutils.connectDB.Open();
            try
            {
                dbutils.LoadReference(dbutils.TableIngridients, indgredients, IngredientsBDSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error");
            }
            finally
            {
                dbutils.connectDB.Close();
                dbutils.connectDB.Dispose();
            }
        }
        private void Add_btn_create(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("It's me, Carl!");
            ingridChoise.Add(indgredients[IngredientsBDSearch.SelectedIndex]);
            IngredientsBDADD.Items.Clear();
            for (int i = 0; i < indgredients.Count; i++)
            {IngredientsBDADD.Items.Add(indgredients[i].Name);}
        }

        private void Delete_btn_create(object sender, MouseButtonEventArgs e)
        {
            ingridChoise.Add(indgredients[IngredientsBDSearch.SelectedIndex]);
            IngredientsBDADD.Items.Clear();
            for (int i = 0; i < indgredients.Count; i++)
            { IngredientsBDADD.Items.Add(indgredients[i].Name); }
        }
}
}
