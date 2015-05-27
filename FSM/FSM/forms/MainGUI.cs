using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace FSM
{
    public partial class MainGUI : Form
    {
        #region Tabs
        enum Tabs
        {
            create,
            edit,
            test,
            help,
            readme
        }
        enum TabNameState
        {
            base_names,
            file_names,
            altered_names
        }
        #endregion

        CommonAutomaton Common = new CommonAutomaton();
        CommonAutomaton.Automaton automaton = new CommonAutomaton.Automaton("");
        bool hasChanged = false;
        string currentAutomaton = "";

        /* Function: MainGUI
         * Initializes the GUI
         * Input: N/A
         * Output: N/A
         */
        #region MainGUI
        public MainGUI()
        {
            InitializeComponent();
        }
        #endregion

        /* Function: exit_btn_Click
         * Exits the program
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region exit_btn_Click
        private void exit_btn_Click(object sender, EventArgs e)
        {
            bool cont = false;
            if (hasChanged)
            {
                var result = MessageBox.Show("The current automaton hasn't been saved, are you sure you want to quit?", 
                    "Exit Program", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    cont = true;
            }
            else
                cont = true;

            if (cont)
                this.Close();
        }
        #endregion

        /* Function: about_btn_Click
         * Opens the about box
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region about_btn_Click
        private void about_btn_Click(object sender, EventArgs e)
        {
            forms.About aboutBox = new forms.About();
            aboutBox.ShowDialog();
        }
        #endregion

        /* Function: readme_btn_Click
         * Opens the readme tab
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region readme_btn_Click
        private void readme_btn_Click(object sender, EventArgs e)
        {
            // hide all tabs
            clearTabs();
            
            // Show Readme tab
            tab_ctl.TabPages.Add(readme_pge);
        }
        #endregion

        /* Function: help_btn_Click
         * Opens the help tab
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region help_btn_Click
        private void help_btn_Click(object sender, EventArgs e)
        {
            // hide all tabs
            clearTabs();

            // Show Help tab
            tab_ctl.TabPages.Add(help_pge);
        }
        #endregion

        /* Function: test_btn_Click
         * Opens the test tab for an automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region test_btn_Click
        private void test_btn_Click(object sender, EventArgs e)
        {
            // get the current "working" automaton (if there is one)
            if (tab_ctl.SelectedTab == create_pge)
            {
                setupAutomaton(Tabs.create);
            }
            else if (tab_ctl.SelectedTab == edit_pge)
            {
                setupAutomaton(Tabs.edit);
            }

            // get an automaton file to open
            if (automaton.initial == "" || tab_ctl.SelectedTab == test_pge)
            {
                OpenFileDialog loader = new OpenFileDialog();

                loader.InitialDirectory = Directory.GetCurrentDirectory();
                loader.Filter = "XML files (*.xml)|*.xml";
                loader.FilterIndex = 1;
                loader.RestoreDirectory = true;

                if (loader.ShowDialog() == DialogResult.OK)
                {
                    // Check if file is good...
                    CommonAutomaton.Automaton tempAutomaton = Common.loadAutomaton(loader.FileName);

                    if (testAutomaton(tempAutomaton) && tempAutomaton.initial != "")
                    {
                        // hide all tabs
                        clearTabs();

                        // Make sure the trees are cleared...
                        clearTrees(Tabs.edit);

                        // Show Edit tab
                        tab_ctl.TabPages.Add(test_pge);

                        // Load Automaton and show automaton
                        automaton = tempAutomaton;
                        setCurrentAutomatonName(loader.FileName);
                        showAutomaton(Tabs.test);
                    }
                    else if (tempAutomaton.initial != "")
                        MessageBox.Show("File does not have a valid automaton saved in it.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // otherwise open the current automaton
            else
            {
                if (testAutomaton(automaton))
                {
                    // hide all tabs
                    clearTabs();

                    // Make sure the trees are cleared...
                    clearTrees(Tabs.test);

                    // Show Test tab
                    tab_ctl.TabPages.Add(test_pge);

                    // Load Automaton and show automaton
                    showAutomaton(Tabs.test);
                }
            }
        }
        #endregion

        /* Function: edit_btn_Click
         * Opens the edit tab for an automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region edit_btn_Click
        private void edit_btn_Click(object sender, EventArgs e)
        {
            if (automaton.initial == "" || tab_ctl.SelectedTab == edit_pge)
            {
                OpenFileDialog loader = new OpenFileDialog();

                loader.InitialDirectory = Directory.GetCurrentDirectory();
                loader.Filter = "XML files (*.xml)|*.xml";
                loader.FilterIndex = 1;
                loader.RestoreDirectory = true;

                if (loader.ShowDialog() == DialogResult.OK)
                {
                    // Check if file is good...
                    CommonAutomaton.Automaton tempAutomaton = Common.loadAutomaton(loader.FileName);

                    if (testAutomaton(tempAutomaton) && tempAutomaton.initial != "")
                    {
                        // hide all tabs
                        clearTabs();

                        // Make sure the trees are cleared...
                        clearTrees(Tabs.edit);

                        // Show Edit tab
                        tab_ctl.TabPages.Add(edit_pge);

                        // Load Automaton and show automaton
                        automaton = tempAutomaton;
                        setCurrentAutomatonName(loader.FileName);
                        showAutomaton(Tabs.edit);
                    }
                    else if (tempAutomaton.initial != "")
                        MessageBox.Show("File does not have a valid automaton saved in it.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // hide all tabs
                clearTabs();

                // Make sure the trees are cleared...
                clearTrees(Tabs.edit);

                // Show Test tab
                tab_ctl.TabPages.Add(edit_pge);

                // Show automaton
                showAutomaton(Tabs.edit);
            }
        }
        #endregion

        /* Function: create_btn_Click
         * Create a new automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region create_btn_Click
        private void create_btn_Click(object sender, EventArgs e)
        {
            bool cont = false;
            if (hasChanged)
            {
                var result = MessageBox.Show("The current automaton hasn't been saved, are you sure you want to create a new one?",
                    "Create Automaton", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    cont = true;
            }
            else
                cont = true;

            if (cont)
            {
                hasChanged = false;

                // hide all tabs
                clearTabs();

                // Make sure the trees are cleared...
                clearTrees(0);

                // clear the last automaton
                automaton = new CommonAutomaton.Automaton("");

                setTabNames(TabNameState.base_names);

                // Show Create tab
                tab_ctl.TabPages.Add(create_pge);
            }
        }
        #endregion


        /* Function: altCreate_btn_Click
         * Create a new automaton via commands
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region altCreate_btn_Click
        private void altCreate_btn_Click(object sender, EventArgs e)
        {
            bool cont = false;
            if (hasChanged)
            {
                var result = MessageBox.Show("The current automaton hasn't been saved, are you sure you want to create a new one?",
                    "Create Automaton", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    cont = true;
            }
            else
                cont = true;

            if (cont)
            {
                hasChanged = false;

                // hide all tabs
                clearTabs();

                // Make sure the inputbox is cleared...
                altCreate_txt.Text = "";

                // clear the last automaton
                automaton = new CommonAutomaton.Automaton("");

                setTabNames(TabNameState.base_names);

                // Show Create tab
                tab_ctl.TabPages.Add(altCreate_pge);
            }
        }
        #endregion

        /* Function: saveC_btn_Click
         * Save the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region saveC_btn_Click
        private void saveC_btn_Click(object sender, EventArgs e)
        {
            ////// SETUP AUTOMATON //////
            setupAutomaton(Tabs.create);

            ////// TEST AND SAVE AUTOMATON //////
            testAndSaveAutomaton();
        }
        #endregion

        /* Function: saveE_btn_Click
         * Save the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region saveE_btn_Click
        private void saveE_btn_Click(object sender, EventArgs e)
        {
            ////// SETUP AUTOMATON //////
            setupAutomaton(Tabs.edit);

            ////// TEST AND SAVE AUTOMATON //////
            testAndSaveAutomaton();
        }
        #endregion

        /* Function: saveA_btn_Click
         * Save the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region saveA_btn_Click
        private void saveA_btn_Click(object sender, EventArgs e)
        {
            ////// SETUP AUTOMATON //////
            setupCommandAutomaton();

            ////// TEST AND SAVE AUTOMATON //////
            testAndSaveAutomaton();
        }
        #endregion

        /* Function: testC_btn_Click
         * Test the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region testC_btn_Click
        private void testC_btn_Click(object sender, EventArgs e)
        {
            setupAutomaton(Tabs.create);
            if (testAutomaton(automaton))
            {
                // hide all tabs
                clearTabs();

                // Make sure the trees are cleared...
                clearTrees(Tabs.test);

                // Show Test tab
                tab_ctl.TabPages.Add(test_pge);

                // show the automaton
                showAutomaton(Tabs.test);
            }
        }
        #endregion

        /* Function: testE_btn_Click
         * Test the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region testE_btn_Click
        private void testE_btn_Click(object sender, EventArgs e)
        {
            setupAutomaton(Tabs.edit);
            if (testAutomaton(automaton))
            {
                // hide all tabs
                clearTabs();

                // Make sure the trees are cleared...
                clearTrees(Tabs.test);

                // Show Test tab
                tab_ctl.TabPages.Add(test_pge);

                // Gets the current Automaton and show it
                showAutomaton(Tabs.test);
            }
        }
        #endregion


        /* Function: testA_btn_Click
         * Test the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region testA_btn_Click
        private void testA_btn_Click(object sender, EventArgs e)
        {
            setupCommandAutomaton();
            if (testAutomaton(automaton))
            {
                hasChanged = true;

                // hide all tabs
                clearTabs();

                // Make sure the trees are cleared...
                clearTrees(Tabs.test);

                // Show Test tab
                tab_ctl.TabPages.Add(test_pge);

                // show the automaton
                showAutomaton(Tabs.test);
            }
        }
        #endregion

        /* Function: editAutomaton_btn_Click
         * Edit the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region editAutomaton_btn_Click
        private void editAutomaton_btn_Click(object sender, EventArgs e)
        {
            // hide all tabs
            clearTabs();

            // Make sure the trees are cleared...
            clearTrees(Tabs.edit);

            // Show Test tab
            tab_ctl.TabPages.Add(edit_pge);

            // Load Automaton and show automaton
            showAutomaton(Tabs.edit);
        }
        #endregion

        /* Function: addStateC_btn_Click
         * Add a state to the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addStateC_btn_Click
        private void addStateC_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("State to add?", "Add State", "", -1, -1);
            if (input != "")
            {
                statesC_view.Nodes.Add("State~" + input, input);
                hasChanged = true;
            }
        }
        #endregion

        /* Function: addStateE_btn_Click
         * Add a state to the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addStateE_btn_Click
        private void addStateE_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("State to add?", "Add State", "", -1, -1);
            if (input != "")
            {
                statesE_view.Nodes.Add("State~" + input, input);
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
                hasChanged = true;
            }
        }
        #endregion

        /* Function: addInputC_btn_Click
         * Add an input symbol to the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addInputC_btn_Click
        private void addInputC_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Input Symbol to add?", "Add Input Symbol", "", -1, -1);
            if (input != "")
            {
                inputsC_view.Nodes.Add("Input~" + input, input);
                hasChanged = true;
            }
        }
        #endregion

        /* Function: addInputE_btn_Click
         * Add an input symbol to the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addInputE_btn_Click
        private void addInputE_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Input Symbol to add?", "Add Input Symbol", "", -1, -1);
            if (input != "")
            {
                inputsE_view.Nodes.Add("Input~" + input, input);
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
                hasChanged = true;
            }
        }
        #endregion

        /* Function: addTransitionC_btn_Click
         * Add a transition to the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addTransitionC_btn_Click
        private void addTransitionC_btn_Click(object sender, EventArgs e)
        {
            string from = Microsoft.VisualBasic.Interaction.InputBox("State transition comes from?", "Add Transition", "", -1, -1);
            if (from != "")
            {
                string to = Microsoft.VisualBasic.Interaction.InputBox("State transition goes to?", "Add Transition", "", -1, -1);
                if (to != "")
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox("Input symbol for the transition?", "Add Transition", "", -1, -1);
                    if (input != "")
                    {
                        TreeNode node = transitionsC_view.Nodes.Add("Transition~" + from + "~" + to + "~" + input, "Transition (" + from + "," + input + "," + to + ")");
                        node.Nodes.Add(from, "From: " + from);
                        node.Nodes.Add(to, "To: " + to);
                        node.Nodes.Add(input, "Input: " + input);
                        hasChanged = true;
                    }
                }
            }
        }
        #endregion

        /* Function: addTransitionE_btn_Click
         * Add a transition to the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addTransitionE_btn_Click
        private void addTransitionE_btn_Click(object sender, EventArgs e)
        {
            string from = Microsoft.VisualBasic.Interaction.InputBox("State transition comes from?", "Add Transition", "", -1, -1);
            if (from != "")
            {
                string to = Microsoft.VisualBasic.Interaction.InputBox("State transition goes to?", "Add Transition", "", -1, -1);
                if (to != "")
                {
                    string input = Microsoft.VisualBasic.Interaction.InputBox("Input symbol for the transition?", "Add Transition", "", -1, -1);
                    if (input != "")
                    {
                        TreeNode node = transitionsE_view.Nodes.Add("Transition~" + from + "~" + to + "~" + input, "Transition (" + from + "," + input + "," + to + ")");
                        node.Nodes.Add(from, "From:" + from);
                        node.Nodes.Add(to, "To:" + to);
                        node.Nodes.Add(input, "Input:" + input);
                        if (currentAutomaton != "")
                            setTabNames(TabNameState.altered_names);
                        hasChanged = true;
                    }
                }
            }
        }
        #endregion

        /* Function: setInitialC_btn_Click
         * Set the initial state for the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region setInitialC_btn_Click
        private void setInitialC_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Initial State for the Automaton?", "Set Initial State", "", -1, -1);
            if (input != "")
            {
                initialC_view.Text = input;
                hasChanged = true;
            }
        }
        #endregion

        /* Function: setInitialE_btn_Click
         * Set the initial state for the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region setInitialE_btn_Click
        private void setInitialE_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Initial State for the Automaton?", "Set Initial State", "", -1, -1);
            if (input != "")
            {
                initialE_view.Text = input;
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
                hasChanged = true;
            }
        }
        #endregion

        /* Function: addFinalC_btn_Click
         * Add a final state to the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addFinalC_btn_Click
        private void addFinalC_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Final State to add?", "Add Final State", "", -1, -1);
            if (input != "")
            {
                finalsC_view.Nodes.Add("Final~" + input, input);
                hasChanged = true;
            }
        }
        #endregion

        /* Function: addFinalE_btn_Click
         * Add a final state to the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region addFinalE_btn_Click
        private void addFinalE_btn_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Final State to add?", "Add Final State", "", -1, -1);
            if (input != "")
            {
                finalsE_view.Nodes.Add("Final~" + input, input);
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
                hasChanged = true;
            }
        }
        #endregion

        /* Function: removeStateC_btn_Click
         * Remove a state from the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region removeStateC_btn_Click
        private void removeStateC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = statesC_view.SelectedNode;
            try
            {
                statesC_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                hasChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: removeStateE_btn_Click
         * Remove a state from the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region removeStateE_btn_Click
        private void removeStateE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = statesE_view.SelectedNode;
            try
            {
                statesE_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                hasChanged = true;
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: removeFinalC_btn_Click
         * Remove a final state from the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region removeFinalC_btn_Click
        private void removeFinalC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = finalsC_view.SelectedNode;
            try
            {
                finalsC_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                hasChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no final state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: removeFinalE_btn_Click
         * Remove a final state from the automaton
         * Input: 
         * Output: 
         */
        #region removeFinalE_btn_Click
        private void removeFinalE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = finalsE_view.SelectedNode;
            try
            {
                finalsE_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
                hasChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no final state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: removeTransitionC_btn_Click
         * Remove a transition from the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region removeTransitionC_btn_Click
        private void removeTransitionC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = transitionsC_view.SelectedNode;
            try
            {
                transitionsC_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                hasChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no transition selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: removeTransitionE_btn_Click
         * Remove a transition from the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region removeTransitionE_btn_Click
        private void removeTransitionE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = transitionsE_view.SelectedNode;
            try
            {
                transitionsE_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
                hasChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no transition selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: removeInputC_btn_Click
         * Remove an input symbol from the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region removeInputC_btn_Click
        private void removeInputC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = inputsC_view.SelectedNode;
            try
            {
                inputsC_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                hasChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no input symbol selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: removeInputE_btn_Click
         * Remove an input symbol from the automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region removeInputE_btn_Click
        private void removeInputE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = inputsE_view.SelectedNode;
            try
            {
                inputsE_view.Nodes.Remove(node);
                statesC_view.SelectedNode = null;
                if (currentAutomaton != "")
                    setTabNames(TabNameState.altered_names);
                hasChanged = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("There is no input symbol selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /* Function: testString_btn_Click
         * Tests a string against the current automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region testString_btn_Click
        private void testString_btn_Click(object sender, EventArgs e)
        {
            string inputString = inputT_txt.Text;
            // should we ignore whitespace?
            if (clearWhitespaceT_ckbx.Checked)
                inputString = inputString.Replace(" ",String.Empty);

            CommonAutomaton.Checker check = Common.checkString(inputString, automaton);

            if (check.isGood)
                resultT_txt.Text = "String is valid for the Automaton!";
            else
                resultT_txt.Text = "String is not valid for the Automaton!";
            finalStateT_txt.Text = check.finalState;
        }
        #endregion

        /* Function: editStateC_btn_Click
         * edit a state
         * Input: N/A
         * Output: N/A
         */
        #region editStateC_btn_Click
        private void editStateC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = statesC_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string currentState = node.Text;
                string input = Microsoft.VisualBasic.Interaction.InputBox("New State?", "Edit State: " + currentState, "", -1, -1);
                if (input != "")
                {
                    int index = statesC_view.Nodes.IndexOf(node);
                    statesC_view.Nodes.Remove(node);
                    statesC_view.Nodes.Insert(index, "State~" + input, input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: editStateE_btn_Click
         * edit a state
         * Input: N/A
         * Output: N/A
         */
        #region editStateE_btn_Click
        private void editStateE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = statesE_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string currentState = node.Text;
                string input = Microsoft.VisualBasic.Interaction.InputBox("New State?", "Edit State: " + currentState, "", -1, -1);
                if (input != "")
                {
                    int index = statesE_view.Nodes.IndexOf(node);
                    statesE_view.Nodes.Remove(node);
                    statesE_view.Nodes.Insert(index, "State~" + input, input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: editFinalC_btn_Click
         * edit a final state
         * Input: N/A
         * Output: N/A
         */
        #region editFinalC_btn_Click
        private void editFinalC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = finalsC_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string currentState = node.Text;
                string input = Microsoft.VisualBasic.Interaction.InputBox("New Final State?", "Edit Final State: " + currentState, "", -1, -1);
                if (input != "")
                {
                    int index = finalsC_view.Nodes.IndexOf(node);
                    finalsC_view.Nodes.Remove(node);
                    finalsC_view.Nodes.Insert(index, "State~" + input, input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: editFinalE_btn_Click
         * edit a final state
         * Input: N/A
         * Output: N/A
         */
        #region editFinalE_btn_Click
        private void editFinalE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = finalsE_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no state selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string currentState = node.Text;
                string input = Microsoft.VisualBasic.Interaction.InputBox("New Final State?", "Edit Final State: " + currentState, "", -1, -1);
                if (input != "")
                {
                    int index = finalsE_view.Nodes.IndexOf(node);
                    finalsE_view.Nodes.Remove(node);
                    finalsE_view.Nodes.Insert(index, "State~" + input, input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: editTransitionC_btn_Click
         * edit a transition
         * Input: N/A
         * Output: N/A
         */
        #region editTransitionC_btn_Click
        private void editTransitionC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = transitionsC_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no transition selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string curFrom = node.Nodes[0].Text;
                string curTo = node.Nodes[1].Text;
                string curInput = node.Nodes[2].Text;
                string current = "From: " + curFrom + ", Input: " + curInput + ", To: " + curTo;
                bool shouldModify = false;

                string from = Microsoft.VisualBasic.Interaction.InputBox("State transition comes from?", "Edit Transition: " + current, curFrom, -1, -1);
                if (from != "")
                {
                    curFrom = from;
                    shouldModify = true;
                }
                string to = Microsoft.VisualBasic.Interaction.InputBox("State transition goes to?", "Edit Transition: " + current, curTo, -1, -1);
                if (to != "")
                {
                    curTo = to;
                    shouldModify = true;
                }
                string input = Microsoft.VisualBasic.Interaction.InputBox("Input symbol for the transition?", "Edit Transition" + current, curInput, -1, -1);
                if (input != "")
                {
                    curInput = input;
                    shouldModify = true;
                }
                if (shouldModify)
                {
                    int index = transitionsC_view.Nodes.IndexOf(node);
                    transitionsC_view.Nodes.Remove(node);
                    node = transitionsC_view.Nodes.Insert(index, "Transition~" + from + "~" + to + "~" + input, "Transition (" + from + "," + input + "," + to + ")");
                    node.Nodes.Add(curFrom, "From: " + from);
                    node.Nodes.Add(curTo, "To: " + to);
                    node.Nodes.Add(curInput, "Input: " + input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: editTransitionE_btn_Click
         * edit a transition
         * Input: N/A
         * Output: N/A
         */
        #region editTransitionE_btn_Click
        private void editTransitionE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = transitionsE_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no transition selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string curFrom = node.Nodes[0].Text;
                string curTo = node.Nodes[1].Text;
                string curInput = node.Nodes[2].Text;
                string current = "From: " + curFrom + ", Input: " + curInput + ", To: " + curTo;
                bool shouldModify = false;

                string from = Microsoft.VisualBasic.Interaction.InputBox("State transition comes from?", "Edit Transition: " + current, curFrom, -1, -1);
                if (from != "")
                {
                    curFrom = from;
                    shouldModify = true;
                }
                string to = Microsoft.VisualBasic.Interaction.InputBox("State transition goes to?", "Edit Transition: " + current, curTo, -1, -1);
                if (to != "")
                {
                    curTo = to;
                    shouldModify = true;
                }
                string input = Microsoft.VisualBasic.Interaction.InputBox("Input symbol for the transition?", "Edit Transition" + current, curInput, -1, -1);
                if (input != "")
                {
                    curInput = input;
                    shouldModify = true;
                }
                if (shouldModify)
                {
                    int index = transitionsE_view.Nodes.IndexOf(node);
                    transitionsE_view.Nodes.Remove(node);
                    node = transitionsE_view.Nodes.Insert(index, "Transition~" + from + "~" + to + "~" + input, "Transition (" + from + "," + input + "," + to + ")");
                    node.Nodes.Add(curFrom, "From: " + from);
                    node.Nodes.Add(curTo, "To: " + to);
                    node.Nodes.Add(curInput, "Input: " + input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: editInputC_btn_Click
         * edit an input
         * Input: N/A
         * Output: N/A
         */
        #region editInputC_btn_Click
        private void editInputC_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = inputsC_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no input symbol selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string currentState = node.Text;
                string input = Microsoft.VisualBasic.Interaction.InputBox("New Input Symbol?", "Edit Input Symbol: " + currentState, "", -1, -1);
                if (input != "")
                {
                    int index = inputsC_view.Nodes.IndexOf(node);
                    inputsC_view.Nodes.Remove(node);
                    inputsC_view.Nodes.Insert(index, "State~" + input, input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: editInputE_btn_Click
         * edit an input
         * Input: N/A
         * Output: N/A
         */
        #region editInputE_btn_Click
        private void editInputE_btn_Click(object sender, EventArgs e)
        {
            TreeNode node = inputsE_view.SelectedNode;
            if (node == null)
                MessageBox.Show("There is no input symbol selected.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string currentState = node.Text;
                string input = Microsoft.VisualBasic.Interaction.InputBox("New Input Symbol?", "Edit Input Symbol: " + currentState, "", -1, -1);
                if (input != "")
                {
                    int index = inputsE_view.Nodes.IndexOf(node);
                    inputsE_view.Nodes.Remove(node);
                    inputsE_view.Nodes.Insert(index, "State~" + input, input);
                    hasChanged = true;
                }
            }
        }
        #endregion

        /* Function: newAutomatonToolStripMenuItem_Click
         * Create a new automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region newAutomatonToolStripMenuItem_Click
        private void newAutomatonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            create_btn_Click(sender, e);
        }
        #endregion

        /* Function: toolStripMenuItem1_Click
         * Edit an automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region toolStripMenuItem1_Click
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            edit_btn_Click(sender, e);
        }
        #endregion

        /* Function: loadAutomatonToolStripMenuItem_Click
         * Test an automaton
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region loadAutomatonToolStripMenuItem_Click
        private void loadAutomatonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test_btn_Click(sender, e);
        }
        #endregion

        /* Function: exitToolStripMenuItem_Click
         * Exit the program
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region exitToolStripMenuItem_Click
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit_btn_Click(sender, e);
        }
        #endregion

        /* Function: helpToolStripMenuItem1_Click
         * Open the help tab
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region helpToolStripMenuItem1_Click
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            help_btn_Click(sender, e);
        }
        #endregion

        /* Function: readmeToolStripMenuItem_Click
         * Open the readme tab
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region readmeToolStripMenuItem_Click
        private void readmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            readme_btn_Click(sender, e);
        }
        #endregion

        /* Function: aboutToolStripMenuItem_Click
         * Open the about box
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region aboutToolStripMenuItem_Click
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about_btn_Click(sender, e);
        }
        #endregion

        /* Function: altCreate_menu_Click
         * Go to Alternate Creation tab
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region altCreate_menu_Click
        private void altCreate_menu_Click(object sender, EventArgs e)
        {
            altCreate_btn_Click(sender, e);
        }
        #endregion

        /* Function: MainGUI_Load
         * Setup all the initial stuff for the program and load anything we need to.
         * Input: sender object and event arguments
         * Output: N/A
         */
        #region MainGUI_Load
        private void MainGUI_Load(object sender, EventArgs e)
        {
            // load readme and help files
            StreamReader readmeFile = new StreamReader("../../settings/Readme.txt");
            StreamReader helpFile = new StreamReader("../../settings/help.txt");
            StreamReader instructionsCFile = new StreamReader("../../settings/instructionsC.txt");
            readme_txt.Text = readmeFile.ReadToEnd();
            help_txt.Text = helpFile.ReadToEnd();
            instructionsA_txt.Text = instructionsCFile.ReadToEnd();
            readmeFile.Close();
            helpFile.Close();
            instructionsCFile.Close();

            // hide all tabs
            clearTabs();
        }
        #endregion

        /* Function: clearTabs
         * Clear all current tabs from the tab controller
         * Input: N/A
         * Output: N/A
         */
        #region clearTabs
        private void clearTabs()
        {
            foreach (TabPage tab in tab_ctl.TabPages)
                tab_ctl.TabPages.Remove(tab);
        }
        #endregion

        /* Function: setCurrentAutomatonName
         * Sets the current automaton name
         * Input: the file name to get the automaton name from
         * Output: N/A
         */
        #region setCurrentAutomatonName
        private void setCurrentAutomatonName(string file)
        {
            string[] temp = file.Split('\\');

            currentAutomaton = temp[temp.Count() - 1];

            setTabNames(TabNameState.file_names);
        }
        #endregion

        /* Function: setTabNames
         * Set (or reset) the tab names
         * Input: (optional) whether to reset the tab names
         * Output: N/A
         */
        #region setTabNames
        private void setTabNames(TabNameState reset = TabNameState.base_names)
        {
            if (reset == TabNameState.base_names)
            {
                edit_pge.Text = "Edit Automaton";
                test_pge.Text = "Test Automaton";
                currentAutomaton = "";
            }
            else if (reset == TabNameState.altered_names)
            {
                edit_pge.Text = "Edit " + currentAutomaton + " (Changed)";
                test_pge.Text = "Test " + currentAutomaton + " (Changed)";
                currentAutomaton = "";
            }
            else
            {
                edit_pge.Text = "Edit " + currentAutomaton;
                test_pge.Text = "Test " + currentAutomaton;
            }
        }
        #endregion

        /* Function: clearTrees
         * Clear the treeviews of the passed tab page
         * Input: the tab to clear the trees of
         * Output: N/A
         */
        #region clearTrees
        private void clearTrees(Tabs tab)
        {
            // 0: create
            if (tab == Tabs.create)
            {
                statesC_view.Nodes.Clear();
                finalsC_view.Nodes.Clear();
                transitionsC_view.Nodes.Clear();
                inputsC_view.Nodes.Clear();
                initialC_view.Text = "";
            }
            // 1: edit
            else if (tab == Tabs.edit)
            {
                statesE_view.Nodes.Clear();
                finalsE_view.Nodes.Clear();
                transitionsE_view.Nodes.Clear();
                inputsE_view.Nodes.Clear();
                initialC_view.Text = "";
            }
            // 2: test
            else if (tab == Tabs.test)
            {
                statesT_view.Nodes.Clear();
                finalsT_view.Nodes.Clear();
                transitionsT_view.Nodes.Clear();
                inputsT_view.Nodes.Clear();
                initialC_view.Text = "";
            }
        }
        #endregion

        /* Function: showAutomaton
         * Displays the automaton for the passed tab
         * Input: the tab to clear the trees of
         * Output: N/A
         */
        #region showAutomaton
        private void showAutomaton(Tabs tab)
        {
            ////// EDIT AUTOMATON //////
            if (tab == Tabs.edit)
            {
                // Add all the states
                foreach (string state in automaton.states)
                    statesE_view.Nodes.Add("State~" + state, state);
                // Add all the final states
                foreach (string state in automaton.finalStates)
                    finalsE_view.Nodes.Add("Final~" + state, state);
                // Add all the transitions
                foreach (CommonAutomaton.Transition transition in automaton.transitions)
                {
                    TreeNode node = transitionsE_view.Nodes.Add("Transition~" + transition.from + "~" + transition.to + "~" + transition.input, "Transition (" + transition.from + "," + transition.input + "," + transition.to + ")");
                    node.Nodes.Add(transition.from, "From: " + transition.from);
                    node.Nodes.Add(transition.to, "To: " + transition.to);
                    node.Nodes.Add(transition.input, "Input: " + transition.input);
                }
                // Add all the input symbols
                foreach (string input in automaton.inputSymbols)
                    inputsE_view.Nodes.Add("Input~" + input, input);
                // Get the initial state
                initialE_view.Text = automaton.initial;
            }
            ////// TEST AUTOMATON //////
            else if (tab == Tabs.test)
            {
                // Add all the states
                foreach (string state in automaton.states)
                    statesT_view.Nodes.Add("State~" + state, state);
                // Add all the final states
                foreach (string state in automaton.finalStates)
                    finalsT_view.Nodes.Add("Final~" + state, state);
                // Add all the transitions
                foreach (CommonAutomaton.Transition transition in automaton.transitions)
                {
                    TreeNode node = transitionsT_view.Nodes.Add("Transition~" + transition.from + "~" + transition.to + "~" + transition.input, "Transition (" + transition.from + "," + transition.input + "," + transition.to + ")");
                    node.Nodes.Add(transition.from, "From: " + transition.from);
                    node.Nodes.Add(transition.to, "To: " + transition.to);
                    node.Nodes.Add(transition.input, "Input: " + transition.input);
                }
                // Add all the input symbols
                foreach (string input in automaton.inputSymbols)
                    inputsT_view.Nodes.Add("Input~" + input, input);
                // Get the initial state
                initialT_view.Text = automaton.initial;

                // Clear the other textboxes
                inputT_txt.Text = "";
                resultT_txt.Text = "";
                finalStateT_txt.Text = "";

                // set the checkbox to true
                clearWhitespaceT_ckbx.Checked = true;
            }
        }
        #endregion

        /* Function: setupAutomaton
         * "Creates" the current automaton from the treeviews of the passed tab
         * Input: the tab to clear the trees of
         * Output: N/A
         */
        #region setupAutomaton
        private void setupAutomaton(Tabs tab)
        {
            automaton = new CommonAutomaton.Automaton("");
            if (tab == Tabs.edit)
            {
                // get the initial state
                automaton.setInitial(initialE_view.Text);
                // get the states for the automaton
                foreach (TreeNode node in statesE_view.Nodes)
                    automaton.addState(node.Text);
                // get the inputs for the automaton
                foreach (TreeNode node in inputsE_view.Nodes)
                    automaton.addInput(node.Text);
                // get the finals for the automaton
                foreach (TreeNode node in finalsE_view.Nodes)
                    automaton.addFinal(node.Text);
                // get the transitions for the automaton
                foreach (TreeNode node in transitionsE_view.Nodes)
                    automaton.addTransition(new CommonAutomaton.Transition(node.Nodes[0].Name, node.Nodes[1].Name, node.Nodes[2].Name));
            }
            else if (tab == Tabs.create)
            {
                // get the initial state
                automaton.setInitial(initialC_view.Text);
                // get the states for the automaton
                foreach (TreeNode node in statesC_view.Nodes)
                    automaton.addState(node.Text);
                // get the inputs for the automaton
                foreach (TreeNode node in inputsC_view.Nodes)
                    automaton.addInput(node.Text);
                // get the finals for the automaton
                foreach (TreeNode node in finalsC_view.Nodes)
                    automaton.addFinal(node.Text);
                // get the transitions for the automaton
                foreach (TreeNode node in transitionsC_view.Nodes)
                    automaton.addTransition(new CommonAutomaton.Transition(node.Nodes[0].Name, node.Nodes[1].Name, node.Nodes[2].Name));
            }
        }
        #endregion

        /* Function: setupCommandAutomaton
         * "Creates" the current automaton from the commands in the command text box
         * Input: N/A
         * Output: Whether the creation was successful
         */
        #region setupCommandAutomaton
        public void setupCommandAutomaton()
        {
            automaton = new CommonAutomaton.Automaton("");

            string commandTextBase = Regex.Replace(altCreate_txt.Text, @"\r\n?|\n", "");
            commandTextBase.Trim();
            string[] commands = commandTextBase.Split(';'); // "build" menu items array

            int initialCount = 0;
            foreach (string command in commands)
            {
                if (command.Contains("<state>"))
                {
                    automaton.addState(Regex.Replace(command, "<state>", ""));
                }
                else if (command.Contains("<final>"))
                {
                    automaton.addFinal(Regex.Replace(command, "<final>", ""));
                }
                else if (command.Contains("<initial>"))
                {
                    initialCount++;
                    automaton.setInitial(Regex.Replace(command, "<initial>", ""));
                    if (initialCount > 1)
                    {
                        MessageBox.Show("There are two initial states when there can only be one.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (command.Contains("<t>"))
                {
                    // get from
                    string newcommand = Regex.Replace(command, "<t><from>", "");
                    int nextIndex = newcommand.IndexOf("<input>");
                    string from = newcommand.Substring(0, nextIndex);
                    // get input
                    string temp2 = from + "<input>";
                    newcommand = Regex.Replace(newcommand, temp2, "");
                    nextIndex = newcommand.IndexOf("<to>");
                    string input = newcommand.Substring(0, nextIndex);
                    // get to
                    temp2 = input + "<to>";
                    newcommand = Regex.Replace(newcommand, temp2, "");
                    string to = newcommand.Substring(0, newcommand.Length);
                    // add transition
                    automaton.addTransition(from, input, to);
                }
                else if (command.Contains("<input>"))
                {
                    automaton.addInput(Regex.Replace(command, "<input>", ""));
                }
                else
                {
                    MessageBox.Show("There is an invalid (or improperly formated) command in the input box.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }
        #endregion

        /* Function: testAutomaton
         * Tests the current automaton for validity
         * Input: N/A
         * Output: whether the automaton is valid or not
         */
        #region testAutomaton
        private bool testAutomaton(CommonAutomaton.Automaton temp)
        {
            if (!Common.checkStates(temp))
            {
                MessageBox.Show("There are no states for the automaton.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if (!Common.checkInitial(temp))
                {
                    MessageBox.Show("There is no initial state for the automaton.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (!Common.checkFinals(temp))
                    {
                        MessageBox.Show("There are no valid final state(s) for the automaton.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        if (!Common.checkInputs(temp))
                        {
                            MessageBox.Show("There is no input(s) for the automaton.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                        {
                            if (!Common.checkTransitions(temp))
                            {
                                MessageBox.Show("There are no transitions for the automaton.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        /* Function: testAndSaveAutomaton
         * Tests the automaton for validity and, if it is, saves it
         * Input: N/A
         * Output: N/A
         */
        #region testAndSaveAutomaton
        private void testAndSaveAutomaton()
        {
            ////// TEST AND SAVE AUTOMATON //////
            if (testAutomaton(automaton))
            {
                // ask what the person wants to name the file...
                SaveFileDialog saver = new SaveFileDialog();
                saver.InitialDirectory = Directory.GetCurrentDirectory();
                saver.Filter = "XML files (*.xml)|*.xml";
                saver.FilterIndex = 1;
                saver.RestoreDirectory = true;

                if (saver.ShowDialog() == DialogResult.OK)
                {
                    // save the file
                    Common.saveAutomaton(automaton, saver.FileName);

                    setCurrentAutomatonName(saver.FileName);

                    hasChanged = false;
                }
            }
        }
        #endregion
    }
}
