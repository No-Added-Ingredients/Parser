namespace No.Added.Parser.Code
{
    using Nodes;
    using Expressions;

    public class IdentifierCode : TokenCode
    {
        public override Node ParseLiteral(DefaultParser parser)
        {
            if (!this.Next.EndOfExpression)
            {
                throw parser.Error("Literal is not end node.");
            }

            return new Identifier(parser, this);
        }
 
        public override CallExpression ParseCall(DefaultParser parser)
        {
            if (this.Next.Type == NodeType.Group)
            {
                var next = this.Next;
                this.Next = Utils.CodeSplit;
                return new CallExpression(parser, this, NodeType.Function, next);
            }

            return null;
        }
    }
}
