namespace No.Added.Parser.Code
{
    using System;
    using Nodes;
    using Expressions;

    /// <summary>
    /// Start of array, group or block.
    /// </summary>
    public class SetStart : TokenCode
    {
        public SetEnd End { get; internal set; } = null;

        public override T ParseExpression<T>(DefaultParser parser, Func<TokenCode, TokenCode, T> tokenOf)
        {
            if (this.End == null)
            {
                return this.Next.ExpressionToken(parser, this, this, tokenOf);
            }

            return this.End.Next.ExpressionToken(parser, this, this.End, tokenOf);
        }

        public override GroupExpression ParseGroup(DefaultParser parser)
        {
            if (this.Type == NodeType.Group)
            {
                this.End.Next = Utils.CodeSplit;
                return new GroupExpression(parser, NodeType.RoundOpenBracket, this.Next);
            }

            return null;
        }

        public override void ParseList(DefaultParser parser, int separator, ListNode list)
        {
            if (this.Next.EndOfExpression)
            {
                return;
            }

            this.Next.ParseList(parser, separator, list);
        }

        internal override T ExpressionToken<T>(DefaultParser parser, TokenCode startLeft, TokenCode endLeft, Func<TokenCode, TokenCode, T> tokenOf)
        {
            var next = this.End.Next;
            if (next == Utils.CodeSplit)
            {
                // Parser split the code in left and right immediately after this set.
                return base.ExpressionToken(parser, startLeft, endLeft, tokenOf);
            }

            if (next == Utils.EndOfCode)
            {
                return tokenOf(startLeft, this);
            }

            // Skipping code inside set and forcing splitting of code immediately after this set.
            this.End.Next = Utils.CodeSplit;
            var result = tokenOf(startLeft, next);
            if (result == null)
            {
                this.End.Next = next;
                return this.End.Next.ExpressionToken(parser, startLeft, this.End, tokenOf);
            }

            // Making this.End.Next != Utils.CodeSplit
            // avoiding that this.End.Next == Utils.CodeSplit is used a second time.
            this.End.Next = Utils.EndOfCode;
            return result;
        }

        internal override void ListToken(DefaultParser parser, int separator, ListNode list, TokenCode startLeft, TokenCode endLeft)
        {
            this.Next.ListToken(parser, separator, list, startLeft, endLeft);
        }
    }
}
