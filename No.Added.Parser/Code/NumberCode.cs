namespace No.Added.Parser.Code
{
    using System;
    using Nodes;
    using Expressions;

    public class NumberCode : TokenCode
    {
        public override int ParseInt32(DefaultParser parser)
        {
            switch (this.Type)
            {
                case NodeType.BinaryInteger:
                    return Convert.ToInt32(this.Text.Substring(2), 2);

                case NodeType.Heximal:
                    return Convert.ToInt32(this.Text.Substring(2), 16);

                case NodeType.OctalInteger:
                    return Convert.ToInt32(this.Text.Substring(2), 8);

                case NodeType.Integer:
                    return Convert.ToInt32(this.Text);

                case NodeType.Decimal:
                case NodeType.ExponentDecimal:
                    return base.ParseInt32(parser); 

                default:
                    throw parser.Error("Unexpected node type");
            }
        }

        public override double ParseDouble(DefaultParser parser)
        {
            switch (this.Type)
            {
                case NodeType.BinaryInteger:
                case NodeType.Heximal:
                case NodeType.OctalInteger:
                case NodeType.Integer:
                    return base.ParseDouble(parser);

                case NodeType.Decimal:
                case NodeType.ExponentDecimal:
                    return Convert.ToDouble(this.Text);

                default:
                    throw parser.Error("Unexpected node type");
            }

            throw parser.Error(string.Format("This is not a Double: {0}.", this.Text));
        }

        public override Node ParseLiteral(DefaultParser parser)
        {
            if (!this.Next.EndOfExpression)
            {
                throw parser.Error("Literal is not end node.");
            }

            switch (this.Type)
            {
                case NodeType.BinaryInteger:
                case NodeType.Heximal:
                case NodeType.OctalInteger:
                case NodeType.Integer:
                    return new IntegerLiteral(parser, this);

                case NodeType.Decimal:
                case NodeType.ExponentDecimal:
                    return new DecimalLiteral(parser, this);

                default:
                    throw parser.Error("Unexpected node type");
            }
        }
    }
}
