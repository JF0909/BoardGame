# C# Two-Player BoardGame

## Purpose
This project is designed to provide a reusable framework for different two-player board gamesn with design diagrams and applying software design patterns and OOP principles. 

## Description
To demostrate that, the framework can be easily adapted to different games. The design must accommodate all the following games in the same software:
- `Numerical Tic-Tac-Toe`
- `Notakto` - Two players take turns placing the same piece (e.g. an X ) on a finite number of 3 × 3 board (in this project, we only
use three boards). The game ends when all three boards contain a three-in-a-row of X s, at which point the player to have made the last move loses the game.
- `Gomoku` - Also known as Five in a Row (Wikipedia). Two players take turns placing two typesof pieces (e.g. an X and an O ) on a 15 × 15 board (you can use a smaller board if that helps the UI). The winner is the first player to form an unbroken line of five pieces of their colour horizontally, vertically, or diagonally.

## Features
1. Cater for different modes of play, including: 
- Human vs Human
- Computer vs Human
2. Moves Validation Check
3. Game can be able to be saved and restored from any state of play
4. Track History of games
5. Moves can be undoable and redoable
6. Provide Help Menu for better user experience

## Design patterns 
- Template Method Pattern
- Factory Pattern
- Commond Pattern

## OOP principle
- Class and Objects
- Inheritance
- Encapsulation
- Polymorphism 