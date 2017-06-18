namespace JSONMapper
{
    public class JStream
    {
        private readonly string data;
        private int index;

        public JStream(string data)
        {
            this.data = data;
            this.index = 0;
        }

        public char Char
        {
            get
            {
                return data[index];
            }
        }
        
        public void NextTimes(int nTimes)
        {
            index += nTimes;
        }

        public void Next()
        {
            ++index;
        }
        
        public void SkipSpaces()
        {
            while (data[index] == ' ') ++index;
        }

        public string GetSubString(int count)
        {
            return data.Substring(index, count);
        }
    }
}
