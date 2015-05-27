using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.IO;

namespace FSM
{
    class CommonAutomaton
    {
        // String Checker Variable
        #region Checker
        public struct Checker
        {
            public bool isGood;
            public string finalState;

            // Checker definition
            public Checker(bool g, string f)
            {
                isGood = g;
                finalState = f;
            }
        }
        #endregion

        // Transition variable
        #region Transition
        public struct Transition
        {
            public string from;  // The state that the transition starts from
            public string to;    // The state that the transition ends up in
            public string input; // The input symbol for the transition

            // transition definition
            public Transition(string f, string t, string inp)
            {
                from = f;
                to = t;
                input = inp;
            }
        }
        #endregion

        // TransitionPath variable (used for finding a valid path)
        #region TransitionPath
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
        #endregion

        // Automaton variable
        #region Automaton
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
            // add a transition to the list of transitions
            public void addTransition(string fr, string inp, string to)
            {
                transitions.Add(new Transition(fr, to, inp));
            }
        }
        #endregion

        /* Function: loadAutomaton
         * loads an automaton from the passed file location
         * Input: the file location
         * Output: the automaton in the file
         */
        #region LoadAutomaton
        public Automaton loadAutomaton(string file)
        {
            // define a blank new automaton
            Automaton automaton = new Automaton(null);

            // if the file exists, begin loading it...
            if (File.Exists(file) == true)
            {
                XmlNodeList[] automatonXML = new XmlNodeList[5];

                automatonXML = parseXMLFile(file);

                //// Cycle through NodeLists filling out automaton...
                try
                {
                    // States
                    foreach (XmlNode node in automatonXML[0].Item(0))
                    {
                        automaton.addState(node.InnerText);
                    }
                    // Input Symbols
                    foreach (XmlNode node in automatonXML[1].Item(0))
                    {
                        automaton.addInput(node.InnerText);
                    }
                    // Initial State
                    automaton.setInitial(automatonXML[2].Item(0).InnerText);
                    // Final States
                    foreach (XmlNode node in automatonXML[3].Item(0))
                    {
                        automaton.addFinal(node.InnerText);
                    }
                    // Transitions
                    for (int i = 0; i < automatonXML[4].Count; i++)
                    {
                        // Create a temporary transition variable
                        Transition transition = new Transition();

                        // cycle through each node in the current transition node...
                        foreach (XmlNode node in automatonXML[4].Item(i))
                        {
                            // if the node is a "from" node...
                            if (node.Name == "from")
                            {
                                transition.from = node.InnerText;
                            }
                            // if the node is a "input" node...
                            else if (node.Name == "input")
                            {
                                transition.input = node.InnerText;
                            }
                            // if the node is a "to" node...
                            else if (node.Name == "to")
                            {
                                transition.to = node.InnerText;
                            }
                        }

                        // add the transition to the automaton
                        automaton.addTransition(transition);
                    }
                }
                catch (Exception ex)
                {
                    automaton.initial = "";
                    MessageBox.Show("There was a problem loading the XML file, most likely it was improperly formatted.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // if the file doesn't exist, say it doesn't exist and end the loading
            else
            {
                MessageBox.Show("There was a problem loading the XML file, it could not be found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return automaton;
        }
        #endregion
        
        /* Function: parseXMLFile
         * Parses an XML file as a list of XmlNodeLists
         * Input: a location of an xml file
         * Output: a list of XmlNodeLists
         */
        #region ParseXMLFile
        public XmlNodeList[] parseXMLFile(string file)
        {
            XmlNodeList[] output = new XmlNodeList[5];

            // create and load an XML document variable
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            // Get NodeLists for each component of the XML File
            output[0] = getNodeList(doc, "States");
            output[1] = getNodeList(doc, "InputSymbols");
            output[2] = getNodeList(doc, "Initial");
            output[3] = getNodeList(doc, "FinalStates");
            output[4] = getNodeList(doc, "transition");

            return output;
        }
        #endregion

        /* Function: saveAutomaton
         * Saves an automaton to a file
         * Input: an automaton
         * Output: a boolean over whether the save was valid (should always be valid, so always is true)
         */
        #region SaveAutomaton
        public bool saveAutomaton(Automaton automaton, string file)
        {
            // create a new blank xml document the load the parsed automaton string to it
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(parseAutomatonToString(automaton));

            doc.PreserveWhitespace = false; // automatically setup the whitespace for the document
            doc.Save(file); // save the file

            return true;
        }
        #endregion

        /* Function: parseAutomatonToString
         * Parses a passed an automaton as a string to use for saving it
         * Input: the automaton to parse
         * Output: the string of the parsed automaton
         */
        #region ParseAutomatonToString
        public string parseAutomatonToString(Automaton automaton)
        {
            string output = "<automaton>"; // setup the string

            // Setup the states
            output += "<States>";
            foreach (string state in automaton.states)
            {
                output += "<state>" + state + "</state>"; // add a state
            }
            output += "</States>";

            // Setup the input symbols
            output += "<InputSymbols>";
            foreach (string symbol in automaton.inputSymbols)
            {
                output += "<symbol>" + symbol + "</symbol>"; // add a symbol
            }
            output += "</InputSymbols>";

            // setup the transitions
            output += "<Transitions>";
            foreach (Transition transition in automaton.transitions)
            {
                // add all the peices of a transition
                output += "<transition>";
                output += "<from>" + transition.from + "</from>";
                output += "<input>" + transition.input + "</input>";
                output += "<to>" + transition.to + "</to>";
                output += "</transition>";
            }
            output += "</Transitions>";

            // setup the initial state
            output += "<Initial>" + automaton.initial + "</Initial>";

            // setup the final states
            output += "<FinalStates>";
            foreach (string state in automaton.finalStates)
            {
                output += "<state>" + state + "</state>"; // add a final state
            }
            output += "</FinalStates>";

            return output += "</automaton>"; // close the automaton string and return
        }
        #endregion

        /* Function: getNodeList
         * Gets a node list from the document
         * Input: the xml document and the tag to get the nodelist from
         * Output: the nodelist wanted
         */
        #region GetNodeList
        public XmlNodeList getNodeList(XmlDocument doc, string tag)
        {
            XmlNodeList temp = doc.GetElementsByTagName(tag);
            return temp;
        }
        #endregion

        /* Function: checkInputs
         * Checks to make sure that there are input symbols
         * Input: the automaton to check
         * Output: whether there are inputs
         */
        #region CheckInputs
        public bool checkInputs(Automaton automaton)
        {
            // Check if there are input symbols
            if (automaton.inputSymbols.Count == 0)
                return false;
            // Check if the input symbols contain any information
            foreach (string input in automaton.inputSymbols)
                if (input == "")
                    return false;
            return true;
        }
        #endregion

        /* Function: checkStates
         * Checks to make sure that there are states
         * Input: the automaton to check
         * Output: whether there are states
         */
        #region CheckStates
        public bool checkStates(Automaton automaton)
        {
            // Check if there are states
            if (automaton.states.Count == 0)
                return false;
            // Check if the states contain any information
            foreach (string state in automaton.states)
                if (state == "")
                    return false;
            return true;
        }
        #endregion

        /* Function: checkInitial
         * Checks to make sure that the initial state is set
         * Input: the automaton to check
         * Output: whether there is an initial state
         */
        #region CheckInitial
        public bool checkInitial(Automaton automaton)
        {
            // Check if there is an initial state and its in the states list
            if (automaton.initial != "")
            {
                if (automaton.states.Contains(automaton.initial) == false)
                    return false;
            }
            else return false;
            return true;
        }
        #endregion

        /* Function: checkFinals
         * Checks to make sure that there are final states
         * Input: the automaton to check
         * Output: whether there are (valid) final states
         */
        #region CheckFinals
        public bool checkFinals(Automaton automaton)
        {
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
            return true;
        }
        #endregion

        /* Function: checkTransitions
         * Checks to make sure that there are transitions and at least one valid transition path
         * Input: the automaton to check
         * Output: whether there are transitions and at least one good transition path
         */
        #region CheckTransitions
        public bool checkTransitions(Automaton temp)
        {
            // Check that the transitions are all valid (valid from, input symbol, and to)
            if (temp.transitions.Count > 0)
            {
                // check each transition...
                foreach (Transition transition in temp.transitions)
                {
                    // is its from contained in the states list?
                    if (temp.states.Contains(transition.from) == false)
                        return false;

                    // is its to contained in the states list?
                    if (temp.states.Contains(transition.to) == false)
                        return false;
                    // is its input symbol contained in the input symbols list?
                    if (temp.inputSymbols.Contains(transition.input) == false)
                        return false;
                }
            }
            else return false;

            // Check that there is at least one valid transition path
            // create list of all transitions and fill it...
            TransitionPath[] transitions = new TransitionPath[temp.transitions.Count];
            for (int i = 0; i < transitions.Count(); i++)
            {
                transitions[i].transition = temp.transitions[i];
                transitions[i].hasDoneTransition = false;
            }
            // create list of initial transitions and fill it...
            List<Transition> initialTransitions = new List<Transition>();
            foreach (Transition transition in temp.transitions)
            {
                if (transition.from == temp.initial)
                    initialTransitions.Add(transition);
            }
            // now we finally can check for a good path...
            bool foundGood = false;
            foreach (Transition transition in initialTransitions)
            {
                if (checkForValidPath(transitions, temp.finalStates, transition))
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
        #endregion

        /* Function: checkForValidPath
         * Checks to see whether there is a valid path to a final state
         * Input: the (enhanced) transitions list, the acceptable final states, the current transition being performed
         * Output: whether the transition is valid
         */
        #region CheckForValidPath
        private bool checkForValidPath(TransitionPath[] transitions, List<string> finalStates, Transition currentTransition)
        {
            bool isGood = false;
            if (finalStates.Contains(currentTransition.to))
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
                    foreach (TransitionPath transPath in transitions)
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
        #endregion

        /* Function: checkString
         * checks to see whether the input string is valid for the automaton
         * Input: the input string and the automaton
         * Output: whether the string is good
         */
        #region checkString
        public Checker checkString(string input, Automaton automaton)
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
                    try
                    {
                        string temp = input.Substring(0, symbolLength); // get the value of the current next input symbol
                        // if the symbols match, we've found the current input symbol we're working with!
                        if (symbol == temp)
                        {
                            currentInput = symbol;
                            break;
                        }
                    }
                    catch { }
                }
                // if no input symbol has been found, the string is bad
                if (currentInput == "")
                {
                    MessageBox.Show("There was a symbol that was invalid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                // remove the current input symbol from the input string...
                input = input.Remove(0, currentInput.Length);

                //// TRANSITION ////
                // loop through transitions till one is found that matches the current state and input value (if none found, we're done looking, the string failed)
                foundTransition = false;
                foreach (Transition transition in automaton.transitions)
                {
                    // does the current state match the state we're looking at?
                    if (transition.from == currentState)
                    {
                        // does the current input match the input we're looking at?
                        if (transition.input == currentInput)
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

            return new Checker(good, currentState);
        }
        #endregion
    }
}
