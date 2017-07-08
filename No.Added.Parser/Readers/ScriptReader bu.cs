using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace No.Added.Parser.Readers
{
    public class ScriptReaderBU
    {
        private TextReader reader = null;
        private const int BUFFER_SIZE = 255;
        private char[] buffer = new char[BUFFER_SIZE];
        private int size = 0;
        private int index = 0;
        private List<char> script = new List<char>();

        public bool EOF
        {
            get;

            private set;
        }

        public int Position
        {
            get;

            set;
        }

        public int Read()
        {
            if (this.EOF)
            {
                return -1;
            }

            if (this.Position + 1 < this.script.Count)
            {
                this.Position++;
                return this.script[this.Position];
            }

            if (this.ReadBlock())
            {
                this.Position++;
                return this.buffer[0];
            }

            return -1;
        }

        private bool ReadBlock()
        {
            this.size = this.reader.ReadBlock(this.buffer, 0, BUFFER_SIZE);
            this.EOF = this.size == 0;
            if (this.EOF)
            {
                return false;
            }

            if (this.size == BUFFER_SIZE)
            {
                this.script.AddRange(buffer);
            }
            else
            {
                this.script.AddRange(new ArraySegment<char>(buffer, 0, this.size));
            }
            
            return true;
        }
    }
}
