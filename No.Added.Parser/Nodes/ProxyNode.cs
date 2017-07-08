namespace No.Added.Parser.Nodes
{
    using System;
    using Expressions;

    public class ProxyNode : Node
    {
        private Func<Node> process;

        private Node node = null;

        public ProxyNode(Func<Node> processor)
        {
            this.process = processor;
        }

        public override void Accept(IVisitor visitor)
        {
            this.MySelf().Accept(visitor);
        }

        internal override Node MySelf()
        {
            if (this.process == null)
            {
                if (this.node != null)
                {
                    return this.node;
                }

                throw this.Exception("Proxy processor not assigned.");
            }

            this.node = this.process();
            if (this.node == null)
            {
                throw this.Exception("Fail to parse proxy.");
            }

            this.process = null;
            return this.node.MySelf();
        }
    }
}
