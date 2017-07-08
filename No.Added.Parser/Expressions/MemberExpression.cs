namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class MemberExpression : TreeNode
    {
        public MemberExpression(DefaultParser parser, TokenCode left, NodeType nodeType, TokenCode right) : base(parser, left, nodeType, right)
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
            Node node = parser.TryCall(code);
            if (node != null)
            {
                this.LeftNode = node;
                return node;
            }

            node = parser.TryLiteral(code);
            if (node != null)
            {
                this.LeftNode = node;
                return node;
            }

            throw parser.Error("Member expression expected");
        }

        protected override Node InitializeRight(DefaultParser parser, TokenCode code)
        {
            Node node = parser.TryMember(code);
            if (node != null)
            {
                this.RightNode = node;
                return node;
            }

            node = parser.TryCall(code);
            if (node != null)
            {
                this.RightNode = node;
                return node;
            }

            node = parser.TryIdentifier(code);
            if (node != null)
            {
                this.RightNode = node;
                return node;
            }

            throw parser.Error("Identifier expected");
        }
    }
}
