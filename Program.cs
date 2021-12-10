using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InteractiveAssignment2._0
{
    class Program
    {

        static int storyLength = 17;
        static int pageNum; // WHICH PAGE IS DISPLAYED

        static string[] story = new string[storyLength]; // STORY LIES HERE
        static string[] splitText;

        static int playerChoiceA;  // CHOICE A
        static int playerChoiceB;  // CHOICE B

        static string selection;
        static string textToSplit;
        static string saveData;

        static string SourceData;
        static byte[] tmpSource;
        

        static string savePath = @"SaveFile.txt";
        static string storyPath = @"StoryFile.txt";

        static bool isGameOver;
        static bool quitGame;

        static void Main(string[] args)
        {

            pageNum = 0;
            story = File.ReadAllLines(@"StoryFile.txt");

            quitGame = false;

            //------------------------------------

            
            SaveGameInit();

            while (isGameOver == false)
            {

               Story();



                //gameplay loop
                while (isGameOver == false) //GAME LOOP
                {
                    ErrorChecking(); // Checks to see if pageNum is in range
                    HasDelimiters(); // checks if page is an ending or not
                    PrintPageText(); // WRITTEN TEXT                 
                    UserSelection(); // PLAYERS CHOICE
                }

                Console.Clear();

                if (quitGame == false)
                {
                    isGameOver = false;
                }

            }

        }

        // ------------------------- ERROR CHECK / HASH ------------------------------

        static void ErrorChecking()
        {
            if (!File.Exists(storyPath)) // checks if file exists
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR - story.txt not found. Press any key to quit.");
                Console.ReadKey(true);

                quitGame = true;
                isGameOver = true;
            }

            if (story[pageNum].Length == 0) // ensures page isn't empty
            {
                isGameOver = true;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR - String " + pageNum + " is empty. Press any key to quit.");
                pageNum = 0;
                Console.ReadKey(true);

                quitGame = true;
                isGameOver = true;
            }

            if (pageNum > story.Length || pageNum < story.Length - story.Length) // checks if story is witthin array range
            {
                Console.Clear();
                pageNum = 0;
                isGameOver = true;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR - Page Number is outside the established range of the story. Press any key to quit to menu");
                Console.ReadKey(true);

                quitGame = true;
                isGameOver = true;
            }

            
        }


  






        // -------------- GAMEPLAY LOOP ------------------------------------
        static void SplitText() // splits PlotText(); into readable text && decision values
        {
            textToSplit = story[pageNum];

            textToSplit.Split(';'); // splits string into new strings on ';' characters
            splitText = textToSplit.Split(';'); // creates an array of strngs based off of textToSplit

            int.TryParse(splitText[splitText.Length - 2], out playerChoiceA); // Checks the last two strings to be used in page navigation - errors blocked by hash code
            int.TryParse(splitText[splitText.Length - 1], out playerChoiceB); // ---------------------------------------------------------------------------------------
        }

        static void HasDelimiters() // checks to see if string has dilimiters
        {
            if (story[pageNum].Contains(";"))
            {
                SplitText();
            }
            else
            {
                isGameOver = true;
            }
        }


        static void PrintPageText() // prints the story and options
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (pageNum == 0)
            {
                Console.WriteLine("Introduction");
            }
            else
            {
                Console.WriteLine("Page " + pageNum); // Display page number
            }

            Console.ForegroundColor = ConsoleColor.Cyan;

            if (isGameOver)
            {
                Print(story[pageNum]);
            }
            else
            {
                for (int i = 0; i < splitText.Length - 2; i++)
                {
                    Print(splitText[i]);
                }

            }
        }

        static void UserSelection() // switich statement for player choice **Decided to keep the enter key and press enter style system as personal choice
        {
            selection = Console.ReadLine();

            if (!isGameOver)
            {
                switch (selection)
                {
                    case "1":

                        pageNum = playerChoiceA;
                        Console.Clear();

                        break;

                    case "2":

                        pageNum = playerChoiceB;
                        Console.Clear();

                        break;

                    case "3":

                        SaveGame();
                        Console.Clear();
                        Console.WriteLine("Game Saved!");

                        break;

                    case "4":

                        isGameOver = true;

                        break;

                    default: // basically the else statement

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(selection + " not recognised, Press any key to try again");
                        Console.ReadKey();
                        Console.Clear();

                        break;
                }
            }
        }

        // ---------------------------- TEXT DISPLAY ------------------------------------

        public static void Print(string text, int delay = 25) // slowly prints out text
        {
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        // ----------------------------- SAVE GAME --------------------------------------

        static void SaveGame() //checks to see if file exists and saves
        {
            if (File.Exists(savePath))
            {
                saveData = pageNum.ToString();
                File.WriteAllText(savePath, saveData);
            }
            else
            {
                SaveGameInit();
            }
        }
        static void SaveGameInit() // runs at start to check if save file is present as well as if save file is not present, it creates a new save.txt
        {
            if (!File.Exists(savePath))
            {
                saveData = pageNum.ToString();
                File.WriteAllText(savePath, saveData);
            }
        }

        static void PlotText()
        {
            // Story 
            story[0] = "You are an experienced traveller who has stumbled upon a mysterious cave. \nYour curiousity tells you to enter.\nPress 1 or 2 to start your journey into the unknown. ;  ;Press 3 To Save Your Game \nPress 4 to Quit;1;1";
            story[1] = "Quick! The Giant Bat Flies Towards you!;1: Run Away;2: Move To The Right And Swing At It ;2;4";
            story[2] = "You Run Away But It Catches Up With You!;1: Keep Running, Must Find An Exit!;2: Dodge and Attack!;3;4";
            story[3] = "You Try your Hardest But Finding An Exit Seems Unlikly, The Bat Jumps On Top Of You!\nYou Have A Knife Easily Accessable In Your Pocket. ;1: Push It Off And Get up!;2: Stab The Bat!;5;7";
            story[4] = "You Dodged a Dangerous Attack!, And You Swing At It. Its Stunned, You Feel The Knife in Your Pocket.;1: Run Away!;2: Attack It Again With The Knife!;3;6";
            story[5] = "Putting All Your Power Into Pushing Him Off You Get Winded,\nThe Bat Launches Itself Onto You With A Loud Screech Putting Its Razor Sharp Fangs Into Your Neck.\nThe Bat Flies Off Seemingly Getting Its Fill.\nYou Stumble Around Holding your Neck Gushing Trying To Keep Yourself Alive.\nYou Collapse And Slowly Bleed Out.\nYou Have Died"; // death 1
            story[6] = "You Go In For A Second Attack!;1: This Is Your Chance To Find An Exit!;2: Pin Down The Bat And Stab It!;3;8";
            story[7] = "You Stab The Bat As It Gives Out A Huge Screech;1: Throw A Rock At It!;2: This Is Your Chance To Run!;1;2";
            story[8] = "You've Done It! The Bat Is Finished! \n You Continue On Your Path To Find The Mysteries Roumored To Be In This Cave\n You Stumble Upon A Rickity Bridge. Water Can Be Heard Below. Being The Only Way Forward You Try To Cross.\n The Ropes Holding It Up Start To Snap! ;1: Run! Try To Make It To The End Before It Snaps!;2: Jump To The Chasm Below!;14;9";
            story[9] = "You Fall Down And Safly Reach The Bottom ;1: Climb Back Up;2: Keep Walking in Chasm ;11;12";
            story[10] = "You Fall Deep Into The Chasm, Water Breaks Your Fall,;1: Climb to the Top;2: Walk Down Further Into Chasm;11;12";
            story[11] = "You Start To Climb Back Up When Your Foot Slips Think Fast!;1: Jump To Small Ledge;2: Stab Your Knife In The Wall;13;16";
            story[12] = "You WAlk Further Into The Chasm With Worry Overtaking You. ;1: Continue to the wings;2:Turn Around And Climb To The Top;13;11";
            story[13] = "You Jump And Try Your Hardest to balence On The Ledge But It Breaks And Send You Tumbling Down Into The Dark Depths. You Did Not Survive The Fall"; // death 2
            story[14] = "You Sprint To The End With A Leap Near The End As it Breaks! You've Made It To The Other Side!\nYou Keep Walking What Feels like A Mile. You Find A Cat Sized Chrystal, The Cave Is Full Of Them! You Cannot Take them All, You Only Have Room And Strength For One After Everything You've Been Through. \n You Keep Walking In The Endless Cave, Your Legs Begin To Burn From Exaustion. \n After Hours Of Walking You See A Light.. Hope Fills You With The Strength To Move On. You See What Looks Like And Exit And Hear Voices? \n Through A Small Hole You Call For Help And People Respond a Few Hours Later A rescue Team and Reporters Are On Scene To Hear Your Story And Take Car of your Wounds. The End."; // win 1
            story[15] = "You Travel Through The Cave And See A Light At the End of the Tunnel, You Hear Voices And It Fills You With Joy. You Were FOUND. The End."; // win 2
            story[16] = "You Stab Your Knife Into The Soft Wall And Are Able To Regain Your Balence And Climb To The Top. ;1: Go Back Down;2: Travel Further Into Cave;10;15";
        }

        //static void PageText()
       // {
          //  Console.ForegroundColor = ConsoleColor.White;
         //   Console.WriteLine("Page " + pageNum);
         //   Console.ForegroundColor = ConsoleColor.Yellow;
          //  if (isGameOver)
         //   {
         //       Console.WriteLine(story[pageNum]);
         //   }
          //  else
          //  {
          //      Console.WriteLine(splitText[0]);
          //      Console.WriteLine(splitText[1]);
          //      Console.WriteLine(splitText[2]);
         //   }
      //  }

        static void UserChoice()
        {
            selection = Console.ReadLine();

            if (!isGameOver)
            {
                switch (selection)
                {
                    case "1":
                        pageNum = playerChoiceA;
                        Console.Clear();
                        break;

                    case "2":
                        pageNum = playerChoiceB;
                        Console.Clear();

                        break;

                    case "3":
                        Console.WriteLine("Save Game Option Coming Soon!");
                        break;

                    case "4":
                        Environment.Exit(0);
                        break;

                    default: // basically the else statement
                        Console.WriteLine(selection + " Player Input Key Not Avalible, Type 1 - 4 To Continue LOST");
                        break;
                }
            }
        }

        static void CheckText()
        {
            if (story[pageNum].Contains(";"))
            {
                SplitText();
            }
            else
            {
                isGameOver = true;
            }
        }

        //static void SplitText()
       // {
           // textToSplit = story[pageNum];
           // textToSplit.Split(';');
           // splitText = textToSplit.Split(';');
           // playerChoiceA = int.Parse(splitText[3]);
            //playerChoiceB = int.Parse(splitText[4]);
       // }

        static void Story()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" @@            @@          @@@@        @@@@@@@@@@          " +
                "\n @@          @@  @@       @@               @@                            " +
                "\n @@          @@  @@       @@               @@            " +
                "\n @@          @@  @@        @@@             @@              " +
                "\n @@          @@  @@          @@@           @@             " +
                "\n @@@@@@@@      @@          @@@@            @@               ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("For The Best Experience Please Play In Full Screen.");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press 1 To Begin\nPress 2 To Continue From Last Save Point\nPress 3 To Exit Game");
            Console.WriteLine(" ");
            Console.WriteLine("Lost By Brandon VanBuskirk");
            Console.WriteLine(" ");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;

            MenuSelect();
        }
        static void MenuSelect()
        {
            string menuSelect = Console.ReadLine();

            switch (menuSelect)
            {
                case "1":

                    pageNum = 0;
                    Console.Clear();
                    break;

                case "2":

                    Console.Clear();
                    saveData = File.ReadAllText(savePath);
                    pageNum = int.Parse(saveData);

                    break;

                case "3":

                    quitGame = true;
                    isGameOver = true;

                    break;

                default:

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(menuSelect + " Not recognised, Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    isGameOver = true;

                    break;
            }


        }
    }
}


