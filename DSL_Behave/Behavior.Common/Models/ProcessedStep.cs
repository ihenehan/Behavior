using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Behavior.Common.Models
{
    public class ProcessedStep
    {
        private string processedCommand;
        private string rawCommand;
        private List<object> parameters;
        private char quote = '"';
        private char openBracket = '<';
        private char closeBracket = '>';
        private string argBuffer = "";
        private bool foundOpenQuote = false;
        private bool foundCloseQuote = false;
        private bool foundOpenBracket = false;
        private bool foundCloseBracket = false;

        public string RawCommand
        {
            get { return rawCommand; }
            set { rawCommand = value; }
        }

        public string ProcessedCommand
        {
            get { return processedCommand; }
            set { processedCommand = value; }
        }

        public List<object> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public ProcessedStep(string rawCommand)
        {
            RawCommand = rawCommand;

            Process();
        }

        public void Process()
        {
            Parameters = new List<object>();

            foreach (char c in RawCommand)
                ProcessChar(c);

            ProcessedCommand = RawCommand;

            InsertParameterTokens("[arg]");

            RemoveParameterQuotes();
        }

        public void ProcessChar(char c)
        {
            if (!foundOpenQuote && !foundOpenBracket)
                NotOpenQuoteNotOpenBracket(c);

            else if (foundOpenQuote && !foundCloseQuote)
                IsOpenQuoteNotCloseQuote(c);

            else if (foundOpenQuote && foundCloseQuote)
                IsOpenQuoteIsClosedQuote(c);

            else if (foundOpenBracket && !foundCloseBracket)
                IsOpenBracketNotCloseBracket(c);

            else if (foundOpenBracket && foundCloseBracket)
                IsOpenBracketIsCloseBracket(c);
        }

        public void InsertParameterTokens(string token)
        {
            foreach (string arg in Parameters)
                ProcessedCommand = ProcessedCommand.Replace(arg, token);
        }

        public void RemoveParameterQuotes()
        {
            for (int i = 0; i < Parameters.Count; i++)
                if (Parameters[i].GetType().Equals(typeof(string)))
                    Parameters[i] = (Parameters[i] as string).Replace("\"", "");
        }

        public void AddCharToBuffer(char c)
        {
            argBuffer = argBuffer + c;
        }

        public void SetOpenQuote(bool found, char c)
        {
            foundOpenQuote = found;
            AddCharToBuffer(c);
        }

        public void SetCloseQuote(bool found, char c)
        {
            foundCloseQuote = found;
            AddCharToBuffer(c);
        }

        public void SetOpenBracket(bool found, char c)
        {
            foundOpenBracket = found;
            AddCharToBuffer(c);
        }

        public void SetCloseBracket(bool found, char c)
        {
            foundCloseBracket = found;
            AddCharToBuffer(c);
        }

        public void NotOpenQuoteNotOpenBracket(char c)
        {
            if (c.Equals(quote))
                SetOpenQuote(true, c);

            if (c.Equals(openBracket))
                SetOpenBracket(true, c);
        }

        public void IsOpenQuoteNotCloseQuote(char c)
        {
            if (c.Equals(quote))
            {
                SetCloseQuote(true, c);
                Parameters.Add(argBuffer);
                argBuffer = "";
            }

            else
                AddCharToBuffer(c);
        }

        public void IsOpenQuoteIsClosedQuote(char c)
        {
            foundOpenQuote = false;
            foundCloseQuote = false;

            if (c.Equals(quote))
                SetOpenQuote(true, c);
        }

        public void IsOpenBracketNotCloseBracket(char c)
        {
            if (c.Equals(closeBracket))
            {
                SetCloseBracket(true, c);
                Parameters.Add(argBuffer);
                argBuffer = "";
            }

            else
                AddCharToBuffer(c);
        }

        public void IsOpenBracketIsCloseBracket(char c)
        {
            foundOpenBracket = false;
            foundCloseBracket = false;

            if (c.Equals(openBracket))
                SetOpenBracket(true, c);
        }
    }
}
