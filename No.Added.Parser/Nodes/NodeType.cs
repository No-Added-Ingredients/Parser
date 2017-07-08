namespace No.Added.Parser.Nodes
{
    public enum NodeType
    {
        Unknown,
        Integer,
        Heximal,
        Decimal,
        ExponentDecimal,
        OctalInteger,
        BinaryInteger,

        // { }
        Block,

        // ( )
        Group,

        // [ ]
        Array,

        IdentifierName,
        NameSeparator,

        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Addition,
        Subtraction,
        Division,
        Multiplication,
        Modulus,
        LogicalAND,
        LogicalOR,

        Plus,
        Minus,
        Not,

        Function,

        SingleQuote,
        DoubleQuote,
        RoundOpenBracket,
        RoundCloseBracket,
        SquareOpenBracket,
        SquareCloseBracket,
        CurlyOpenBracket,
        CurlyCloseBracket,
        Space,
        Number
    }
}
