namespace No.Added.Parser.Nodes
{
    using System;
    using Expressions;

    public abstract class Node
    {
        public Node()
        {
        }

        public NodeType Type { get; internal set; } = NodeType.Unknown;

        public abstract void Accept(IVisitor visitor);

        internal virtual Node MySelf()
        {
            return this;
        } 

        protected virtual Exception Exception(string error) 
        {
            return new ParseException(error);
        }
    }
}
