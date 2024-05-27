using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            ShowAddDishes(Visibility.Hidden, Visibility.Visible, Visibility.Hidden);
        }


        private void NewDish(object sender, MouseButtonEventArgs e)
        {
            ShowAddDishes(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
        }

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ShowAddDishes(Visibility.Visible, Visibility.Hidden, Visibility.Hidden);
        }

        private void ShowAddDishes (Visibility addDishes, Visibility tables, Visibility dishesTable)
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
    }
}
