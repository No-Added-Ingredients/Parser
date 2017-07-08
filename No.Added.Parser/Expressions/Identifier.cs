namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class Identifier : EndNode<string>
    {
        public Identifier(DefaultParser parser, TokenCode code) : base(parser, code)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override string Initialize(DefaultParser parser, TokenCode code)
        {
            return code.Text;
        }
    }
}
