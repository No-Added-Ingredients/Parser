namespace No.Added.Parser.Code
{
    using System;
    using Nodes;
    using Expressions;

    public class RawScript : TokenCode
    {
        public RawScript(string code) 
        {
            this.Text = string.Empty;
            this.Next = new Script(code);
            this.Next.Next = this;
        }

        public RawScript(Script code)
        {
            this.Text = string.Empty;
            this.Next = code;
            this.Next.Next = this;
        }

        public override T ParseExpression<T>(DefaultParser parser, Func<TokenCode, TokenCode, T> tokenOf)
        {
            return this.Next.ParseExpression(parser, tokenOf);
        }

        public override GroupExpression ParseGroup(DefaultParser parser)
        {
            return this.Next.ParseGroup(parser);
        }

        public override CallExpression ParseCall(DefaultParser parser)
        {
            return this.Next.ParseCall(parser);
        }

        internal override T ExpressionToken<T>(DefaultParser parser, TokenCode startLeft, TokenCode endLeft, Func<TokenCode, TokenCode, T> tokenOf)
        {
            return this.Next.ParseExpression(parser, tokenOf);
        }
    }
}
