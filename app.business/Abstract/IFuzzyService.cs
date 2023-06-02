namespace app.business.Abstract
{
    public interface IFuzzyService
    {
        public int Run(string input, string comparedTo, bool caseSensitive = false);

        public string Run(string input, IEnumerable<string> comparedTo, bool caseSensitive = false);

        public Dictionary<string, int> Run(string input, IEnumerable<string> comparedTo);
    }
}