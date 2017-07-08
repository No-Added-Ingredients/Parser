namespace No.Added.Parser.Nodes
{
    using System.Collections.Generic;
    using Code;

    public abstract class ListNode : Node
    {
        public int Count
        {
            get
            {
                return this.Nodes.Count;
            }
        }

        protected List<Node> Nodes { get; set; } = new List<Node>();

        public Node this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Nodes.Count)
                {
                    return null;
                }

                return this.Nodes[index].MySelf();
            }
        }

        public void Add(DefaultParser parser, TokenCode code)
        {
            var index = this.Nodes.Count;
            this.Nodes.Add(
                new ProxyNode(
                    () => 
                    {
                        return this.InitializeNode(parser, code, index);
                    }));
        }

        protected abstract Node InitializeNode(DefaultParser parser, TokenCode code, int index);
    }
}
