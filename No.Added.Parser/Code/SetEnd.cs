namespace No.Added.Parser.Code
{
    using System;
    using Nodes;

    public class SetEnd : TokenCode
    {
        public SetEnd()
        {
            this.EndOfExpression = true;
        }

        public override T ParseExpression<T>(DefaultParser parser, Func<TokenCode, TokenCode, T> tokenOf)
        {
            return null;
        }

        public override void ParseList(DefaultParser parser, int separator, ListNode list)
        {
            // Does nothing.
        }

        internal override T ExpressionToken<T>(DefaultParser parser, TokenCode startLeft, TokenCode endLeft, Func<TokenCode, TokenCode, T> tokenOf)
        {
            // Cannot be right side of expression, so take next.
            return null;
        }

        internal override void ListToken(DefaultParser parser, int separator, ListNode list, TokenCode startLeft, TokenCode endLeft)
        {
            endLeft.Next = Utils.CodeSplit;
            list.Add(parser, startLeft);
        }
    }
}
