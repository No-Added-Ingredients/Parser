namespace No.Added.Parser.Code
{
    using System;
    using Nodes;
    using Readers;

    public class Script : TokenCode
    {
        private ScriptReader reader;

        private TokenStack offset = new TokenStack(null, Utils.Space);

        public Script(string code) 
        {
            this.Text = string.Empty;
            this.reader = new ScriptString(code); 
        }

        public Script(ScriptReader code)
        {
            this.Text = string.Empty;
            this.reader = code;
        }

        public override T ParseExpression<T>(DefaultParser parser, Func<TokenCode, TokenCode, T> tokenOf)
        {
            var leftScript = this.Next;
            var context = leftScript;
            context.Next = this;
            var stream = this.reader;
            stream.Reset();
            var rightSide = new Script(stream);
            rightSide.offset = this.offset;
            var settled = rightSide.offset.Set == null; // Whether or not sets are closed.
            var nextScript = new RawScript(rightSide);

            // Function to check if token is found.
            Func<TokenCode, T> tryToken = (rightScript) =>
            {
                if (settled)
                {
                    // rightScript is in current set. May check for token.
                    context.Next = Utils.CodeSplit;
                    var result = tokenOf(leftScript, rightScript);
                    if (result != null)
                    {
                        return result;
                    }
                }

                context.Next = rightScript;
                context = rightScript;
                return null;
            };

            for (var code = stream.Read(parser); code != -1; code = stream.Read(parser))
            {
                var set = parser.TypeSetOpenOf(code);
                if (set != NodeType.Unknown)
                {
                    settled = rightSide.offset.Set == null; // Settled for left side of next SetStart.
                    rightSide.offset = new TokenStack(rightSide.offset, code);
                    if (stream.Position > 1)
                    {
                        var rightScript = new TokenCode() { Text = stream.SplitAt(stream.Position - 1) };
                        rightSide.offset.Set = new SetStart() { Text = stream.SplitAt(1), Next = nextScript, Type = set };
                        rightScript.Next = rightSide.offset.Set;
                        var found = tryToken(rightScript);

                        if (found != null)
                        {
                            return found;
                        }

                        settled = false;
                        context = rightScript.Next;
                        continue;
                    }

                    settled = false;
                    rightSide.offset.Set = new SetStart() { Text = stream.SplitAt(stream.Position), Next = nextScript, Type = set };
                    context.Next = rightSide.offset.Set;
                    context = context.Next;
                    context.Next = nextScript;
                    continue;
                }

                if (parser.IsEndGroup(code, rightSide.offset.Token))
                {
                    var setStart = rightSide.offset.Set;
                    if (setStart == null)
                    {
                        throw parser.Error(string.Format("Unexpected {0}.", Convert.ToChar(code)));
                    }
                    
                    rightSide.offset = rightSide.offset.Previous;
                    settled = rightSide.offset.Set == null;
                    if (stream.Position > 1)
                    {
                        // TODO: Check if this can happen?
                        Console.WriteLine(" It did happen!");
                        context.Next = new TokenCode() { Text = stream.SplitAt(stream.Position - 1) };
                        context = context.Next;
                        setStart.End = new SetEnd() { Text = stream.SplitAt(1) };
                        context.Next = setStart.End;
                        continue;
                    }
                    
                    setStart.End = new SetEnd() { Text = stream.SplitAt(1) };
                    context.Next = setStart.End;
                    context = context.Next;
                    context.Next = nextScript;
                    continue;
                }

                if (parser.IsStartText(code))
                {
                    if (stream.Position > 1)
                    {
                        var found = tryToken(new TokenCode() { Text = stream.SplitAt(stream.Position - 1), Next = nextScript });

                        if (found != null)
                        {
                            return found;
                        }

                        stream.Read(parser);
                    }

                    var rightScript = parser.ParseTextCode(code, stream);
                    rightScript.Next = nextScript;
                    context.Next = rightScript;
                    context = rightScript;
                    continue;
                }

                if (parser.IsStartNumber(code, stream))
                {
                    if (stream.Position > 1)
                    {
                        var found = tryToken(new TokenCode() { Text = stream.SplitAt(stream.Position - 1), Next = nextScript });

                        if (found != null)
                        {
                            return found;
                        }

                        stream.Read(parser);
                    }

                    var rightScript = parser.ParseNumberCode(stream, code);
                    rightScript.Next = nextScript;
                    context.Next = rightScript;
                    context = rightScript;
                    continue;
                }

                if (parser.IsStartUnary(code, stream))
                {
                    if (stream.Position > 1)
                    {
                        var found = tryToken(new TokenCode() { Text = stream.SplitAt(stream.Position - 1), Next = nextScript });

                        if (found != null)
                        {
                            return found;
                        }

                        stream.Read(parser);
                    }

                    var rightScript = parser.ParseUnaryCode(stream);
                    rightScript.Next = nextScript;
                    context.Next = rightScript;
                    context = rightScript;
                    continue;
                }

                if (parser.IsStartIndentifier(code))
                {
                    if (stream.Position > 1)
                    {
                        var found = tryToken(new TokenCode() { Text = stream.SplitAt(stream.Position - 1), Next = nextScript });

                        if (found != null)
                        {
                            return found;
                        }

                        stream.Read(parser);
                    }

                    var rightScript = parser.ParseIndentifierCode(stream);
                    rightScript.Next = nextScript;
                    context.Next = rightScript;
                    context = rightScript;
                    continue;
                }

                if (parser.IsNameSeparator(code))
                {
                    if (stream.Position > 1)
                    {
                        var found = tryToken(new TokenCode() { Text = stream.SplitAt(stream.Position - 1), Next = nextScript });

                        if (found != null)
                        {
                            return found;
                        }

                        stream.Read(parser);
                    }

                    var rightScript = parser.ParseNameSeparatorCode(stream);
                    rightScript.Next = nextScript;
                    context.Next = rightScript;
                    context = rightScript;
                    continue;
                }

                if (parser.IsWhitespace(code))
                {
                    if (stream.Position > 1)
                    {
                        var found = tryToken(new TokenCode() { Text = stream.CutAt(stream.Position - 1), Next = nextScript });

                        if (found != null)
                        {
                            return found;
                        }

                        continue;
                    }

                    stream.TrimLeft(parser);
                    continue;
                }
            }

            if (rightSide.offset.Set != null)
            {
                throw parser.Error("Set closure expected.");
            }

            context.Next = Utils.EndOfCode;
            ////if (leftScript is SetStart)
            ////{
            ////    // First Next is referenced RawScript second Next is approached.
            ////    this.Next.Next = leftScript.Next; // Skip SetStart to get into set.
            ////    return this.Next.ParseExpression(parser, tokenOf); // Go into set to try to find token.
            ////}

            return null;
        }

        internal override T ExpressionToken<T>(DefaultParser parser, TokenCode startLeft, TokenCode endLeft, Func<TokenCode, TokenCode, T> tokenOf)
        {
            throw new NotSupportedException();
        }
    }
}
