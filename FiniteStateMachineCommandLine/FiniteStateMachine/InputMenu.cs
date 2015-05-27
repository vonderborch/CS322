using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine
{
    class InputMenu
    {
        /* Function: inputMenu
         * Displays the input menu
         * Input: the current automaton
         * Output: n/a
         */
        public void inputMenu(main.Automaton automaton)
        {
            bool isInMenu = true;
            menu MenuBuilder = new menu();
            commonAutomaton common = new commonAutomaton();
            parser Parser = new parser();
            BuildAutomaton builder = new BuildAutomaton();
            int option = 0;

            while (isInMenu == true)
            {
                // display the input menu
                option = MenuBuilder.display_menu(';', "Input Menu (use arrow keys to navigate, press enter to select)", "Finite State Machine", 
                    "(Return to Main Menu);Input String;Display Automaton;Edit Automaton;Save Automaton", option);

                // Return to Main Menu
                if (option == 0)
                {
                    isInMenu = false;
                }
                // Input String
                else if (option == 1)
                {
                    Console.WriteLine("String to check?");
                    bool temp = checkString(Console.ReadLine(), automaton); // get whether the string is valid or not
                    
                    // if it is, tell the user...
                    if (temp == true)
                        Console.WriteLine("String is valid for the Automaton!");
                    else
                        Console.WriteLine("String is not valid for the Automaton!");
                }
                // Display Automaton
                else if (option == 2)
                {
                    common.displayAutomaton(automaton);
                }
                // Edit Automaton
                else if (option == 3)
                {
                    automaton = builder.buildAutomaton(automaton, false);
                }
                // Save Automaton
                else if (option == 4)
                {
                    if (Parser.saveAutomaton(automaton))
                        Console.WriteLine("Saved Automaton!");
                    else
                        Console.WriteLine("Failed to Save Automaton!");
                }
            }
        }

        /* Function: checkString
         * checks to see whether the input string is valid for the automaton
         * Input: the input string and the automaton
         * Output: whether the string is good
         */
        private bool checkString(string input, main.Automaton automaton)
        {
            bool good = false;
            bool foundTransition = false;
            string currentState = automaton.initial;

            // loop through input string till we've finished reading the input string
            while (input != "")
            {
                string currentInput = "";

                //// INPUT SYMBOL ////
                // loop through input values in the automaton till next one is determined (if not found, string failed)
                foreach (string symbol in automaton.inputSymbols)
                {
                    int symbolLength = symbol.Length; // get length of the current input symbol
                    string temp = input.Substring(0, symbolLength); // get the value of the current next input symbol
                    // if the symbols match, we've found the current input symbol we're working with!
                    if (symbol == temp)
                    {
                        currentInput = symbol;
                        break;
                    }
                }
                // if no input symbol has been found, the string is bad
                if (currentInput == "")
                    break;

                // trim current input symbol from the input string
                input = input.TrimStart(currentInput.ToCharArray());

                //// TRANSITION ////
                // loop through transitions till one is found that matches the current state and input value (if none found, we're done looking, the string failed)
                foundTransition = false;
                foreach (main.Transition transition in automaton.transitions)
                {
                    // does the current state match the state we're looking at?
                    if (transition.from == currentState)
                    {
                        // does the current input match the input we're looking at?
                        if (transition.state == currentInput)
                        {
                            // if it does, we've found our transition!
                            currentState = transition.to;
                            foundTransition = true;
                            break;
                        }
                    }
                }
                // if we've found no input symbol, the string is bad
                if (foundTransition == false)
                    break;
            }

            //// FINAL STATE ////
            if (foundTransition == true)
            {
                // loop through final states to determine if the current state matches a valid final state
                foreach (string state in automaton.finalStates)
                {
                    // if our current state matches the currently selected final state, we've got a valid string!
                    if (currentState == state)
                    {
                        good = true;
                        break;
                    }
                }   
            }

            return good;
        }
    }
}
