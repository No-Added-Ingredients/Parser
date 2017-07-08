namespace No.Added.Parser.Nodes
{
    using Code;

    public abstract class TreeNode : Node
    {
        public TreeNode(DefaultParser parser, TokenCode left, NodeType nodeType, TokenCode right)
        {
            this.Type = nodeType;
            this.LeftNode = new ProxyNode(() => { return this.InitializeLeft(parser, left); });
            this.RightNode = new ProxyNode(() => { return this.InitializeRight(parser, right); });
        }

        public Node Left
        {
            get
            {
                return this.LeftNode.MySelf();
            }
        }

        public Node Right
        {
            get
            {
                return this.RightNode.MySelf();
            }
        }

        protected Node LeftNode { get; set; } = null;

        protected Node RightNode { get; set; } = null;

        protected abstract Node InitializeLeft(DefaultParser parser, TokenCode code);

        protected abstract Node InitializeRight(DefaultParser parser, TokenCode code);
    }
}
