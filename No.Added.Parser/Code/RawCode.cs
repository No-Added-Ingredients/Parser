namespace No.Added.Parser.Code
{
    using System;
    using Nodes;
    using Expressions;

    public abstract class RawCode
    {
        public abstract T ParseExpression<T>(DefaultParser parser, Func<TokenCode, TokenCode, T> tokenOf) where T : Node;

        public abstract void ParseList(DefaultParser parser, int separator, ListNode list);

        public abstract Node ParseLiteral(DefaultParser parser);

        public abstract int ParseInt32(DefaultParser parser);

        public abstract double ParseDouble(DefaultParser parser);

        public abstract string ParseString(DefaultParser parser);

        public abstract GroupExpression ParseGroup(DefaultParser parser);

        public abstract CallExpression ParseCall(DefaultParser parser);
    }
}
