namespace No.Added.Parser
{
    using System;
    using Code;
    using Expressions;
    using Nodes;
    using Readers;

    public class DefaultParser
    {
        private Func<string, NodeType> tokenOfTree;

        private Func<string, NodeType> tokenOfRoot;

        private Func<char, NodeType> separateToken;

        public DefaultParser()
        {
            this.separateToken = token =>
            {
                switch (token)
                {
                    case '"':
                        return NodeType.SingleQuote;

                    case '\'':
                        return NodeType.DoubleQuote;

                    case '(':
                        return NodeType.Minus;

                    default:
                        return NodeType.Unknown;
                }
            };

            this.tokenOfRoot = code =>
            {
                switch (code)
                {
                    case "!":
                        return NodeType.Not;

                    case "+":
                        return NodeType.Plus;

                    case "-":
                        return NodeType.Minus;

                    default:
                        return NodeType.Unknown;
                }
            };

            this.tokenOfTree = code =>
            {
                switch (code)
                {
                    case "\"":
                        return NodeType.DoubleQuote;

                    case "'":
                        return NodeType.SingleQuote;

                    case "+":
                        return NodeType.Addition;

                    case "-":
                        return NodeType.Subtraction;

                    case "*":
                        return NodeType.Multiplication;

                    case "/":
                        return NodeType.Division;

                    case "%":
                        return NodeType.Modulus;

                    case "==":
                        return NodeType.Equal;

                    case "!=":
                        return NodeType.NotEqual;

                    case ">":
                        return NodeType.GreaterThan;

                    case ">=":
                        return NodeType.GreaterThanOrEqual;

                    case "<":
                        return NodeType.LessThan;

                    case "<=":
                        return NodeType.LessThanOrEqual;

                    case "||":
                        return NodeType.LogicalOR;

                    case "&&":
                        return NodeType.LogicalAND;

                    case ".":
                        return NodeType.NameSeparator;

                    default:
                        return NodeType.Unknown;
                }
            };
        }

        public virtual MemberExpression TryMember(TokenCode code)
        {
            return code.ParseExpression<MemberExpression>(
                this,
                (left, right) =>
                {
                    var operatorType = this.tokenOfTree(right.Text);
                    switch (operatorType)
                    {
                        case NodeType.NameSeparator:
                            if (right.Next.Type != NodeType.IdentifierName)
                            {
                                throw this.Error("Indentifier name expected");
                            }

                            return new MemberExpression(this, left, operatorType, right.Next);

                        default:
                            return null;
                    }
                });
        }

        public virtual Node TryIdentifier(TokenCode code)
        {
            if (code.Type != NodeType.IdentifierName)
            {
                return null;
            }

            return code.ParseLiteral(this);
        }

        public virtual bool IsWhitespace(int code)
        {
            return char.IsWhiteSpace((char)code);
        }

        public virtual bool IsStartIndentifier(int code)
        {
            return (code >= Utils.CapitalA && code <= Utils.CapitalZ)
                || (code >= Utils.SmallA && code <= Utils.SmallZ)
                || (code == Utils.LowLine || code == Utils.Dollar);
        }

        public virtual bool IsStartText(int code)
        {
            return code == Utils.Apostrophe || code == Utils.Quotation;
        }

        public virtual bool IsStartUnary(int code, ScriptReader stream)
        {
            switch (code)
            {
                case Utils.PlusSign:
                case Utils.MinusSign:
                    return true;

                case Utils.Exclamation:
                    if (stream.Peek() == Utils.EqualsSign)
                    {
                        return false;
                    }

                    return true;

                default:
                    return false;
            }
        }

        public virtual TokenCode ParseUnaryCode(ScriptReader stream)
        {
            // Is potentieel unary token.
            return new TokenCode() { Text = stream.SplitAt(stream.Position) };
        }

        public virtual NodeType TypeSetOpenOf(int code)
        {
            switch (code)
            {
                case Utils.OpenCurlyBracket:
                    return NodeType.Block;

                case Utils.OpenParenthesis:
                    return NodeType.Group;

                case Utils.OpenSquareBracket:
                    return NodeType.Array;

                default:
                    return NodeType.Unknown;
            }
        }

