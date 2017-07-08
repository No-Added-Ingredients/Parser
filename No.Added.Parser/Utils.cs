namespace No.Added.Parser
{
    using Code;

    public static class Utils
    {
        /// <summary>
        /// Tab 
        /// </summary>
        public const int Tab = 0x9;

        /// <summary>
        /// Form Feed
        /// </summary>
        public const int FF = 0xC;

        /// <summary>
        /// Vertical Tab
        /// </summary>
        public const int VTab = 0xB;

        /// <summary>
        /// No-break space
        /// </summary>
        public const int NBSpace = 0xA0;

        /// <summary>
        /// BOM or ZWNBSP is ZERO WIDTH NO‑BREAK SPACE
        /// </summary>
        public const int BOM = 0xFEFF;

        /// <summary>
        /// Carriage Return
        /// </summary>
        public const int CR = 0xD;

        /// <summary>
        /// Line feed
        /// </summary>
        public const int LF = 0xA;

        /// <summary>
        /// Backspace
        /// </summary>
        public const int BS = 0x8;

        /// <summary>
        /// Line separator
        /// </summary>
        public const int LS = 0x2028;

        /// <summary>
        /// Paragraph separator
        /// </summary>
        public const int PS = 0x2029;

        /// <summary>
        /// "
        /// </summary>
        public const int Quotation = 0x22;

        /// <summary>
        /// '
        /// </summary>
        public const int Apostrophe = 0x27;

        /// <summary>
        /// 0
        /// </summary>
        public const int ZeroDigit = 0x30;

        /// <summary>
        /// 1
        /// </summary>
        public const int OneDigit = 0x31;

        /// <summary>
        /// 7
        /// </summary>
        public const int SevenDigit = 0x37;

        /// <summary>
        /// 9
        /// </summary>
        public const int NineDigit = 0x39;

        /// <summary>
        /// A
        /// </summary>
        public const int CapitalA = 0x41;

        /// <summary>
        /// B
        /// </summary>
        public const int CapitalB = 0x42;

        /// <summary>
        /// Z
        /// </summary>
        public const int CapitalZ = 0x5A;

        /// <summary>
        /// O
        /// </summary>
        public const int CapitalO = 0x4F;

        /// <summary>
        /// a
        /// </summary>
        public const int SmallA = 0x61;

        /// <summary>
        /// z
        /// </summary>
        public const int SmallZ = 0x7A;

        /// <summary>
        /// F
        /// </summary>
        public const int CapitalF = 0x46;

        /// <summary>
        /// f
        /// </summary>
        public const int SmallF = 0x66;

        /// <summary>
        /// _
        /// </summary>
        public const int LowLine = 0x5F;

        /// <summary>
        /// $
        /// </summary>
        public const int Dollar = 0x24;

        /// <summary>
        /// E
        /// </summary>
        public const int CapitalE = 0x45;

        /// <summary>
        /// e
        /// </summary>
        public const int SmallE = 0x65;

        /// <summary>
        /// X
        /// </summary>
        public const int CapitalX = 0x58;

        /// <summary>
        /// x
        /// </summary>
        public const int SmallX = 0x78;

        /// <summary>
        /// b
        /// </summary>
        public const int SmallB = 0x62;

        /// <summary>
        /// n
        /// </summary>
        public const int SmallN = 0x6E;

        /// <summary>
        /// r
        /// </summary>
        public const int SmallR = 0x72;

        /// <summary>
        /// t
        /// </summary>
        public const int SmallT = 0x74;

        /// <summary>
        /// u
        /// </summary>
        public const int SmallU = 0x75;

        /// <summary>
        /// v
        /// </summary>
        public const int SmallV = 0x76;

        /// <summary>
        /// g
        /// </summary>
        public const int SmallG = 0x67;

        /// <summary>
        /// i
        /// </summary>
        public const int SmallI = 0x69;

        /// <summary>
        /// m
        /// </summary>
        public const int SmallM = 0x6D;

        /// <summary>
        /// o
        /// </summary>
        public const int SmallO = 0x6F;

        /// <summary>
        /// Space
        /// </summary>
        public const int Space = 0x20;

        /// <summary>
        /// {
        /// </summary>
        public const int OpenCurlyBracket = 0x7B;

        /// <summary>
        /// }
        /// </summary>
        public const int CloseCurlyBracket = 0x7D;

        /// <summary>
        /// (
        /// </summary>
        public const int OpenParenthesis = 0x28;

        /// <summary>
        /// )
        /// </summary>
        public const int CloseParenthesis = 0x29;

        /// <summary>
        /// [
        /// </summary>
        public const int OpenSquareBracket = 0x5B;

        /// <summary>
        /// ]
        /// </summary>
        public const int CloseSquareBracket = 0x5D;

        /// <summary>
        /// .
        /// </summary>
        public const int FullStop = 0x2E;

        /// <summary>
        /// ;
        /// </summary>
        public const int Semicolon = 0x3B;

        /// <summary>
        /// ,
        /// </summary>
        public const int Comma = 0x2C;

        /// <summary>
        /// ?
        /// </summary>
        public const int QuestionMark = 0x3F;

        /// <summary>
        /// :
        /// </summary>
        public const int Colon = 0x3A;

        /// <summary>
        /// ~
        /// </summary>
        public const int Tilde = 0x7E;

        /// <summary>
        /// <
        /// </summary>
        public const int LessThan = 0x3C;

        /// <summary>
        /// >
        /// </summary>
        public const int GreaterThan = 0x3E;

        /// <summary>
        /// ^
        /// </summary>
        public const int Circumflex = 0x5E;

        /// <summary>
        /// !
        /// </summary>
        public const int Exclamation = 0x21;

        /// <summary>
        /// %
        /// </summary>
        public const int Percent = 0x25;

        /// <summary>
        /// &
        /// </summary>
        public const int Ampersand = 0x26;

        /// <summary>
        /// |
        /// </summary>
        public const int Pipe = 0x7C;

        /// <summary>
        /// +
        /// </summary>
        public const int PlusSign = 0x2B;

        /// <summary>
        /// -
        /// </summary>
        public const int MinusSign = 0x2D;

        /// <summary>
        /// /
        /// </summary>
        public const int Solidus = 0x2F;

        /// <summary>
        /// \
        /// </summary>
        public const int Backslash = 0x5C;

        /// <summary>
        /// *
        /// </summary>
        public const int Asterisk = 0x2A;

        /// <summary>
        /// =
        /// </summary>
        public const int EqualsSign = 0x3D;

        public static EndOfExpression CodeSplit { get; internal set; } = new EndOfExpression();

        public static EndOfCode EndOfCode { get; internal set; } = new EndOfCode();
    }
}
