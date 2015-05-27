# CS322
Project for CS322 (Software Engineering Principles I) at WSU taught by Bolong Zeng during the Fall 2013 semester, a Finite State Automaton simulator.

This was a team project (my team was David Fletcher, Jacob St.Hilaire, and myself). In our team, each of us implemented our own versions of a Finite State Machine, and I've uploaded here my versions which are the versions that ended up being selected by my team for being turned in.

What is a Finite State Machine/Automaton: "A finite-state machine (FSM) or finite-state automaton (plural: automata), or simply a state machine, is a mathematical model of computation used to design both computer programs and sequential logic circuits. It is conceived as an abstract machine that can be in one of a finite number of states. The machine is in only one state at a time; the state it is in at any given time is called the current state. It can change from one state to another when initiated by a triggering event or condition; this is called a transition. A particular FSM is defined by a list of its states, and the triggering condition for each transition." (Wikipedia)

FSM Links:
- http://en.wikipedia.org/wiki/Finite-state_machine
- http://en.wikipedia.org/wiki/Nondeterministic_finite_automaton



These programs were created using C# and .NET and tested on a Windows 8.1 computer, although they might compile/run with the Mono framework and work under OSX and Linux.

Programs:
- FiniteStateMachineCommandLine: A command line version of the program. This was the initial version of the program that I created and it runs purely through a command-line interface.
- FSM: A Windows-forms based version of the program with more roboust support for finite state automatas. This program has a framework for handling nondeterministic finite automaton (NFA), but actual implementation for NFA's never occured.
