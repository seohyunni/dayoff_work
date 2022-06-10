using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace dayoff
{
    class MySQLManager
    {
        public void Initialize()
        {
            //path를 MySqlConnection의 인자로 넘겨 연결
            Debug.WriteLine("DataBase Initialize");

            string connectionPath = $"SERVER=localhost ;DATABASE=dayoff_db; UID=root;PASSWORD=1234;";
            App.connection = new MySqlConnection(connectionPath);
        }

        // DataBase Connection
        public bool OpenMySqlConnection()
        {
            try
            {
                App.connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        Debug.WriteLine("서버에 접속할 수 없습니다");
                        break;
                    case 1045:
                        Debug.WriteLine("서버 ID와 Password를 확인 해주세요.");
                        break;
                }
                return false;
            }
        }

        // DataBase Close
        public bool CloseMySqlConnection()
        {
            try
            {
                App.connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        // Insert
        public void MySqlQueryExecuter(string userQuery)
        {
            string query = userQuery;

            if (OpenMySqlConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, App.connection);

                if (command.ExecuteNonQuery() == 1)
                { //값 저장 성공
                    App.DataSaveResult = true;
                }
                else
                { //값 저장 실패
                    App.DataSaveResult = false;
                }

                CloseMySqlConnection();
            }
          
        }

       
    }
}
