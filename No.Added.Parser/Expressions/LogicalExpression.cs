﻿namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class LogicalExpression : EqualityExpression
    {
        public LogicalExpression(DefaultParser parser, TokenCode left, NodeType nodeType, TokenCode right) : base(parser, left, nodeType, right)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            this.Left.Accept(visitor);
            this.Right.Accept(visitor);
        }

        protected override Node Initialize(DefaultParser parser, TokenCode code)
        {
            Nodes.Node node = parser.TryLogical(code);
            if (node != null)
            {
                return node;
            }

            return base.Initialize(parser, code);
        }
    }
}
