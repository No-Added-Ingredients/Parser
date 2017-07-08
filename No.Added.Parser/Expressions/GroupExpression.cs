namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    /// <summary>
    /// In Ecma same as ParenthesizedExpression
    /// </summary>
    public class GroupExpression : RootNode
    {
        public GroupExpression(DefaultParser parser, NodeType nodeType, TokenCode code) : base(parser, nodeType, code)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            this.Node.Accept(visitor);
        }

        protected override Node Initialize(DefaultParser parser, TokenCode code)
        {
            Node node = parser.Parse(code);
            if (node != null)
            {
                return node;
            }

            throw parser.Error("Invalid empty group expression.");
        }
    }
}
