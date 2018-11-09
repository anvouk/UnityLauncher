# Unity Launcher (Unofficial)

A lightweight application to help manage multiple versions of Unity each with its own license file.

## Features
- Multiple Unity versions with multiple licenses support.
- Database saved in a JSON file.

## Setup
1. Add a new list item.
2. Insert the Unity.exe path.
3. Insert the .ulf Unity's license path to be used.
4. Save the changes.
5. Double click on the list item to launch Unity with the specified license file.

## How it works
The current Unity license in ProgramData (hidden folder usually at 'C:\ProgramData\Unity\') gets ***overwritten*** with a copy of the specified license right before starting Unity.
