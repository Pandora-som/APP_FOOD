using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void signin_btn(object sender, MouseButtonEventArgs e)
        {
            dbutils.connectDB = dbutils.GetDataDBConnection();
            dbutils.connectDB.Open();
            try
            {
                using (MySqlDataReader reader = dbutils.QueryDB(string.Format("SELECT * FROM {0} WHERE Login = '{1}' AND Password = '{2}'", dbutils.TableAdmin, textBoxUserName.Text, textBoxPass.Text)).ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        AdminPage1 toUseWindow = new AdminPage1();
                        toUseWindow.Show();
                        this.Close();
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
    }
}
