namespace No.Added.Parser.Expressions
{
    using System;
    using Nodes;
    using Code;

    public class ElementList : ListNode
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override Node InitializeNode(DefaultParser parser, TokenCode code, int index)
        {
            throw new NotImplementedException();
        }
    }
}
