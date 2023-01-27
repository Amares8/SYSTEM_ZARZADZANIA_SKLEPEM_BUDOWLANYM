﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYSTEM_ZARZADZANIA_SKLEPEM_BUDOWLANYM
{
    //Class that contains methods and functions used in providing user interface.
    ///They should only be used for that purpose and for connecting to database they must 
    class UserInterface
    {

        //Default startup screen
        public static void StartupPanel (LoggedUser userSession)
        {
            while (true)
            {
                //default startup screen
                Console.Clear();
                Console.WriteLine("********************************************");
                Console.WriteLine("Witam,                                     |");
                Console.WriteLine("Proszę wybrać opcje login lub rejestracji: |");
                Console.WriteLine("********************************************\n");

                Console.WriteLine("     LOGIN:");
                Console.WriteLine("ZALOGUJ SIĘ {1} \n");

                Console.WriteLine("      INNE:");
                Console.WriteLine("PRZYPOMNIJ HASŁO {2}\n");
                Console.WriteLine("      EXIT {3}");
                Console.WriteLine("");

                //input type check
                bool isNumber = int.TryParse(Console.ReadLine(), out int userAnswer);

                if (!isNumber)
                {
                    Console.Clear();
                    Console.WriteLine("Numbers Please!");
                    Thread.Sleep(1000);
                }
                else
                {
                    switch (userAnswer)
                    {
                        case 1:
                            //zaloguj sie metoda
                            LoginPage(userSession);
                            break;

                        case 2:
                            Console.Clear();
                            Console.WriteLine("No..."); //FOR NOW 
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;

                        case 3:
                            ExitConfirmation(userSession);
                            break;

                        default:
                            Console.Clear();
                            Console.WriteLine("Invalid Input!");
                            Thread.Sleep(800);
                            break;
                    }
                }
            }

        //end of startup screen
        }

        public static void ExitConfirmation(LoggedUser userSession)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Czy jesteś pewien/na że chcesz wyjść z programu?\n");
                Console.WriteLine("              TAK {T}       NIE {N}");
                Console.WriteLine("");

                char.TryParse(Console.ReadLine(), out char c);
                char endPick = char.ToUpper(Convert.ToChar(c));


                if (endPick == 'T')
                {
                    Environment.Exit(0);
                }
                else if (endPick == 'N')
                {
                    Console.Clear();

                    StartupPanel(userSession);
                    
                }
            }
        //end exit cnofirmation
        }



        public static void LoginPage(LoggedUser userSession) //Login method 
        {
            string? userInput;
            string? passInput;
            int loginResult;

            for (int tries = 0; tries < 3; tries++)
            {
                Console.Clear();
                Console.WriteLine("********************************");
                Console.WriteLine("             LOGIN");
                Console.WriteLine("********************************\n");

                //collect user input
                Console.Write("Login: ");
                userInput = Console.ReadLine();

                Console.Write("Hasło: ");
                passInput = Console.ReadLine();

                loginResult = userSession.Login(userInput, passInput);

                switch (loginResult)
                {
                    case 0:
                        //udane logowanie
                        UserPanel(userSession);
                        break;
                    case 2:
                        Console.WriteLine("Podany login lub hasło są błędne! ");
                        Thread.Sleep(1000);
                        break;
                    case 4:
                        //sql error
                        Console.WriteLine("Wystąpił błąd z połaczeniem z bazą danych. ");
                        Thread.Sleep(1000);
                        break;
                    case 5:
                        //pusty login lub hasło
                        Console.WriteLine("Login ani hasło nie mogą być puste! . ");
                        Thread.Sleep(1000);
                        break;
                    default:
                        //inny blad
                        Console.WriteLine("Wystąpił niezidentyfikowany błąd. ");
                        Thread.Sleep(1000);
                        break;
                }
                

                
            }
            StartupPanel(userSession);
            //end LoginPage
        }


        public static void UserPanel(LoggedUser userSession)
        {
            
            
            while (true)
            {
                bool isNumber;

                Console.Clear();
                Console.WriteLine("********************************");
                Console.WriteLine(" Witaj " + userSession.GetFirstName() + " " + userSession.GetLastName());
                Console.WriteLine("********************************\n");



                Console.WriteLine("     WYBIERZ FUNKCJĘ:");
                DeletingUserPanel(userSession);
            }
            

            
        }

        public static void TransactionPanel(LoggedUser userSession)
        {

        }


        public static void AddingUserPanel(LoggedUser userSession)
        {
            string newLogin;
            string newFirstName;
            string newLastName;
            string newJobTitle;
            string newPassword;
            int newAccessLevel;
            int registrationResult;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("********************************");
                Console.WriteLine("     REJESTRACJA UŻYTKOWNIKA");
                Console.WriteLine("********************************\n");


                Console.Write("Login: ");
                newLogin = Console.ReadLine();

                Console.Write("Imię: ");
                newFirstName = Console.ReadLine();

                Console.Write("Nazwisko: ");
                newLastName = Console.ReadLine();

                Console.Write("Stanowisko: ");
                newJobTitle = Console.ReadLine();

                Console.Write("Hasło: ");
                newPassword = Console.ReadLine();

                Console.Write("Poziom uprawnień (0-3): ");
                newAccessLevel = int.Parse(Console.ReadLine());

                Console.Write("Czy na pewno chcesz dodać użytkownika? (T/N): ");

                char.TryParse(Console.ReadLine(), out char c);
                char endPick = char.ToUpper(Convert.ToChar(c));


                if (endPick == 'N')
                {
                    UserPanel(userSession);
                }


                registrationResult = userSession.RegisterUser(newLogin, newFirstName, newLastName, newJobTitle, newPassword, newAccessLevel);

                switch (registrationResult)
                {
                    case 0:
                        Console.WriteLine("Dodano użytkownika " + newLogin + " pomyslnie. ");
                        break;
                    case 3:
                        Console.WriteLine("Brak uprawnień do utworzenia użytkownika. ");
                        break;
                    case 4:
                        Console.WriteLine("Wystapił problem z połaczeniem z bazą danych. ");
                        break;
                    case 5:
                        Console.WriteLine("Podano błędnie jeden lub więcej parametrów. ");
                        break;
                    default:
                        Console.WriteLine("Wystapił niezidentyfikowany problem. ");
                        break;
                }
                Thread.Sleep(3000);
                UserPanel(userSession);
                
            }
        }


        public static void DeletingUserPanel(LoggedUser userSession)
        {
            Console.Clear();
            Console.WriteLine("********************************");
            Console.WriteLine("      KASOWANIE UŻYTKOWNIKA");
            Console.WriteLine("********************************\n");

            Console.Write("Podaj nazwę użytkownika którego chcesz usunąć: ");
            string userToDelete = Console.ReadLine();

            Console.Write("\nCzy na pewno chcesz skasować użytkownika? Operacja jest nieodwracalna! (T/N): ");

            char.TryParse(Console.ReadLine(), out char c);
            char endPick = char.ToUpper(Convert.ToChar(c));

            if (endPick == 'N')
            {
                UserPanel(userSession);
            }

            int userDeletingResult = userSession.DeleteUser(userToDelete);
            switch (userDeletingResult)
            {
                case 0:
                    Console.WriteLine("Użytkownik usunięty pomyslnie. ");
                    break;
                case 3:
                    Console.WriteLine("Nie masz uprawnień do kasowania kont użytkowników. ");
                    break;
                case 4:
                    Console.WriteLine("Nie udało się usunąć konta użytkownika. ");
                    break;
                case 5:
                    Console.WriteLine("Podano pusty lub błędny parametr! ");
                    break;
            }
            Thread.Sleep(2000);

        }

    }







}
