using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FiniteStateMachine;

namespace FiniteStateMachine
{
    class BuildAutomaton
    {
        /* Function: buildAutomaton
         * Builds an automaton based on user input
         * Input: the current (or a blank) automaton
         * Output: the new (or edited) automaton
         */
        public main.Automaton buildAutomaton(main.Automaton automaton, bool fromMain = true)
        {
            bool isInMenu = true;
            menu MenuBuilder = new menu();
            commonAutomaton common = new commonAutomaton();
            int option = 0;

            // Set Automaton Base Variables
            List<string> states = new List<string>();
            List<string> inputs = new List<string>();
            string initial = "";
            List<string> finals = new List<string>();
            List<main.Transition> transitions = new List<main.Transition>();

            while (isInMenu == true)
            {
                // display the build automaton menu based on where the user was last in the program
                if (fromMain == true)
                {
                    option = MenuBuilder.display_menu(';', "Build Automaton Menu (use arrow keys to navigate, press enter to select)", "Finite State Machine",
                        "(Back to Main Menu);Display Current Automaton;Finish Building Automaton;Set Initial State;Add Final State;Add State;Add Input Symbol;Add Transition;Remove Final State;Remove State;Remove Input Symbol;Remove Transition",
                        option);
                }
                else
                {
                    option = MenuBuilder.display_menu(';', "Build Automaton Menu (use arrow keys to navigate, press enter to select)", "Finite State Machine",
                        "(Back to Input Menu);Display Current Automaton;Finish Building Automaton;Set Initial State;Add Final State;Add State;Add Input Symbol;Add Transition;Remove Final State;Remove State;Remove Input Symbol;Remove Transition",
                        option);
                }

                // Back to Previous Menu
                if (option == 0)
                {
                    if (fromMain == true)
                        initial = "";
                    isInMenu = false;
                }
                // Display Automaton
                else if (option == 1)
                {
                    main.Automaton temp = new main.Automaton(states, inputs, initial, finals, transitions);
                    common.displayAutomaton(temp);
                }
                // Finish Building Automaton
                else if (option == 2)
                {
                    main.Automaton temp = new main.Automaton(states, inputs, initial, finals, transitions);
                    // Check if automaton is theoretically valid...
                    if (checkAutomaton (temp))
                        isInMenu= false; // if it is, exit the builder
                    else
                        Console.WriteLine("Automaton has some missing peices or has no valid paths to an acceptance state!"); // otherwise tell the user
                }
                // Set Initial State
                else if (option == 3)
                {
                    initial = getInitial();
                }
                // Add Final State
                else if (option == 4)
                {
                    finals.Add(getAcceptance());
                }
                // Add State
                else if (option == 5)
                {
                    states.Add(getState());
                }
                // Add Input Symbol
                else if (option == 6)
                {
                    inputs.Add(getInput());
                }
                // Add Transition
                else if (option == 7)
                {
                    transitions.Add(getTransition());
                }
                // Remove final state
                else if (option == 8)
                {
                    transitions.Remove(getTransition());
                }
                // Remove state
                else if (option == 9)
                {
                    transitions.Remove(getTransition());
                }
                // remove input symbol
                else if (option == 10)
                {
                    transitions.Remove(getTransition());
                }
                // remove transition
                else if (option == 11)
                {
                    transitions.Remove(getTransition());
                }
            }
            // Create Automaton variable...
            automaton = new main.Automaton(states, inputs, initial, finals, transitions);

            return automaton;
        }

        /* Function: getState
         * Finds out a new State (or a State to remove)
         * Input: n/a
         * Output: the new State
         */
        private string getState()
        {
            return ask("State?");
        }

        /* Function: getInput
         * Finds out a new Input Symbol (or a Input Symbol to remove)
         * Input: n/a
         * Output: the new Input Symbol
         */
        private string getInput()
        {
            return ask("Input Symbol?");
        }

        /* Function: getInitial
         * Finds out a new Initial State (or a Initial State to remove)
         * Input: n/a
         * Output: the new Initial State
         */
        private string getInitial()
        {
            return ask("Initial State?");
        }

        /* Function: getAcceptance
         * Finds out a new final state (or a final state to remove)
         * Input: n/a
         * Output: the new final state
         */
        private string getAcceptance()
        {
            return ask("Final State?");
        }

