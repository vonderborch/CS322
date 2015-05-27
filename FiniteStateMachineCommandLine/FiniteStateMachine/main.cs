using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using FiniteStateMachine;

namespace FiniteStateMachine
{
    class main
    {
        // Possible Program States
        enum States
        {
            loading,
            main_menu,
            build_automaton,
            load_automaton,
            input_menu,
            exit
        }

        // Transition variable
        public struct Transition
        {
            public string from;  // The state that the transition starts from
            public string to;    // The state that the transition ends up in
            public string state; // The input symbol for the transition

            // transition definition
            public Transition(string f, string t, string st)
            {
                from = f;
                to = t;
                state = st;
            }
        }

        // TransitionPath variable
        public struct TransitionPath
        {
            public Transition transition;
            public bool hasDoneTransition;

            // transitionpath definition
            public TransitionPath(bool done)
            {
                transition = new Transition();
                hasDoneTransition = done;
            }

            public TransitionPath(Transition trans, bool done)
            {
                transition = trans;
                hasDoneTransition = done;
            }
        }

        // Automaton variable
        public struct Automaton
        {
            public List<string> states;          // the list of states in the automaton
            public List<string> inputSymbols;    // the list of input symbols of the automaton
            public string initial;               // the initial state of the automaton
            public List<string> finalStates;     // the possible final states of the automaton
            public List<Transition> transitions; // the possible transitions for the automaton
            
            // "blank" automaton definition
            public Automaton(string ini)
            {
                states = new List<string>();
                inputSymbols = new List<string>();
                initial = ini;
                finalStates = new List<string>();
                transitions = new List<Transition>();
            }

            // "filled" automaton definition
            public Automaton(List<string> st, List<string> iS, string ini, List<string> fS, List<Transition> tra)
            {
                states = st;
                inputSymbols = iS;
                initial = ini;
                finalStates = fS;
                transitions = tra;
            }

            // add a state to the list of states
            public void addState(string t)
            {
                states.Add(t);
            }
            // add an input symbol to the list of input symbols
            public void addInput(string t)
            {
                inputSymbols.Add(t);
            }
            // set the initial state
            public void setInitial(string t)
            {
                initial = t;
            }
            // add a final state to the list of final states
            public void addFinal(string t)
            {
                finalStates.Add(t);
            }
            // add a transition to the list of transitions
            public void addTransition(Transition t)
            {
                transitions.Add(t);
            }
        }


        /* Function: Main
         * Controls the program
         * Input: n/a
         * Output: n/a
         */
        static void Main(string[] args)
        {
            menu MenuBuilder = new menu(); // define menubuilder as class menu
            States state = States.loading; // define state as class States
            Automaton automaton = new Automaton(); // define automaton as an automaton struct
            BuildAutomaton Builder = new BuildAutomaton(); // define Builder as class BuildAutomaton
            LoadAutomaton Loader = new LoadAutomaton(); // define Loader as class LoadAutomaton

            int option = 0; // current option selected on the menu
            bool isRunning = true; // is the program running?

            while (isRunning == true)
            {
                // LOAD
                if (state == States.loading)
                {
                    state = States.main_menu;
                }
                // MAIN MENU
                else if (state == States.main_menu)
                {
                    // Display menu and get results of option selected
                    option = MenuBuilder.display_menu(';', "Main Menu (use arrow keys to navigate, press enter to select)", "Finite State Machine", 
                        "Build Automaton;Load Automaton;Exit", option);

                    // Build Automaton
                    if (option == 0)
                    {
                        state = States.build_automaton;
                    }
                    // Load Automaton
                    else if (option == 1)
                    {
                        state = States.load_automaton;
                    }
                    // Exit Program
                    else if (option == 2)
                    {
                        state = States.exit;
                    }
                }
                // BUILD AUTOMATON
                else if (state == States.build_automaton)
                {
                    // set automaton to the results of a call to buildAutomaton
                    automaton = Builder.buildAutomaton(new Automaton(""));
                    
                    // if no automaton was built, return to the main menu
                    if (automaton.initial == "")
                        state = States.main_menu;
                    // otherwise, go to the input menu
                    else
                        state = States.input_menu;
                }
                // LOAD AUTOMATON
                else if (state == States.load_automaton)
                {
                    string[] menuItemsRaw = Directory.GetFiles("../../files/"); // get all files from the files directory
                    string file = Loader.loadMenu(menuItemsRaw); // get the file selected from a call to the load menu function
                    // if no file is selected, return to the main menu
                    if (file == "")
                        state = States.main_menu;
                    // otherwise...
                    else
                    {
                        // load an automaton from the selected file
                        automaton = Loader.loadAutomaton(file);
                        // if the load is successful, go to the input menu
                        if (automaton.initial != null)
                            state = States.input_menu;
                        // otherwise, return to the main menu
                        else
                            state = States.main_menu;
                    }
                }
                // INPUT MENU
                else if (state == States.input_menu)
                {
                    InputMenu inputmenu = new InputMenu(); // create a new InputMenu class instance
                    inputmenu.inputMenu(automaton); // go to the input menu with the current automaton

                    state = States.main_menu; // return to the main menu
                }
                // EXIT
                else if (state == States.exit)
                {
                    // end the program
                    isRunning = false;
                }
            }
        }
    }
}
