//https://zetcode.com/csharp/mysql/ -- MYSQL
//https://zetcode.com/csharp/json/  -- JSON

using System;
using System.IO;
using System.Security;
using MySql.Data.MySqlClient;

using System.Text.Json;
using System.Text.RegularExpressions;


namespace App
{
    
    class Program
    {
        class LoggedUser
        {
            //A class representing ongoing logged user session
            //loggedLogin being equal to "" means user is not logged in
            private MySqlConnection databaseConnection;
            private string loggedLogin;
            private int accessLevel;



            public LoggedUser(MySqlConnection connection)
            {
                //Default values after creating object instance
                databaseConnection = connection;
                loggedLogin = "";
                accessLevel = 0;
            }




            public string GetLogin()
            {
                return this.loggedLogin;
            }

            public int GetAccessLevel()
            {
                return this.accessLevel;
            }



            public int Login(string login, string password)
            {
                /*
                * - RETURN VALUES -
                * 0 - success
                * 1 - wrong input data
                * 2 - wrong login/password
                *
                * 4 - sql/other error
                */
                
                if (login != "" && password != "")
                {
                    string loginSanitized = SanitizeString(login);
                    string passwordHash = GetStringSha256Hash(password);
                    try
                    {
                        databaseConnection.Open();
                        string sql = $"SELECT COUNT(*) FROM employees WHERE login = '{loginSanitized}' AND password = '{passwordHash}';";
                        MySqlCommand command  = new MySqlCommand(sql, databaseConnection);
                        int result = (int)(long)command.ExecuteScalar();
                        if (result > 0)
                        {
                            //success
                            loggedLogin = loginSanitized;
                            sql = $"SELECT accessLevel FROM employees WHERE login = '{loginSanitized}';";
                            command  = new MySqlCommand(sql, databaseConnection);
                            accessLevel = (int)command.ExecuteScalar();
                            databaseConnection.Close();
                            return 0;
                        }
                        else
                        {
                            //wrong login/password
                            CreateLogMessage("Błędny login lub hasło przy próbie logowania ", false);
                            databaseConnection.Close();
                            return 2;
                        }
                    }
                    catch (MySqlException e)
                    {
                        //sql/other error
                        CreateLogMessage($"Błąd połączenia przy próbie zalogowania ({e.Message})", false);
                        databaseConnection.Close();
                        return 4;
                    }
                    
                }
                else
                {
                    CreateLogMessage("Podano błędne (np puste) parametry przy próbie logowania", false);
                    return 1;
                }
                
                //end login method
            }

            

            /*
                * - FUNCTION RETURN VALUES -
                * 0 - successfull
                * 1 - not logged in
                * 2 - wrong login/password
                * 3 - no permissions
                * 4 - sql/other error
                * 5 - invalid/empty parameters
            */


            public int RegisterUser(string newName, string newSurname, string newLogin, string newPassword, int newAccessLevel)
            {
               
                if (loggedLogin != "")
                {
                    if (accessLevel >= 2 && accessLevel >= newAccessLevel && newAccessLevel >= 0)
                    {
                        string newNameSanitized = SanitizeString(newName);
                        string newSurnameSanitized = SanitizeString(newSurname);
                        string newLoginSanitized = SanitizeString(newLogin);
                        string newPasswordHash = GetStringSha256Hash(newPassword);

                        try
                        {
                            databaseConnection.Open();

                            string sql = $"SELECT COUNT(*) FROM employees WHERE login = '{newLoginSanitized}'";
                            MySqlCommand command  = new MySqlCommand(sql, databaseConnection);
                            int result = (int)(long)command.ExecuteScalar();
                            databaseConnection.Close();

                            if (result > 0)
                            {
                                //no permissions
                                return 3;
                            }
                            else
                            {
                                databaseConnection.Open();
                                sql = $"INSERT INTO `employees` (`id`, `name`, `surname`, `login`, `password`, `accessLevel`) VALUES (NULL, '{newNameSanitized}', '{newSurnameSanitized}', '{newLoginSanitized}', '{newPasswordHash}', '{newAccessLevel}')";
                                command  = new MySqlCommand(sql, databaseConnection);
                                int registerResult = (int)(long)command.ExecuteNonQuery();
                                databaseConnection.Close();
                                if (registerResult == 1)
                                {
                                    //success
                                    return 0;
                                }
                                else
                                {
                                    //sql/other error
                                    CreateLogMessage("Błąd rejestracji użytkownika ", false);
                                    return 4;
                                }
                                
                            }
                        }
                        catch (Exception e)
                        {
                            //sql/other error
                            CreateLogMessage("Błąd rejestracji użytkownika " + e.Message, false);
                            return 4;
                        }
                        
                    }
                    else
                    {
                        //no permissions
                        CreateLogMessage($"Próba rejestracji użytkownika bez uprawnień, użytkownik: {loggedLogin}", false);
                        return 2;
                    }
                }
                else
                {
                    //not logged in
                    return 1;
                }


                //end registeruser method
            }
            