        public virtual bool IsEndGroup(int code, int startToken)
        {
            switch (code)
            {
                case Utils.CloseCurlyBracket:
                    if (startToken == Utils.OpenCurlyBracket)
                    {
                        return true;
                    }

                    throw this.Error(string.Format("Unexpected: {0}.", Convert.ToChar(code)));

                case Utils.CloseParenthesis:
                    if (startToken == Utils.OpenParenthesis)
                    {
                        return true;
                    }

                    throw this.Error(string.Format("Unexpected: {0}.", Convert.ToChar(code)));

                case Utils.CloseSquareBracket:
                    if (startToken == Utils.OpenSquareBracket)
                    {
                        return true;
                    }

                    throw this.Error(string.Format("Unexpected: {0}.", Convert.ToChar(code)));

                default:
                    return false;
            }
        }

        public virtual bool IsNameSeparator(int code)
        {
            // Do not use before IsStartNumber
            return code == Utils.FullStop;
        }

        public virtual TokenCode ParseNameSeparatorCode(ScriptReader stream)
        {
            return new TokenCode() { Text = stream.SplitAt(stream.Position), Type = NodeType.NameSeparator };
        }

        public virtual IdentifierCode ParseIndentifierCode(ScriptReader stream)
        {
            for (var code = stream.Read(this); code != -1; code = stream.Read(this))
            {
                if (this.IsStartIndentifier(code))
                {
                    continue;
                }

                break;
            }

            return new IdentifierCode() { Text = stream.SplitAt(stream.Position - 1), Type = NodeType.IdentifierName };
        }

        public virtual TokenCode ParseNumberCode(ScriptReader stream, int code)
        {
            switch (code)
            {
                case Utils.FullStop:
                    return this.ParseDecimalCode(NodeType.Decimal, stream);

                case Utils.ZeroDigit:
                    code = stream.Read(this);
                    switch (code)
                    {
                        case Utils.SmallX:
                        case Utils.CapitalX:
                            return this.ParseHeximalCode(stream);

                        case Utils.SmallO:
                        case Utils.CapitalO:
                            return this.ParseOctalIntegerCode(stream);

                        case Utils.SmallB:
                        case Utils.CapitalB:
                            return this.ParseBinaryIntegerCode(stream);

                        case Utils.SmallE:
                        case Utils.CapitalE:
                            return this.ParseExponentPart(stream);

                        case Utils.FullStop:
                            return this.ParseDecimalCode(NodeType.Decimal, stream);

                        default:
                            return new NumberCode() { Text = stream.SplitAt(stream.Position - 1), Type = NodeType.Integer };
                    }

                default:
                    return this.ParseDecimalCode(NodeType.Integer, stream);
            }
        }

        public virtual bool IsStartNumber(int code, ScriptReader stream)
        {
            var test = code;
            if (test == Utils.FullStop)
            {
                test = stream.Peek();
            }

            return test >= Utils.ZeroDigit && test <= Utils.NineDigit;
        }

        public virtual Exception Error(string message)
        {
            return new ParseException(message);
        }

        public virtual TokenCode ParseTextCode(int quotations, ScriptReader stream)
        {
            stream.CutAt(stream.Position - 1);
            var text = string.Empty;
            for (var code = stream.Read(this); code != -1; code = stream.Read(this))
            {
                if (this.IsLineTerminator(code))
                {
                    throw this.Error("Unexpected line termination.");
                }

                if (code == quotations)
                {
                    text += stream.CutAt(stream.Position - 1);
                    return new TextCode() { Text = text };
                }

                if (code == Utils.Backslash)
                {
                    code = stream.Read(this);
                    switch (code)
                    {
                        case Utils.CR:
                            text += stream.CutAt(stream.Position - 1);
                            if (stream.Peek() == Utils.LF)
                            {
                                // Double Line Continuation
                                stream.CutAt(2);
                                break;
                            }

                            // Single Line Continuation
                            stream.CutAt(1);
                            break;

                        case Utils.LF:
                        case Utils.LS:
                        case Utils.PS:
                            text += stream.CutAt(stream.Position - 1);
                            stream.CutAt(1);
                            break;

                        default:
                            text += this.EscapeSequence(stream, code);
                            break;
                    }
                }
            }

            throw this.Error("Unterminated string.");
        }

