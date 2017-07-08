namespace No.Added.Parser.Code
{
    using System;

    public class EndOfExpression : EndOfCode
    {
        protected override void SingletonCheck()
        {
            if (Utils.CodeSplit != null)
            {
                throw new Exception("EndOfExpression is a singleton. Use Utils.EndOfExpression.");
            }
        }
    }
}
