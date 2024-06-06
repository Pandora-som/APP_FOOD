using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace NutriPlan
{
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
        List<int> selectTables = new List<int>();
        List<Dish> dishes = new List<Dish>();

        public int typeDish;
        public int typeMeal;
        public int editTable = 0;


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
            this.Close();
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
            dbutils.connectDB = dbutils.GetDataDBConnection();
            dbutils.connectDB.Open();

            try
            {
                MySqlCommand cmd = dbutils.QueryDB("INSERT INTO " + dbutils.TableDishes + "(Name, Energy_value, Type_of_dish, Meal_type) VALUES (@name,@energy,@typeDish,@typeMeal)");
                cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = nameDish.Text;
                cmd.Parameters.Add("@energy", MySqlDbType.Float).Value = float.Parse(kkl.Text);
                cmd.Parameters.Add("@typeDish", MySqlDbType.Int32).Value = typeDish;//сюда соответственно число
                cmd.Parameters.Add("@typeMeal", MySqlDbType.Int32).Value = typeMeal;//сюда соответственно число
                cmd.ExecuteNonQuery(); //Это выполнение запрос
                long lastId = cmd.LastInsertedId; //Получаем ID вставленного блюда
                for (int i = 0; i < ingridChoise.Count; i++)
                {
                    cmd = dbutils.QueryDB("INSERT INTO " + dbutils.TableIngridientsDish + "(Ingredients_id, Dishes_id) VALUES (@ingrid,@dishId)");
                    cmd.Parameters.Add("@ingrid", MySqlDbType.Int32).Value = ingridChoise[i].ID;
                    cmd.Parameters.Add("@dishId", MySqlDbType.Int32).Value = (int)lastId;
                    cmd.ExecuteNonQuery();
                }
                for (int i = 0; i < selectTables.Count; i++)
                {
                    cmd = dbutils.QueryDB("INSERT INTO " + dbutils.TableMenuDishes + "(Tables_id, Dishes_id) VALUES (@tables_id, @dishId)");
                    cmd.Parameters.Add("@tables_id", MySqlDbType.Int32).Value = selectTables[i];
                    cmd.Parameters.Add("@dishId", MySqlDbType.Int32).Value = (int)lastId;
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Блюдо добавлено.");
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

        private void Add_btn_create(object sender, RoutedEventArgs e)
        {
            ingridChoise.Add(indgredients[IngredientsBDSearch.SelectedIndex]);
            IngredientsBDADD.Items.Clear();
            for (int i = 0; i < ingridChoise.Count; i++)
            { IngredientsBDADD.Items.Add(ingridChoise[i].Name); }
        }

        private void Delete_btn_create(object sender, RoutedEventArgs e)
        {
            ingridChoise.RemoveAt(IngredientsBDADD.SelectedIndex);
            IngredientsBDADD.Items.Clear();
            for (int i = 0; i < ingridChoise.Count; i++)
            { IngredientsBDADD.Items.Add(ingridChoise[i].Name); }
        }
        private void Add_btn_create(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("It's me, Carl!");
            ingridChoise.Add(indgredients[IngredientsBDSearch.SelectedIndex]);
            IngredientsBDADD.Items.Clear();
            for (int i = 0; i < indgredients.Count; i++)
            { IngredientsBDADD.Items.Add(indgredients[i].Name); }
        }

        private void Delete_btn_create(object sender, MouseButtonEventArgs e)
        {
            ingridChoise.Add(indgredients[IngredientsBDSearch.SelectedIndex]);
            IngredientsBDADD.Items.Clear();
            for (int i = 0; i < indgredients.Count; i++)
            { IngredientsBDADD.Items.Add(indgredients[i].Name); }
        }

        private void tpDish(object sender, RoutedEventArgs e)
        {
            string name = ((RadioButton)sender).Name;
            typeDish = int.Parse(name.Remove(0, 4));
        }

        private void tpPlan(object sender, RoutedEventArgs e)
        {
            string name = ((RadioButton)sender).Name;
            typeMeal = int.Parse(name.Remove(0, 6));
        }

        private void tpDesk(object sender, RoutedEventArgs e)
        {
            string name = ((CheckBox)sender).Name;
            int table = int.Parse(name.Remove(0, 4));
            if (((CheckBox)sender).IsChecked == true)
            {
                if (selectTables.IndexOf(table) == -1)
                    selectTables.Add(table);
            }
            else
            {
                selectTables.Remove(table);
            }
        }

        private void btnDesk(object sender, RoutedEventArgs e)
        {
            ShowAddDishes(Visibility.Hidden, Visibility.Hidden, Visibility.Visible);
            Addinfo();
            string name = ((Button)sender).Name;
            int btnNameDesk = int.Parse(name.Remove(0, 7));
            TitleWindow.Text = "Стол №" + btnNameDesk;
            editTable = btnNameDesk;

            dbutils.connectDB = dbutils.GetDataDBConnection();
            dbutils.connectDB.Open();

            LoadDishes(1);
            ShowDishes();

        }

        private void LoadDishes(int meal_type)
        {
            dishes.Clear();
            dbutils.connectDB = dbutils.GetDataDBConnection();
            dbutils.connectDB.Open();
            try
            {
                using (MySqlDataReader reader = dbutils.QueryDB(string.Format("SELECT {1}.Dishes_ID, {1}.Name, {1}.Energy_value, {1}.Type_of_dish, {1}.Meal_type FROM {0}, {1} WHERE {0}.Tables_ID = {2} AND {1}.Dishes_id = {0}.Dishes_ID AND {1}.Meal_type = {3}", dbutils.TableMenuDishes, dbutils.TableDishes, editTable, meal_type)).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Dish tmp_dish = new Dish();
                            tmp_dish.ID = reader.GetInt32(0);
                            tmp_dish.Name = reader.GetString(1);
                            tmp_dish.Cal = reader.GetFloat(2);
                            tmp_dish.TypeDish = reader.GetInt32(3);
                            tmp_dish.TypeMeal = reader.GetInt32(4);
                            dishes.Add(tmp_dish);
                        }
                    }
                }
                for (int i = 0; i < dishes.Count; i++)
                {
                    using (MySqlDataReader reader2 = dbutils.QueryDB(string.Format("SELECT {1}.Ingredients_ID, {1}.Name FROM {0}, {1} WHERE {0}.Dishes_ID = {2} && {1}.Ingredients_ID = {0}.Ingredients_ID", dbutils.TableIngridientsDish, dbutils.TableIngridients, dishes[i].ID)).ExecuteReader())
                    {
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                                dishes[i].ingredients.Add(new ReferenceItem(reader2.GetInt32(0), reader2.GetString(1)));
                            }
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

        private void ShowDishes()
        {
            gridDishes.Children.Clear();
            gridDishes.RowDefinitions.Clear();
            RowDefinition tmp_rowDef = new RowDefinition();
            tmp_rowDef.Height = new GridLength(80);
            gridDishes.RowDefinitions.Add(tmp_rowDef);
            tmp_rowDef = new RowDefinition();
            tmp_rowDef.Height = new GridLength(100);
            gridDishes.RowDefinitions.Add(tmp_rowDef);
            tmp_rowDef = new RowDefinition();
            tmp_rowDef.Height = new GridLength(100);
            gridDishes.RowDefinitions.Add(tmp_rowDef);

            TextBlock tmp_text = new TextBlock();
            tmp_text.HorizontalAlignment = HorizontalAlignment.Center;
            tmp_text.TextWrapping = TextWrapping.Wrap;
            tmp_text.Text = "№";
            tmp_text.VerticalAlignment = VerticalAlignment.Center;
            tmp_text.FontSize = 14;
            tmp_text.FontWeight = FontWeights.Bold;
            tmp_text.TextAlignment = TextAlignment.Center;
            gridDishes.Children.Add(tmp_text);
            Grid.SetRow(tmp_text, 0);
            Grid.SetColumn(tmp_text, 0);
            tmp_text = new TextBlock();
            tmp_text.HorizontalAlignment = HorizontalAlignment.Center;
            tmp_text.TextWrapping = TextWrapping.Wrap;
            tmp_text.Text = "Наименование";
            tmp_text.VerticalAlignment = VerticalAlignment.Center;
            tmp_text.FontSize = 14;
            tmp_text.FontWeight = FontWeights.Bold;
            tmp_text.TextAlignment = TextAlignment.Center;
            gridDishes.Children.Add(tmp_text);
            Grid.SetRow(tmp_text, 0);
            Grid.SetColumn(tmp_text, 1);
            tmp_text = new TextBlock();
            tmp_text.HorizontalAlignment = HorizontalAlignment.Center;
            tmp_text.TextWrapping = TextWrapping.Wrap;
            tmp_text.Text = "Ингредиенты";
            tmp_text.VerticalAlignment = VerticalAlignment.Center;
            tmp_text.FontSize = 14;
            tmp_text.FontWeight = FontWeights.Bold;
            tmp_text.TextAlignment = TextAlignment.Center;
            gridDishes.Children.Add(tmp_text);
            Grid.SetRow(tmp_text, 0);
            Grid.SetColumn(tmp_text, 2);
            tmp_text = new TextBlock();
            tmp_text.HorizontalAlignment = HorizontalAlignment.Center;
            tmp_text.TextWrapping = TextWrapping.Wrap;
            tmp_text.Text = "Калорийность";
            tmp_text.VerticalAlignment = VerticalAlignment.Center;
            tmp_text.FontSize = 14;
            tmp_text.FontWeight = FontWeights.Bold;
            tmp_text.TextAlignment = TextAlignment.Center;
            gridDishes.Children.Add(tmp_text);
            Grid.SetRow(tmp_text, 0);
            Grid.SetColumn(tmp_text, 3);

            int rC = 0;
            for (int i = 0; i < dishes.Count; i++)
            {
                rC = i + 1;
                tmp_text = new TextBlock();
                tmp_text.HorizontalAlignment = HorizontalAlignment.Center;
                tmp_text.TextWrapping = TextWrapping.Wrap;
                tmp_text.Text = "" + (i + 1);
                tmp_text.VerticalAlignment = VerticalAlignment.Center;
                tmp_text.FontSize = 14;
                tmp_text.TextAlignment = TextAlignment.Center;
                gridDishes.Children.Add(tmp_text);
                Grid.SetRow(tmp_text, rC);
                Grid.SetColumn(tmp_text, 0);

                tmp_text = new TextBlock();
                tmp_text.HorizontalAlignment = HorizontalAlignment.Center;
                tmp_text.TextWrapping = TextWrapping.Wrap;
                tmp_text.Text = dishes[i].Name;
                tmp_text.VerticalAlignment = VerticalAlignment.Center;
                tmp_text.FontSize = 14;
                tmp_text.TextAlignment = TextAlignment.Center;
                gridDishes.Children.Add(tmp_text);
                Grid.SetRow(tmp_text, rC);
                Grid.SetColumn(tmp_text, 1);

                //List
                ListBox tmp_listBox = new ListBox();
                tmp_listBox.Items.Clear();
                for (int j = 0; j < dishes[i].ingredients.Count; j++)
                {
                    tmp_listBox.Items.Add(dishes[i].ingredients[j].Name);
                }
                gridDishes.Children.Add(tmp_listBox);
                Grid.SetRow(tmp_listBox, rC);
                Grid.SetColumn(tmp_listBox, 2);

                tmp_text = new TextBlock();
                tmp_text.HorizontalAlignment = HorizontalAlignment.Center;
                tmp_text.TextWrapping = TextWrapping.Wrap;
                tmp_text.Text = "" + dishes[i].Cal;
                tmp_text.VerticalAlignment = VerticalAlignment.Center;
                tmp_text.FontSize = 14;
                tmp_text.TextAlignment = TextAlignment.Center;
                gridDishes.Children.Add(tmp_text);
                Grid.SetRow(tmp_text, rC);
                Grid.SetColumn(tmp_text, 3);

                //Button
                Button tmp_button = new Button();
                tmp_button.Name = "dishDel" + i;
                tmp_button.Content = "Удалить";
                tmp_button.Cursor = Cursors.Hand;
                tmp_button.Click += btnDishDel_Click;
                gridDishes.Children.Add(tmp_button);
                Grid.SetRow(tmp_button, rC);
                Grid.SetColumn(tmp_button, 4);
                if (rC > 1)
                {
                    tmp_rowDef = new RowDefinition();
                    tmp_rowDef.Height = new GridLength(100);
                    gridDishes.RowDefinitions.Add(tmp_rowDef);
                }
            }
        }

        private void btnDishDel_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;
            int btnNameDesk = int.Parse(name.Remove(0, 7));
            dbutils.connectDB = dbutils.GetDataDBConnection();
            dbutils.connectDB.Open();
            try
            {
                MySqlCommand cmd = dbutils.QueryDB(string.Format("DELETE FROM {0} WHERE Dishes_id = {1} AND Tables_id = {2}", dbutils.TableMenuDishes, dishes[btnNameDesk].ID, editTable));
                cmd.ExecuteNonQuery();
                int countR = 0;
                using (MySqlDataReader reader = dbutils.QueryDB(string.Format("SELECT Count(*) FROM {0} WHER Dishes_id = {1}", dbutils.TableMenuDishes, dishes[btnNameDesk].ID)).ExecuteReader())
                {
                    if (reader.Read())
                    {
                        while (reader.Read())
                        {
                            countR = reader.GetInt32(0);
                        }
                    }
                }
                if (countR == 0)
                {
                    cmd = dbutils.QueryDB(string.Format("DELETE FROM {0} WHERE Dishes_id = {1}", dbutils.TableIngridientsDish, dishes[btnNameDesk].ID));
                    cmd.ExecuteNonQuery();
                    cmd = dbutils.QueryDB(string.Format("DELETE FROM {0} WHERE Dishes_id = {1}", dbutils.TableDishes, dishes[btnNameDesk].ID));
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Блюдо удалено со стола.");
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



        private void ClosePanelDT(object sender, MouseButtonEventArgs e)
        {
            ShowAddDishes(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
            TitleWindow.Text = "Панель адмимнистрирования столов";
            dishes.Clear();
        }


        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            LoadDishes(1);
            ShowDishes();
            if (Brst.Background == Brushes.Gold)
            {
                Brst.Background = Brushes.Gold;
            }
            else
            {
                Brst.Background = Brushes.Gold;
            }
            if (Dinner.Background == Brushes.Gold)
            {
                Dinner.Background = Brushes.White;
            }
            else
            {
                Dinner.Background = Brushes.White;
            }
            if (Dinner.Background == Brushes.White)
            {
                Dinner.Background = Brushes.White;
            }
            else
            {
                Dinner.Background = Brushes.White;
            }
            if (LastDinner.Background == Brushes.Gold)
            {
                LastDinner.Background = Brushes.White;
            }
            else
            {
                LastDinner.Background = Brushes.White;
            }
            if (LastDinner.Background == Brushes.White)
            {
                LastDinner.Background = Brushes.White;
            }
            else
            {
                LastDinner.Background = Brushes.White;
            }
        }

        private void BorderMouseDown1(object sender, MouseButtonEventArgs e)
        {
            LoadDishes(2);
            ShowDishes();
            if (Dinner.Background == Brushes.Gold)
            {
                Dinner.Background = Brushes.Gold;
            }
            else
            {
                Dinner.Background = Brushes.Gold;
            }
            if (LastDinner.Background == Brushes.Gold)
            {
                LastDinner.Background = Brushes.White;
            }
            else
            {
                LastDinner.Background = Brushes.White;
            }
            if (LastDinner.Background == Brushes.White)
            {
                LastDinner.Background = Brushes.White;
            }
            else
            {
                LastDinner.Background = Brushes.White;
            }
            if (Brst.Background == Brushes.Gold)
            {
                Brst.Background = Brushes.White;
            }
            else
            {
                Brst.Background = Brushes.White;
            }
            if (Brst.Background == Brushes.White)
            {
                Brst.Background = Brushes.White;
            }
            else
            {
                Brst.Background = Brushes.White;
            }

        }

        private void BorderMouseDown2(object sender, MouseButtonEventArgs e)
        {
            LoadDishes(3);
            ShowDishes();
            if (LastDinner.Background == Brushes.Gold)
            {
                LastDinner.Background = Brushes.Gold;
            }
            else
            {
                LastDinner.Background = Brushes.Gold;
            }
            if (Dinner.Background == Brushes.Gold)
            {
                Dinner.Background = Brushes.White;
            }
            else
            {
                Dinner.Background = Brushes.White;
            }
            if (Dinner.Background == Brushes.White)
            {
                Dinner.Background = Brushes.White;
            }
            else
            {
                Dinner.Background = Brushes.White;
            }
            if (Brst.Background == Brushes.Gold)
            {
                Brst.Background = Brushes.White;
            }
            else
            {
                Brst.Background = Brushes.White;
            }
            if (Brst.Background == Brushes.White)
            {   
                Brst.Background = Brushes.White;
            }   
            else
            {   
                Brst.Background = Brushes.White;
            }
        }
    }
}