        public Node Parse(TokenCode code)
        {
            Node result = this.TryLogical(code);
            if (result != null)
            {
                return result;
            }

            result = this.TryEquality(code);
            if (result != null)
            {
                return result;
            }

            result = this.TryRelational(code);
            if (result != null)
            {
                return result;
            }

            result = this.TryAdditive(code);
            if (result != null)
            {
                return result;
            }

            result = this.TryMultiplicative(code);
            if (result != null)
            {
                return result;
            }

            result = this.TryUnary(code);
            if (result != null)
            {
                return result;
            }

            result = this.TryMember(code);
            if (result != null)
            {
                return result;
            }

            result = this.TryCall(code);
            if (result != null)
            {
                return result;
            }

            result = code.ParseGroup(this);
            if (result != null)
            {
                return result;
            }

            return this.TryLiteral(code);
        }

        public LogicalExpression TryGroup(Node code)
        {
            return null;
        }

        public LogicalExpression TryLogical(TokenCode code)
        {
            return code.ParseExpression<LogicalExpression>(
                this,
                (left, right) =>
                {
                    var operatorType = this.tokenOfTree(right.Text);
                    switch (operatorType)
                    {
                        case NodeType.LogicalAND:
                        case NodeType.LogicalOR:
                            return new LogicalExpression(this, left, operatorType, right.Next);

                        default:
                            return null;
                    }
                });
        }

        public AdditiveExpression TryAdditive(TokenCode code)
        {
            return code.ParseExpression<AdditiveExpression>(
                this,
                (left, right) =>
                {
                    var operatorType = this.tokenOfTree(right.Text);
                    switch (operatorType)
                    {
                        case NodeType.Addition:
                        case NodeType.Subtraction:
                            return new AdditiveExpression(this, left, operatorType, right.Next);
                        default:
                            return null;
                    }
                });
        }

        public MultiplicativeExpression TryMultiplicative(TokenCode code)
        {
            return code.ParseExpression<MultiplicativeExpression>(
                this,
                (left, right) =>
                {
                    var operatorType = this.tokenOfTree(right.Text);
                    switch (operatorType)
                    {
                        case NodeType.Multiplication:
                        case NodeType.Division:
                        case NodeType.Modulus:
                            return new MultiplicativeExpression(this, left, operatorType, right.Next);
                        default:
                            return null;
                    }
                });
        }

        public EqualityExpression TryEquality(TokenCode code)
        {
            return code.ParseExpression<EqualityExpression>(
                this,
                (left, right) =>
                {
                    var operatorType = this.tokenOfTree(right.Text);
                    switch (operatorType)
                    {
                        case NodeType.NotEqual:
                        case NodeType.Equal:
                            return new EqualityExpression(this, left, operatorType, right.Next);

                        default:
                            return null;
                    }
                });
        }

        public UnaryExpression TryUnary(TokenCode code)
        {
            return code.ParseExpression<UnaryExpression>(
                this,
                (left, right) =>
                {
                    var operatorType = this.tokenOfRoot(left.Text);
                    switch (operatorType)
                    {
                        case NodeType.Not:
                        case NodeType.Minus:
                        case NodeType.Plus:
                            return new UnaryExpression(this, operatorType, right);

                        default:
                            return null;
                    }
                });
        }

