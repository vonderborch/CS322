

Finite State Machine v. 1.0.0
	David Fletcher, Jacob St.Hilaire, Christian Webber

Currently the Finite State Machine will be compiled and run through Visual Studio 2012. It will
be run in a command line interface, using simple keyboard inputs. Navigation will be done
with the up and down facing arrow keys and selection will be made with the enter key.

Upon running you will come to the main menu and thus the program is layed out in such a
manner. For easy navigation you are welcome to refer to this guide labeled below.

Main Menu
	Build Automaton - Allows user to build their own automaton
		- Return to Main Menu - Returns the user to the Main Menu
		- Display Current Automaton - Display the current automaton that is being modified
		- Finish Building Automaton - Opens a new submenu
			- Return to Main Menu - Returns you to programs main menu state
			- Input a String - Input a string to test against the automaton
			- Display Current Auttomaton - Display the automaton and its characteristics
			- Edit Automaton - Allow user to make adjustments and changes to the Automaton
			- Save Automaton - Allow user to name and save the created automaton
		- Set Initial State - Turn a state into the initial starting state
		- Add Final State - Add a state to be a final state to determine completeness
		- Add State - Add a state into the automaton
		- Add Input Symbol - Adds an input symbol to automaton
		- Add Transition - Adds transition determiniates to automaton
		- Remove Final State - Removes Final State from automaton
		- Remove State - Displays menu of states for removal
		- Remove Input Symbol - Displays all input symbols for deletion
		- Remove Transition - Displays all transitions for deletion
	Load Automaton - Loads a prebuilt automaton
		- Displays menu of all current built automaton file names for selection
			- Return to Main Menu - Allows the user to return to the main menu
			- Input String - Allows user to input a string to check against the chosen automaton
			- Display Current Automaton - Displays the automaton that was selected and its characteristics
			- Edit Current Automaton - Allows user to create temporary changes to the loaded automaton
			- Save Current Automaton - Allows user to save the current automaton, either overwriting the previously saved
							version or saving it as a new file.
	Exit - Used to Exit the program