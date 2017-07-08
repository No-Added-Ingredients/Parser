namespace No.Added.Parser.Readers
{
    using Nodes;

    public abstract class ScriptReader
    {
        public abstract int Position
        {
            get;

            set;
        }

        public abstract string Text
        {
            get;

            protected set;
        }

        public bool IsEmpty
        {
            get;

            protected set;
        }

        public abstract int Read(DefaultParser parser);

        public abstract int Peek();

        public abstract string CutAt(int index);

        public abstract string SplitAt(int index);

        public abstract void Reset();

        public abstract void TrimLeft(DefaultParser parser);
    }
}
