namespace No.Added.Parser.Nodes
{
    using Code;

    public abstract class EndNode<V> : Node
    {
        public EndNode(DefaultParser parser, TokenCode code)
        {
            this.Value = this.Initialize(parser, code);
        }

        public V Value
        {
            get;

            private set;
        }

        protected abstract V Initialize(DefaultParser parser, TokenCode code);
    }
}
