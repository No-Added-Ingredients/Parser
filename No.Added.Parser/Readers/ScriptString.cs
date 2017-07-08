namespace No.Added.Parser.Readers
{
    using Nodes;

    public class ScriptString : ScriptReader
    {
        public ScriptString(string script)
        {
            // Avoid this.Text == null;
            this.Text = script + string.Empty;
            this.Position = 0;
        }

        public override int Position
        {
            get;

            set;
        }

        public override string Text
        {
            get;

            protected set;
        }

        public override string CutAt(int index)
        {
            var result = this.Text.Substring(0, index);
            if (index + 1 < this.Text.Length)
            {
                this.Text = this.Text.Substring(index + 1);
            }
            else
            {
                this.Text = string.Empty;
            }

            this.Position = 0;
            return result;
        }

        public override string SplitAt(int index)
        {
            var result = this.Text.Substring(0, index);
            if (index + 1 <= this.Text.Length)
            {
                this.Text = this.Text.Substring(index);
            }
            else
            {
                this.Text = string.Empty;
            }

            this.Position = 0; 
            return result;
        }

        public override void TrimLeft(DefaultParser parser)
        {
            this.Text = this.Text.TrimStart();
            this.Position = 0;
        }

        public override int Peek()
        {
            if (this.Position >= this.Text.Length)
            {
                return -1;
            }

            return this.Text[this.Position];
        }

        public override int Read(DefaultParser parser)
        {
            if (this.Position >= this.Text.Length)
            {
                this.Position = this.Text.Length + 1;
                return -1;
            }

            var code = this.Text[this.Position];
            this.Position++;
            return code;
        }

        public override void Reset()
        {
            this.Position = 0;
        }
    }
}
