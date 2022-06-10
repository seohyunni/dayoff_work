using System;
using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data;

namespace dayoff
{
    public partial class MainWindow : Window
    {
        MySQLManager manager = new MySQLManager();

        public MainWindow()
        {
            InitializeComponent();
            manager.Initialize();
        }
            
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Dayoff_Grid(object sender, RoutedEventArgs e)
        {

            var myConString = $"SERVER=localhost ;DATABASE=dayoff_db; UID=root;PASSWORD=1234;";
            var sql = "Select * From dayoff;";

            using (MySqlConnection connection = new MySqlConnection(myConString))
            {
                connection.Open();
                using (MySqlCommand cmdSel = new MySqlCommand(sql, connection))
                {
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                    
                    da.Fill(dt);
                    DayoffList.DataContext = dt;
                }
                connection.Close();
            }
        }

        private void Amount_Input(object sender, TextCompositionEventArgs e)
        {
            //숫자만 입력
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public class SubmitEventArgs : EventArgs
        {
            public bool isSuccess;
        }

        private void Apply_Button(Object sender, RoutedEventArgs e)
        {
            SubmitEventArgs args = new SubmitEventArgs();

            string query = "insert into dayoff(dayoff_member, dayoff_date, description, use_amount)" + "values('" + inputName.Text + "','" + inputDate.Text + "','" + inputDesc.Text + "'," + inputAmount.Text + ");";
            manager.MySqlQueryExecuter(query);

            if(App.DataSaveResult == true)
            {
                args.isSuccess = true;
            }

            if(args.isSuccess == true)
            {
                MessageBox.Show("연차 사용 등록이 완료되었습니다!");
                inputName.Text = string.Empty;
                inputDesc.Text = string.Empty;
                inputDate.Text = string.Empty;
                inputAmount.Text = string.Empty;

                Dayoff_Grid(sender, e); //목록 새로고침
            }
            else
            {
                MessageBox.Show("연차 사용 등록에 실패하였습니다.");
            }
            manager.CloseMySqlConnection();
        }
    }
}
