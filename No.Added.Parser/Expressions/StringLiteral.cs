namespace No.Added.Parser.Expressions
{
    using Nodes;
    using Code;

    public class StringLiteral : EndNode<string>
    {
        public StringLiteral(DefaultParser parser, TokenCode code) : base(parser, code)
        {
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        protected override string Initialize(DefaultParser parser, TokenCode code)
        {
            return code.ParseString(parser);
        }
    }
}
