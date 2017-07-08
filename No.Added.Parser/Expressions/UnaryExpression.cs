namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class UnaryExpression : RootNode
    {
        public UnaryExpression(DefaultParser parser, NodeType nodeType, TokenCode code) : base(parser, nodeType, code)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            this.Node.Accept(visitor);
        }

        protected override Node Initialize(DefaultParser parser, TokenCode code)
        {
            Nodes.Node node = parser.TryUnary(code);
            if (node != null)
            {
                return node;
            }

            node = parser.TryMember(code);
            if (node != null)
            {
                return node;
            }

            node = parser.TryCall(code);
            if (node != null)
            {
                return node;
            }

            node = code.ParseGroup(parser);
            if (node != null)
            {
                return node;
            }

            return parser.TryLiteral(code);
        }
    }
}
