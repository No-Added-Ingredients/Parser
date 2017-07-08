﻿namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class IntegerLiteral : EndNode<int>
    {
        public IntegerLiteral(DefaultParser parser, TokenCode code) : base(parser, code)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override int Initialize(DefaultParser parser, TokenCode code)
        {
            return code.ParseInt32(parser);
        }
    }
}
