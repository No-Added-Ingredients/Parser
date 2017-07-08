namespace No.Added.Parser.Expressions
{
    public interface IVisitor
    {
        void Visit(MemberExpression node);

        void Visit(GroupExpression node);

        void Visit(StringLiteral node);

        void Visit(DecimalLiteral node);

        void Visit(LogicalExpression node);

        void Visit(AdditiveExpression node);

        void Visit(MultiplicativeExpression node);

        void Visit(EqualityExpression node);

        void Visit(RelationalExpression node);

        void Visit(UnaryExpression node);

        void Visit(CallExpression node);

        void Visit(Identifier node);

        void Visit(ArgumentList node);

        void Visit(ElementList node);

        void Visit(IntegerLiteral node);
    }
}
