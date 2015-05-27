using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine
{
    class commonAutomaton
    {
        /* Function: displayAutomaton
         * Displays the passed automaton
         * Input: the automaton to display
         * Output: n/a
         */
        public void displayAutomaton(main.Automaton temp)
        {
            // create output variable
            string output = "";

            // output the initial state
            output += "\n******** Initial State ********\n";
            output += temp.initial;

            // output the final states
            output += "\n******** Acceptance State(s) ********\n";
            foreach (string i in temp.finalStates)
            {
                output += i + "\n";
            }

            // output the states
            output += "\n******** State(s) ********\n";
            foreach (string i in temp.states)
            {
                output += i + "\n";
            }

            // output the Input Symbols
            output += "\n******** Input Symbol(s) ********\n";
            foreach (string i in temp.inputSymbols)
            {
                output += i + "\n";
            }

            // output the Transitions
            output += "\n******** Transition(s) ********\n";
            foreach (main.Transition transition in temp.transitions)
            {
                output += "-- Transition --\n";
                output += "From: " + transition.from;
                output += ", Input Symbol: " + transition.state;
                output += ", To: " + transition.to + "\n";
            }

            // Write the output string to the console!
            Console.WriteLine(output);
        }
    }
}
