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
        public static void StartupScreen (LoggedUser userSession)
        {
            while (true)
            {
                //default startup screen
                Console.WriteLine("********************************************");
                Console.WriteLine("Witam,                                     |");
                Console.WriteLine("Proszę wybrać opcje login lub rejestracji: |");
                Console.WriteLine("********************************************\n");

                Console.WriteLine("     LOGIN:");
                Console.WriteLine("Admin {1} \n");

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
                    Console.Clear();
                    continue;
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
                Console.WriteLine("              TAK {Y}       NIE {N}");
                Console.WriteLine("");

                char.TryParse(Console.ReadLine(), out char c);
                char endPick = char.ToUpper(Convert.ToChar(c));


                if (endPick == 'Y')
                {
                    Environment.Exit(0);
                }
                else if (endPick == 'N')
                {
                    Console.Clear();

                    StartupScreen(userSession);
                    
                }
            }
        //end exit cnofirmation
        }



        public static void LoginPage(LoggedUser userSession) //Login method 
        {
            string? userInput;
            string? passInput;
            int loginResult;

            while (true)
            {
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
                        break;
                    case 2:
                        //złe hasło
                        break;
                    case 4:
                        //sql error
                        break;
                    case 5:
                        //pusty login lub hasło
                        break;
                    default:
                        //niezintyfikowany bląd
                        break;
                }
                



                
            }

        }








    }







}
