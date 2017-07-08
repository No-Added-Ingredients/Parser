namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class MultiplicativeExpression : TreeNode
    {
        public MultiplicativeExpression(DefaultParser parser, TokenCode left, NodeType nodeType, TokenCode right) : base(parser, left, nodeType, right)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
            this.Left.Accept(visitor);
            this.Right.Accept(visitor);
        }

        protected override Node InitializeLeft(DefaultParser parser, TokenCode code)
        {
            Nodes.Node node = this.Initialize(parser, code);
            if (node != null)
            {
                this.LeftNode = node;
                return node;
            }

            throw this.Exception(string.Format("Invalid left node for {0}: {1}", this.Type, code.Text));
        }

        protected override Node InitializeRight(DefaultParser parser, TokenCode code)
        {
            Node node = this.Initialize(parser, code);
            if (node != null)
            {
                this.RightNode = node;
                return node;
            }

            throw this.Exception(string.Format("Invalid right node for {0}: {1}", this.Type, code));
        }

        protected virtual Node Initialize(DefaultParser parser, TokenCode code)
        {
            Node node = parser.TryMultiplicative(code);
            if (node != null)
            {
                return node;
            }

            node = parser.TryUnary(code);
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
