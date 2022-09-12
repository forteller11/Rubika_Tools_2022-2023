using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Charly.SheetsToMaze
{
    public class MazeFileParser
    {
        public const char ASSIGNMENT_OPERATOR = '=';
        public const string MAGIC_STRING = "--maze file--";
        public const string CHUNK_DIVIDER = "--";
        public string _newLine;
        
        private string[] _lines;

        public MazeFileParser(string text)
        {
            if (text.Contains("\n\r"))
            {
                _newLine = "\n\r";
            } 
            else if (text.Contains("\r\n"))
            {
                _newLine = "\r\n";
            }
            else if (text.Contains("\n"))
            {
                _newLine = "\n";
            }
            else if (text.Contains("\r"))
            {
                _newLine = "\r";
            }
            
            _lines = text.Split(_newLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public Exception Parse(out MazeFileDeserialized mazeResult)
        {
            mazeResult = null;
            int lineNumber = 0;
            mazeResult = new MazeFileDeserialized();

            #region header
            string magic = consumeLine().ToLower();
            if (magic != MAGIC_STRING)
                return new Exception($"Magic string \"{MAGIC_STRING}\" not found at the start of this file, not recognized as a maze type.");

            string versionLine = consumeLine();
            if (!int.TryParse(versionLine, out int version))
                return new Exception($"Could not find Version Number at line {lineNumber}");
            mazeResult.Version = version;

            string col1Line = consumeLine();
            if (!ColorUtility.TryParseHtmlString(col1Line, out var col1))
                return new Exception($"Could not parse Primary Colour at line {lineNumber}");
            mazeResult.Primary = col1;
            
            string col2Line = consumeLine();
            if (!ColorUtility.TryParseHtmlString(col2Line, out var col2))
                return new Exception($"Could not parse Primary Colour at line {lineNumber}");
            mazeResult.Secondary = col2;
            #endregion
            
            #region glossary
            string glossaryLine = goNextThenConsumeUntilPastDivider();
            while (glossaryLine != null && glossaryLine != CHUNK_DIVIDER)
            {
                var glossaryParts = glossaryLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (glossaryParts.Length != 2)
                    return new Exception($"Glossary at line {lineNumber} must not contain more than one \"{ASSIGNMENT_OPERATOR}\"");

                string potentialCode = glossaryParts[0];
                if (potentialCode.Length != 1)
                    return new Exception($"Glossary Code at line {lineNumber} is \"{potentialCode}\" with {potentialCode.Length} characters but must only contain 1 character");

                char code = glossaryParts[0][0];
                string name = glossaryParts[1];
                var nameToSymbol = new NameToSymbol(code, name);
                mazeResult.Glossary.Add(nameToSymbol);
                
                glossaryLine = consumeAndReadNext();
            }
            #endregion

            #region maze chunks
            string currentLine = goNextThenConsumeUntilPastDivider();
            while (currentLine != null)
            {
                var currentMaze = new MazeDeserialized();
                mazeResult.Mazes.Add(currentMaze);

                currentMaze.Name = currentLine;

                #region maze grid
                string mazeRow = consumeAndReadNext();
                var currentGrid = currentMaze.Grid;
                currentGrid.Dimensions = new int2(mazeRow.Length, -1);

                int mazeHeight = 0;
                while (mazeRow != CHUNK_DIVIDER && mazeRow != null)
                {
                    if (mazeRow.Length != currentMaze.Grid.Dimensions.x)
                    {
                        return new Exception($"Maze {currentMaze.Name} does not have a heterogeneous length. Expected width is {currentGrid.Dimensions.x} but at line {lineNumber} it's {mazeRow.Length}. Beware of spaces.");
                    }

                    foreach (var mazeCell in mazeRow)
                    {
                        // ReSharper disable once ReplaceWithSingleAssignment.False
                        bool isDefinedInGlossary = false;

                        if (mazeCell == MazeFileDeserialized.EmptyAssci)
                            isDefinedInGlossary = true;
                        
                        foreach (var glossary in mazeResult.Glossary)
                        {
                            if (mazeCell == glossary.Symbol)
                                isDefinedInGlossary = true;
                        }

                        if (!isDefinedInGlossary)
                            return new Exception($"{mazeCell} at line {lineNumber} cannot be found in the glossary of maze \"{currentMaze.Name}\"");

                        currentMaze.Grid.Symbols.Add(mazeCell);
                    }
                    mazeHeight++;
                    mazeRow = consumeAndReadNext();
                }
                currentMaze.Grid.Dimensions = new int2(currentMaze.Grid.Dimensions.x, mazeHeight);
                #endregion

                currentLine = goNextThenConsumeUntilPastDivider();
            }
            #endregion
           
            string current()
            {
                return _lines[lineNumber];
            }

            string consumeLine()
            {
                string result = null;
                if (lineNumber < _lines.Length)
                {
                    result = _lines[lineNumber];
                    lineNumber++;
                }
                return result;
            }
            string consumeAndReadNext()
            {
                string result = null;
                if (lineNumber < _lines.Length)
                {
                    lineNumber++;
                    result = _lines[lineNumber];
                }
                return result;
            }
            
            string goNextThenConsumeUntilPastDivider()
            {
                if (lineNumber >= _lines.Length)
                {
                    return null;
                }
                
                string result = _lines[lineNumber];
                while (result == CHUNK_DIVIDER)
                {
                    lineNumber++;
                    if (lineNumber < _lines.Length)
                    {
                        result = _lines[lineNumber];
                    }
                    else
                    {
                        return null;
                    }
                }
                
                return result;
            }

            return null;
        }
    }
    
    [Serializable]
    public class MazeFileDeserialized
    {
        public const string EmptyGlossary = "Empty";
        public const char EmptyAssci = ' ';

        public int Version;
        public Color Primary;
        public Color Secondary;
        [SerializeField] public List<NameToSymbol> Glossary = new ();
        [SerializeField] public List<MazeDeserialized> Mazes = new();
        
        public string GetNameFromSymbol(char c)
        {
            if (c == EmptyAssci)
                return EmptyGlossary;

            foreach (var nameToAssci in Glossary)
            {
                if (c == nameToAssci.Symbol)
                {
                    return nameToAssci.Name;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class MazeDeserialized
    {
        [SerializeField] public string Name;
        [SerializeField] public SerializableGrid Grid = new ();
    }

    [Serializable]
    public class NameToSymbol
    {
        public char Symbol;
        public string Name;

        public NameToSymbol(char symbol, string name)
        {
            Symbol = symbol;
            Name = name;
        }
    }

    //isn't sparse
    public class SerializableGrid
    {
        public int2 Dimensions;
        public List<char> Symbols = new ();

        public char GetSymbol(int x, int y)
        {
            if (x > Dimensions.x || y > Dimensions.y)
            {
                throw new ArgumentException();
            }
            int flatIndex = (y * Dimensions.x) + x;
            return Symbols[flatIndex];
        }
    }
}