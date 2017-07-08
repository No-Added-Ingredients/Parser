namespace No.Added.Parser.Code
{
    public class TokenStack
    {
        public TokenStack(TokenStack previous, int token)
        {
            this.Previous = previous;
            this.Token = token;
        }

        public TokenStack Previous { get; set; }

        public int Token { get; set; }

        public SetStart Set { get; set; } = null;
    }
}
