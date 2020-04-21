﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using log4net;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace IntroSE.Kanban.Backend.TestsLayer
{
    class TestsController
    {

        static void Main(string[] args)
        {



            //Console.Write("Do you want to perform a restart of the program? (y/n)");
            //string choice2 = Console.ReadLine();
            //if (choice2 == "y")
            //{
            ServiceLayer.Service service = new ServiceLayer.Service();
            service.LoadData();
            service.Login("3@mashu.com", "123Abc");
            Response<Column> c1 = service.GetColumn("3@mashu.com", "Backlog");
            Response<Column> c2 = service.GetColumn("3@mashu.com", 2);
            Console.WriteLine(c1.Value.ToString());
            Console.WriteLine(c2.Value.ToString());
            Console.ReadKey();
            //}
            //else
            //{
            //    Console.WriteLine("press any key to exit the console...");
            //    Console.ReadKey();
            //}


            ////System.Environment.Exit(0);

            //Stopwatch timer = new Stopwatch();
            //timer.Start();

            ////LoadData tests
            ////LoadDataTest loadDataTest = new LoadDataTest();
            ////loadDataTest.RunTest();


            ////Register and Login tests                      
            //UserTests userTests = new UserTests(7);
            //userTests.RunAllTests();

            ////GetBoard tests
            //GetBoardTest getBoardTest = new GetBoardTest();
            //getBoardTest.RunAllTests();

            ////ColumnInvolvedTests
            //ColumnInvolvedTests columnInvolvedTests = new ColumnInvolvedTests();
            //columnInvolvedTests.RunAllTests();


            ////TaskInvolvedTests
            //TaskInvolvedTests taskInvolvedTests = new TaskInvolvedTests();
            //taskInvolvedTests.RunAllTests();

            //timer.Stop();
            //Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

            ////Console.ForegroundColor = ConsoleColor.Red;

            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("**********************************************************************");
            ////Console.WriteLine("Normal usage state test starts here.");

            //timer.Restart();

            //NormalUsageStateTest normalUsageStateTest = new NormalUsageStateTest();
            //normalUsageStateTest.RunTheTest();

            //timer.Stop();

            //Console.ForegroundColor = ConsoleColor.Green;

            //Console.WriteLine("Total execution time: " + timer.Elapsed.TotalMilliseconds.ToString("#,##0.00 'milliseconds'"));

            Console.ForegroundColor = ConsoleColor.White;

            Console.Write("do you want to clear the 'data' folder? (y/n):");
            string choice = Console.ReadLine();
            if (choice == "y")
            {
                DirectoryInfo dir1 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\");
                DirectoryInfo dir2 = new DirectoryInfo(Path.GetFullPath(@"..\..\") + "data\\Users");
                if (dir2.Exists)
                {
                    dir1.Delete(true);
                    Console.Write("'data' folder was deleted. Thank you for using Tests.");
                }
                Console.ReadKey();
            }
            else
            {
                Console.ReadKey();
            }


        }
    }
}