        public CallExpression TryCall(TokenCode code)
        {
            return code.ParseCall(this);
            ////if (code.Type != NodeType.Identifier)
            ////{
            ////    return null;
            ////}

            ////var startLeft = code;
            ////var test = code.Next;
            ////do
            ////{
            ////    switch(test.Type)
            ////    {
            ////        case NodeType.Group:
            ////            return new CallExpression(this, startLeft, NodeType.Function, test);

            ////        case NodeType.IdentifierSeparator:
            ////            test = code.Next;
            ////            if (test.Type == NodeType.Identifier)
            ////            {
            ////                test = code.Next;
            ////                continue;
            ////            }

            ////            return null;

            ////        default:
            ////            return null;
            ////    }

            ////} while (true);
        }

        public Node TryLiteral(TokenCode code)
        {
            return code.ParseLiteral(this);
        }

        public ArgumentList TryArgumentList(TokenCode code)
        {
            if (code.Type != NodeType.Group)
            {
                return null;
            }

            (code as SetStart).End.Next = Utils.CodeSplit;
            var result = new ArgumentList();
            code.ParseList(this, Utils.Comma, result);
            return result;
        }
 
        public RelationalExpression TryRelational(TokenCode code)
        {
            return code.ParseExpression<RelationalExpression>(
                this, 
                (left, right) =>
                {
                    var operatorType = this.tokenOfTree(right.Text);
                    switch (operatorType)
                    {
                        case NodeType.LessThanOrEqual:
                        case NodeType.LessThan:
                        case NodeType.GreaterThanOrEqual:
                        case NodeType.GreaterThan:
                            return new RelationalExpression(this, left, operatorType, right.Next);
                        default:
                            return null;
                }
            });
        }

        protected virtual Exception Exception(string error)
        {
            return new ParseException(error);
        }

        protected virtual bool IsLineTerminator(int code)
        {
            return code == Utils.CR || code == Utils.LF || code == Utils.LS || code == Utils.PS;
        }

        protected virtual TokenCode ParseExponentPart(ScriptReader stream)
        {
            var code = stream.Read(this);
            if (!(code == Utils.MinusSign || code == Utils.PlusSign))
            {
                throw this.Error("Expect '+' or '-'");
            }

            for (code = stream.Read(this); code != -1; code = stream.Read(this))
            {
                if (code >= Utils.ZeroDigit && code <= Utils.NineDigit)
                {
                    continue;
                }

                break;
            }

            if (stream.Position < 4)
            {
                throw this.Error("Unexpected end of exponent part.");
            }

            return new NumberCode() { Text = stream.SplitAt(stream.Position - 1), Type = NodeType.ExponentDecimal };
        }

        protected virtual TokenCode ParseDecimalCode(NodeType offsetType, ScriptReader stream)
        {
            var type = offsetType;
            for (var code = stream.Read(this); code != -1; code = stream.Read(this))
            {
                if (code >= Utils.ZeroDigit && code <= Utils.NineDigit)
                {
                    continue;
                }

                switch (code)
                {
                    case Utils.CapitalE:
                    case Utils.SmallE:
                        return this.ParseExponentPart(stream);

                    case Utils.FullStop:
                        if (type == NodeType.Decimal)
                        {
                            throw this.Error("Unexpected '.'");
                        }

                        type = NodeType.Decimal;
                        continue;
                }

                break;
            }

            return new NumberCode() { Text = stream.SplitAt(stream.Position - 1), Type = type };
        }

        protected virtual TokenCode ParseBinaryIntegerCode(ScriptReader stream)
        {
            for (var code = stream.Read(this); code != -1; code = stream.Read(this))
            {
                if (code == Utils.ZeroDigit || code == Utils.OneDigit)
                {
                    continue;
                }

                break;
            }

            if (stream.Position < 4)
            {
                throw this.Error("Unexpected end of binary number.");
            }

            return new NumberCode() { Text = stream.SplitAt(stream.Position - 1), Type = NodeType.BinaryInteger };
        }

        protected virtual TokenCode ParseOctalIntegerCode(ScriptReader stream)
        {
            for (var code = stream.Read(this); code != -1; code = stream.Read(this))
            {
                if (code >= Utils.ZeroDigit && code <= Utils.SevenDigit)
                {
                    continue;
                }

                break;
            }

            if (stream.Position < 4)
            {
                throw this.Error("Unexpected end of octal number.");
            }

            return new NumberCode() { Text = stream.SplitAt(stream.Position - 1), Type = NodeType.OctalInteger };
        }

