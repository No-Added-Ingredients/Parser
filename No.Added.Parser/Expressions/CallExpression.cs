namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class CallExpression : TreeNode
    {
        public CallExpression(DefaultParser parser, TokenCode left, NodeType nodeType, TokenCode right) : base(parser, left, nodeType, right)
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
            Node node = parser.TryUnary(code);
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

            throw this.Exception(string.Format("Invalid left node for {0}: {1}", this.Type, code));
        }

        protected override Node InitializeRight(DefaultParser parser, TokenCode code)
        {
            Node node = parser.TryArgumentList(code);
            if (node != null)
            {
                this.RightNode = node;
                return node;
            }

            throw this.Exception(string.Format("Invalid right node for {0}: {1}", this.Type, code.Text));
        }
    }
}
