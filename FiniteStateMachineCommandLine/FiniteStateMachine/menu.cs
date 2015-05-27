using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FiniteStateMachine
{
    class menu
    {
        /* Function: display_menu
         * displays a menu based upon the input and gets the results of what option is selected
         * Input: the menu item deliminator, the title of the menu, the title of the program, the menu items string and (optionally) the last option selected
         * Output: the option of the menu selected
         */
        public int display_menu(char itemDelim, string menuTitle, string programTitle, string menuItems, int option = 0)
        {
            string keysSelected = "";
            string[] menu = menuItems.Split(itemDelim); // "build" menu items array
            int numItems = menu.Count(); // get the number of items in the menu
            bool isInMenu = true, isGettingInput = true;

            ////// Menu Logic //////
            while (isInMenu == true)
            {
                //// Display Menu
                // Display program and menu titles...
                Console.WriteLine(programTitle + "\n" + menuTitle +"\n\n");

                // Display menu items...
                for (int i = 0; i < menu.Count(); i++)
                {
                    // if the option is selected, indicate so
                    if (option == i)
                    {
                        Console.Write("> ");
                    }
                    // otherwise don't
                    else
                    {
                        Console.Write("  ");
                    }
                    // write the menu item
                    Console.WriteLine(menu[i]);
                }

                //// Get Input
                while (isGettingInput == true)
                {
                    keysSelected = getKeysPressed(); // get the currently selected string

                    // if a key is selected...
                    if (keysSelected != "")
                    {
                        string[] keys = keysSelected.Split('&'); // get a list of all selected keys

                        // modify option based on selected key(s)
                        if (keys.Contains("UP") == true)
                        {
                            option--; 
                        }
                        else if (keys.Contains("DOWN") == true)
                        {
                            option++;
                        }
                        else if (keys.Contains("LEFT") == true)
                        {
                            option--;
                        }
                        else if (keys.Contains("RIGHT") == true)
                        {
                            option++;
                        }
                        else if (keys.Contains("ENTER") == true)
                        {
                            isInMenu = false;
                        }
                        // make sure the option is valid
                        option = checkInput(option, numItems);
                    }
                    isGettingInput = false;
                }

                //// Reset Input Cycle
                isGettingInput = true;

                //// Clear Screen
                Console.Clear();
            }

            return option;
        }

        /* Function: getKeysPressed
         * Gets what keys are currently pressed
         * Input: n/a
         * Output: a string of all keys that we care about that are pressed
         */
        private string getKeysPressed ()
        {
            string keysPressed = "";

            ConsoleKeyInfo key = Console.ReadKey();

            // Are any Arrows pressed?
            if (key.Key == ConsoleKey.UpArrow)
            {
                keysPressed += "&UP";
            }
            if (key.Key == ConsoleKey.DownArrow)
            {
                keysPressed += "&DOWN";
            }
            if (key.Key == ConsoleKey.LeftArrow)
            {
                keysPressed += "&LEFT";
            }
            if (key.Key == ConsoleKey.RightArrow)
            {
                keysPressed += "&RIGHT";
            }
            // Is Enter Pressed?
            if (key.Key == ConsoleKey.Enter)
            {
                keysPressed += "&ENTER";
            }

            // sleep for a tiny bit then return the currently pressed key
            Thread.Sleep(5);
            return keysPressed;
        }

        /* Function: checkInput
         * Checks to make sure the input is good
         * Input: the current option selected and the number of menu items
         * Output: the potentially modified option selected
         */
        private int checkInput(int option, int numItems)
        {
            // if the option is below 0, change it to the last item in the list
            if (option < 0)
                return numItems - 1;
            // if the option is above the number of items, go to the first
            if (option >= numItems)
                return 0;
            // otherwise, just return the current option
            return option;
        }
    }
}
