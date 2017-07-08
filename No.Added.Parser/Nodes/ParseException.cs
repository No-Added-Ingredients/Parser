namespace No.Added.Parser.Nodes
{
    using System;

    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }
}
