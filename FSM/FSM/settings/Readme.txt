Finite State Machine v. 2.1.0
	Team David: David Fletcher, Jacob St.Hilaire, Christian Webber
	CptS 322

Requirements:
	Microsoft Windows (tested with Windows 8 and Windows 8.1)

Currently the Finite State Machine (which is in the "FSM" folder) can be run by opening the project file with Visual Studio (tested with Visual Studio 2012) and be debugged, or alternitively by running the .exe located in the "Published" folder.

Upon being run, the program will open as a Windows Form gui. Primary navigation can be done using either the drop-down menus or the buttons on the left-hand side of the window.

Explanations of what makes up the program, how to use it, and what a Finite State Machine (Automaton) is can be found on the help screen, accessed by pressing the help button on the left side or on the drop down menu (under help").

Automatons are currently displayed by the program by their "tables," rather than graphically, so while testing an Automaton it might make it easier if one draws the Automaton on a peice of paper.

Changelog:
 - 2.1.1:
    + Fixed bug with command-made automatons preventing some transitions from being properly read.
	+ Fixed bug with handling input strings causing problems on mis-matched string lengths.
 - 2.1.0:
    + Changed "state" child node of a "transition" node in xml file to be called an "input", to better reflect what it actually is.
	+ Removed some unncessary automata saves and modified the rest to refer to the new XML file specification.
	+ Added "edit ..." buttons when creating or editing an automata via the graphical interface, to allow for easier time modifying parts of the automaton.
	+ Added check to make sure that the file being read contains a valid automaton or not (rather than assuming that it is).
	+ Added "Create Automaton via Commands" section, allowing for the creation of automaton via specific commands.
	   + Contains a helper file on the same tab page.
	   + Testing (or going back to edit the automaton from the test tab) is done using the standard graphical interface.
	+ Fixed small bug when checking an automaton's paths that made it occassionally not detect a valid path when there was one.
	+ Added text hinting for user to expand a transition to view all of it when creating, editing, or testing an automaton using the normal graphical interface.
	+ Fixed problem making testing a file always not pop up with the open file dialog.
 - 2.0.2:
    + Setup directories to default to in the code for automata saves if the program is being run in debug mode through Visual Studio.
	   + files are technically installed and exist for a client if the program is being run through an installed .exe, however the defaul directory won't point to them.
	+ Rearranged saved automata directories.
 - 2.0.1:
    + Updated the readme.
	+ Updated/Created the help file.
	+ Published a build of the program for client installation.
 - 2.0.0:
    + Redesigned program around a graphical interface.
	+ Added checks for making sure there is at least one valid path in the transitions to get to a final state.
	+ Reorganized code to better reflect what stuff is related.