        /* Function: getTransition
         * Finds out a new transition (or a transition to remove)
         * Input: n/a
         * Output: the new transition
         */
        private main.Transition getTransition()
        {
            main.Transition temp = new main.Transition();
            temp.from = ask("From State?");
            temp.to = ask("To State?");
            temp.state = ask("Input Symbol?");
            return temp;
        }

        /* Function: ask
         * asks the user a question and gets their response
         * Input: the question to ask
         * Output: the user's response
         */
        private string ask (string question)
        {
            Console.WriteLine(question);
            Console.WriteLine("(Enter answer and press enter)");
            return Console.ReadLine();
        }

        /* Function: checkAutomaton
         * Checks to make sure an automaton has all the peices it needs (but not if there is at least one valid path from the initial state to the final state)
         * Input: the automaton to check
         * Output: whether the automaton is "valid"
         */
        private bool checkAutomaton (main.Automaton automaton)
        {
            // Check if there are input symbols
            if (automaton.inputSymbols.Count == 0)
                return false;

            // Check if there are states
            if (automaton.states.Count == 0)
                return false;

            // Check if there is an initial state and its in the states list
            if (automaton.initial != "")
            {
                if (automaton.states.Contains(automaton.initial) == false)
                    return false;
            }
            else return false;

            // Check if there are final states and if they're in the states list
            if (automaton.finalStates.Count > 0)
            {
                // check to see if each final state string is contained within the states list
                foreach (string final in automaton.finalStates)
                {
                    if (automaton.states.Contains(final) == false)
                        return false;
                }
            }
            else return false;

            // Check that the transitions are all valid (valid from, input symbol, and to)
            if (automaton.transitions.Count > 0)
            {
                // check each transition...
                foreach (main.Transition transition in automaton.transitions)
                {
                    // is its from contained in the states list?
                    if (automaton.states.Contains(transition.from) == false)
                        return false;

                    // is its to contained in the states list?
                    if (automaton.states.Contains(transition.to) == false)
                        return false;
                    // is its input symbol contained in the input symbols list?
                    if (automaton.inputSymbols.Contains(transition.state) == false)
                        return false;
                }
            }
            else return false;

            // Check that there is at least one valid transition path
            // create list of all transitions and fill it...
            main.TransitionPath[] transitions = new main.TransitionPath[automaton.transitions.Count];
            for (int i = 0; i < transitions.Count(); i++)
            {
                transitions[i].transition = automaton.transitions[i];
                transitions[i].hasDoneTransition = false;
            }
            // create list of initial transitions and fill it...
            List <main.Transition> initialTransitions = new List<main.Transition>();
            foreach (main.Transition transition in automaton.transitions)
            {
                if (transition.from == automaton.initial)
                    initialTransitions.Add(transition);
            }
            // now we finally can check for a good path...
            bool foundGood = false;
            foreach (main.Transition transition in initialTransitions)
            {
                if (checkForValidPath (transitions, automaton.finalStates, transition))
                {
                    foundGood = true;
                    break;
                }
            }
            if (!foundGood)
                return false;

            // if it passed all that, it must be built properly
            return true;
        }

        private bool checkForValidPath (main.TransitionPath[] transitions, List<string> finalStates, main.Transition currentTransition)
        {
            bool isGood = false;
            if (finalStates.Contains(currentTransition.from))
                isGood = true;
            else
            {
                int transIndex = -1;
                // have we been here before (and get the index of the current transition/set that we're here now)?
                for (int i = 0; i < transitions.Count(); i++)
                {
                    if (transitions[i].transition.Equals(currentTransition))
                    {
                        if (transitions[i].hasDoneTransition == false)
                        {
                            transIndex = i;
                            transitions[i].hasDoneTransition = true;
                        }
                        break;
                    }
                }

               // if we haven't been here before...
                if (transIndex != -1)
                {
                    // go through each transition in the transition list and check if its a good path...
                    foreach (main.TransitionPath transPath in transitions)
                    {
                        // are we coming from the node we're going to?
                        if (transPath.transition.from == currentTransition.to)
                        {
                            isGood = checkForValidPath(transitions, finalStates, transPath.transition);
                            if (isGood == true)
                                break;
                        }
                    }
                }
            }

            return isGood;
        }
    }
}
