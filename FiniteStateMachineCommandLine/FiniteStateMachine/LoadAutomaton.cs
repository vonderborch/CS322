using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading.Tasks;

using FiniteStateMachine;

namespace FiniteStateMachine
{
    class LoadAutomaton
    {
        /* Function: loadMenu
         * shows all the files that are in the xml file directory
         * Input: the files in the directory
         * Output: the file to load
         */
        public string loadMenu(string[] menuItemsRaw)
        {
            menu MenuBuilder = new menu();
            string output = "";
            bool inMenu = true;
            int option = 0;
            string menuItems = "(Back to Main Menu)";

            // Build the menu items string
            foreach (string item in menuItemsRaw)
            {
                menuItems += ";" + item;
            }

            // show the menu...
            while (inMenu == true)
            {
                option = MenuBuilder.display_menu(';', "Load Automaton (use arrow keys to navigate, press enter to select)", "Finite State Machine", menuItems, option);

                // it the user wants to return to the main menu, return to it
                if (option == 0)
                {
                    inMenu = false;
                }
                // otherwise, set the output to the selected file.
                else
                {
                    output = menuItemsRaw[option - 1];
                    inMenu = false;
                }
            }

            return output;
        }

        /* Function: loadAutomaton
         * loads an automaton from the passed file location
         * Input: the file location
         * Output: the automaton in the file
         */
        public main.Automaton loadAutomaton(string file)
        {
            // define a blank new automaton
            main.Automaton automaton = new main.Automaton(null);
            Console.WriteLine("Checking if file exists...");

            // if the file exists, begin loading it...
            if (File.Exists(file) == true)
            {
                Console.WriteLine("File exists, Loading file...");
                parser Parser = new parser();
                XmlNodeList[] automatonXML = new XmlNodeList[5];

                automatonXML = Parser.parseXMLFile(file);

                //// Cycle through NodeLists filling out automaton...
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
                    main.Transition transition = new main.Transition();

                    // cycle through each node in the current transition node...
                    foreach (XmlNode node in automatonXML[4].Item(i))
                    {
                        // if the node is a "from" node...
                        if (node.Name == "from")
                        {
                            transition.from = node.InnerText;
                        }
                        // if the node is a "state" node...
                        else if (node.Name == "state")
                        {
                            transition.state = node.InnerText;
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
                Console.WriteLine("Finished Loading File!");
            }
            // if the file doesn't exist, say it doesn't exist and end the loading
            else
            {
                Console.WriteLine("File does not exist!");
            }

            return automaton;
        }

        /* Function: getNodeList
         * Gets a node list from the document
         * Input: the xml document and the tag to get the nodelist from
         * Output: the nodelist wanted
         */
        private XmlNodeList getNodeList(XmlDocument doc, string tag)
        {
            XmlNodeList temp = doc.GetElementsByTagName(tag);
            return temp;
        }
    }
}