        protected virtual TokenCode ParseHeximalCode(ScriptReader stream)
        {
            for (var code = stream.Read(this); code != -1; code = stream.Read(this))
            {
                if (code >= Utils.ZeroDigit && code <= Utils.NineDigit)
                {
                    continue;
                }

                if (code >= Utils.SmallA && code <= Utils.SmallF)
                {
                    continue;
                }

                if (code >= Utils.CapitalA && code <= Utils.CapitalF)
                {
                    continue;
                }

                break;
            }

            if (stream.Position < 4)
            {
                throw this.Error("Unexpected end of heximal number.");
            }

            return new NumberCode() { Text = stream.SplitAt(stream.Position - 1), Type = NodeType.Heximal };
        }

        protected virtual string EscapeSequence(ScriptReader stream, int code)
        {
            switch (code)
            {
                case Utils.Quotation:
                case Utils.Apostrophe:
                case Utils.Backslash:
                case Utils.QuestionMark:
                    var text = stream.CutAt(stream.Position - 2) + Convert.ToChar(code);
                    stream.CutAt(0);
                    return text;

                // New line
                case Utils.SmallN:
                    text = stream.CutAt(stream.Position - 2) + Convert.ToChar(Utils.LF);
                    stream.CutAt(0);
                    return text;

                // Backspace
                case Utils.SmallB:
                    text = stream.CutAt(stream.Position - 2) + Convert.ToChar(Utils.BS);
                    stream.CutAt(0);
                    return text;

                // Formfeed
                case Utils.SmallF:
                    text = stream.CutAt(stream.Position - 2) + Convert.ToChar(Utils.FF);
                    stream.CutAt(0);
                    return text;

                // Carriage return
                case Utils.SmallR:
                    text = stream.CutAt(stream.Position - 2) + Convert.ToChar(Utils.CR);
                    stream.CutAt(0);
                    return text;

                // Tab
                case Utils.SmallT:
                    text = stream.CutAt(stream.Position - 2) + Convert.ToChar(Utils.Tab);
                    stream.CutAt(0);
                    return text;

                // Vertical tab
                case Utils.SmallV:
                    text = stream.CutAt(stream.Position - 2) + Convert.ToChar(Utils.VTab);
                    stream.CutAt(0);
                    return text;

                //// HexEscapeSequence
                case Utils.SmallX:
                    for (var i = 1; i <= 2; i++)
                    {
                        code = stream.Read(this);
                        if (!this.IsHexDigit(code))
                        {
                            throw this.Error("Invalid Hex Escape Sequence");
                        }
                    }

                    // TODO: Test this
                    return stream.CutAt(stream.Position);

                //// UnicodeEscapeSequence
                case Utils.SmallU:
                    for (int i = 1; i <= 4; i++)
                    {
                        code = stream.Read(this);
                        if (!this.IsHexDigit(code))
                        {
                            throw this.Error("Invalid Unicode Escape Sequence");
                        }
                    }

                    // TODO: Test this
                    return stream.CutAt(stream.Position);

                case Utils.ZeroDigit:
                    code = stream.Read(this);
                    if (code >= Utils.ZeroDigit && code <= Utils.NineDigit)
                    {
                        throw this.Error("Invalid Zero Escape Sequence");
                    }

                    // TODO: Test this
                    return stream.CutAt(stream.Position);

                //// NonEscapeCharacter
                default:
                    // TODO: Why not?
                    throw this.Error("Invalid Escape Character");
                    ////if (this.IsLineTerminator(code) || (code > Utils.ZeroDigit && code <= Utils.NineDigit))
                    ////{
                    ////    return NodeType.Error;
                    ////}

                    ////return NodeType.CharEscape;
            }
        }

        private bool IsHexDigit(int code)
        {
            throw new NotImplementedException();
        }
    }
}
