namespace No.Added.Parser.Code
{
    using System;
    using Nodes;

    public class EndOfCode : TokenCode
    {
        public EndOfCode()
        {
            this.SingletonCheck();
            this.EndOfExpression = true;
        }

        public override T ParseExpression<T>(DefaultParser parser, Func<TokenCode, TokenCode, T> tokenOf)
        {
            return null;
        }

        public override Node ParseLiteral(DefaultParser parser)
        {
            return null;
        }

        public override void ParseList(DefaultParser parser, int separator, ListNode list)
        {
            // Does nothing.
        }

        internal override T ExpressionToken<T>(DefaultParser parser, TokenCode startLeft, TokenCode endLeft, Func<TokenCode, TokenCode, T> tokenOf)
        {
            return null;
        }

        internal override void ListToken(DefaultParser parser, int separator, ListNode list, TokenCode startLeft, TokenCode endLeft)
        {
            // Does nothing.
        }

        protected virtual void SingletonCheck()
        {
            if (Utils.EndOfCode != null)
            {
                throw new Exception("EndOfCode is a singleton. Use Utils.EndOfCode.");
            }
        }
    }
}