            public int ChangePassword(string oldPassword, string newPassword)
            {


                if (loggedLogin != "")
                {
                    try
                    {
                        string oldPasswordHash = GetStringSha256Hash(oldPassword);
                        string newPasswordHash = GetStringSha256Hash(newPassword);
                        databaseConnection.Open();
                        string sql = $"SELECT COUNT(*) FROM employees WHERE login = '{loggedLogin}' AND password = '{oldPasswordHash}';";
                        MySqlCommand command  = new MySqlCommand(sql, databaseConnection);
                        int result = (int)(long)command.ExecuteScalar();
                        if (result == 1)
                        {
                            sql = $"UPDATE employees SET password = '{newPasswordHash}' WHERE login = '{loggedLogin}'";
                            command = new MySqlCommand(sql, databaseConnection);
                            int passwordChangeResult = (int)(long)command.ExecuteNonQuery();
                            if (passwordChangeResult == 1)
                            {
                                //success
                                databaseConnection.Close();
                                return 0;
                            }
                            else
                            {
                                //sql/other error
                                CreateLogMessage($"Błąd przy zmianie hasła, użytkownik {loggedLogin}", false);
                                databaseConnection.Close();
                                return 4;
                            }
                        }
                        else
                        {
                            //wrong password
                            CreateLogMessage($"Podano błędne hasło przy zmianie hasła, użytkownik: {loggedLogin}", false);
                            return 2;
                        }
                    }
                    catch (Exception e)
                    {
                        //sql/other error
                        CreateLogMessage("Błąd przy zmianie hasła " + e.Message, false);
                        return 4;
                    }
                }
                else
                {
                    //not logged in
                    return 1;
                }
                //end change password method
            }
        
            public int ResetUsersPassword(string loginOfReseted, string password)
            {
                
                if (loggedLogin != "")
                {
                    if (loginOfReseted != "" && password != "")
                    {
                        if (accessLevel >= 3)
                        {
                            string loginOfResetedSanitized = SanitizeString(loginOfReseted);
                            string passwordHash = GetStringSha256Hash(password);
                            string newPasswordHash = GetStringSha256Hash("password");
                            try
                            {
                                databaseConnection.Open();
                                string sql = $"SELECT COUNT(*) FROM employees WHERE login = '{loggedLogin}' AND password = '{passwordHash}';";
                                MySqlCommand command  = new MySqlCommand(sql, databaseConnection);
                                int passwordCheckResult = (int)(long)command.ExecuteScalar();
                                databaseConnection.Close();
                                if (passwordCheckResult == 1)
                                {
                                    databaseConnection.Open();
                                    sql = $"UPDATE employees SET password = '{newPasswordHash}' WHERE login = '{loginOfResetedSanitized}';";
                                    command = new MySqlCommand(sql, databaseConnection);
                                    int passwordResetResult = (int)(long)command.ExecuteNonQuery();
                                    if (passwordResetResult == 1)
                                    {
                                        //success
                                        databaseConnection.Close();
                                        return 0;
                                    }
                                    else
                                    {
                                        //sql/other error
                                        CreateLogMessage($"Błąd przy resetowaniu hasła, użytkownik: {loggedLogin}", false);
                                        databaseConnection.Close();
                                        return 4;
                                    }
                                }
                                else
                                {
                                    //wrong password
                                    return 2;
                                }
                            }
                            catch (Exception e)
                            {
                                CreateLogMessage($"Błąd przy próbie resetowania hasła ({e.Message})", false);
                                return 4;
                            }

                            
                        }
                        else
                        {
                            //no permissions
                            CreateLogMessage($"Próba resetowania hasła bez uprawnień, użytkownik: {loggedLogin}", false);
                            return 3;
                        }
                        
                    }
                    else
                    {
                        //empty login/passwd
                        return 5;
                    }
                }
                else
                {
                    //not logged in
                    return 1;
                }
                
            }


        //end loggeduser class
        }






        
        public static void CreateLogMessage(string message, bool showInConsole)
        {
            //Metoda do tworzenia logów w pliku log.txt. W parametrze podajemy treść wiadomości, znacznik czasowy zostanie dodany automatycznie. 
            //Proszę o rozważne korzystanie z opcji tworzenia logów :)

            string path = "log.txt";
            DateTime timestamp = DateTime.Now;

            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.WriteLine(timestamp.ToString() + " - " + message);
            }

            if (showInConsole)
            {
                Console.WriteLine(message);
            }
            
        }

        public static string SanitizeString (string input)
        {
            Regex ruleRegex = new Regex(@"[^\w\.@-]");
            return Regex.Replace(input, $"{ruleRegex}", "");
        }

        internal static string GetStringSha256Hash(string text)
        {
            //Function for generating SHA256 hashes
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

        public static string LoadConnectionConfig(string configFile)
        {
            //Ładujemy konfigurację połączenia z bazą danych, w parametrze ściezka do pliku
            //Konfiguracja json "server" "user" "password" "database"
            try
            {
                string jsonString = File.ReadAllText(configFile);
                using JsonDocument document = JsonDocument.Parse(jsonString);
                JsonElement root = document.RootElement;

                string server = root.GetProperty("server").ToString();
                string user = root.GetProperty("user").ToString();
                string password = root.GetProperty("password").ToString();
                string database = root.GetProperty("database").ToString();

                string connectionString = $"server={server};userid={user};password={password};database={database}";
                return connectionString;
            }
            catch (Exception e)
            {
                CreateLogMessage($"Błąd ładowania konfiguracji DB z pliku {configFile} ({e.Message})", true);
                return "";
            }    
        }


        


        

        static void Main(string[] args)
        {
            string connectionString = LoadConnectionConfig("dbConfig.json");
            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
      
            LoggedUser userSession = new LoggedUser(databaseConnection);
            if (userSession.Login("amares8", "shadow") == 0)
            {
                Console.WriteLine("Witaj " + userSession.GetLogin());
            }
            else
            {
                Console.WriteLine("Logowanie nieudane");
            }
            
            Console.Write(userSession.ChangePassword("shadow", "Qwerty1@3"));
            

            
            
        }
    }
}