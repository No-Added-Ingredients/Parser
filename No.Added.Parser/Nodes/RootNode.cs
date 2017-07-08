namespace No.Added.Parser.Nodes
{
    using Code;

    public abstract class RootNode : Node
    {
        private Node node = null;

        public RootNode(DefaultParser parser, NodeType nodeType, TokenCode code)
        {
            this.Type = nodeType;
            this.node = new ProxyNode(() => { return this.Initialize(parser, code); });
        }

        public Node Node
        {
            get
            {
                return this.node.MySelf();
            }
        }

        protected abstract Node Initialize(DefaultParser parser, TokenCode code);
    }
}
