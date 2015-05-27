using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FiniteStateMachine
{
    class parser
    {
        /* Function: parseXMLFile
         * Parses an XML file as a list of XmlNodeLists
         * Input: a location of an xml file
         * Output: a list of XmlNodeLists
         */
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

        /* Function: saveAutomaton
         * Saves an automaton to a file
         * Input: an automaton
         * Output: a boolean over whether the save was valid (should always be valid, so always is true)
         */
        public bool saveAutomaton(main.Automaton automaton)
        {
            // find out what the user wants to call the automaton and setup its filename
            Console.WriteLine("Please enter what you want to call the automaton and press enter...");
            string file = "../../files/" + Console.ReadLine() + ".xml";

            // create a new blank xml document the load the parsed automaton string to it
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(parseAutomatonToString(automaton));

            doc.PreserveWhitespace = false; // automatically setup the whitespace for the document
            doc.Save(file); // save the file

            return true;
        }

        /* Function: parseAutomatonToString
         * Parses a passed an automaton as a string to use for saving it
         * Input: the automaton to parse
         * Output: the string of the parsed automaton
         */
        public string parseAutomatonToString(main.Automaton automaton)
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
            foreach (main.Transition transition in automaton.transitions)
            {
                // add all the peices of a transition
                output += "<transition>";
                output += "<from>" + transition.from + "</from>";
                output += "<state>" + transition.state + "</state>";
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

        /* Function: getNodeList
         * Gets a node list from the document
         * Input: the xml document and the tag to get the nodelist from
         * Output: the nodelist wanted
         */
        public XmlNodeList getNodeList(XmlDocument doc, string tag)
        {
            XmlNodeList temp = doc.GetElementsByTagName(tag);
            return temp;
        }
    }
}
