using app.common.Exctensions;
using app.business.Abstract;
using Microsoft.Extensions.Logging;

namespace app.business.Services
{
    public class FuzzyService : IFuzzyService
    {
        private readonly ILogger<FuzzyService> _logger;
        public FuzzyService(ILogger<FuzzyService> logger)
        {
            _logger = logger;
        }
        public int Run(string input, string comparedTo, bool caseSensitive = false)
        {
            throw new NotImplementedException();
        }

        public string Run(string input, IEnumerable<string> comparedTo, bool caseSensitive = false)
        {
            this.VerifyingIncomeParams(input, comparedTo);

            if (caseSensitive)
            {
                input = input.ToLower();
                comparedTo = comparedTo.Select(s => s.ToLower());
            }

            Dictionary<string, int> wordsWithLevenshteinDistance = new Dictionary<string, int>();
            foreach (string word in comparedTo)
            {
                wordsWithLevenshteinDistance.Add(word, input.LevenshteinDistance(word));
            }
            KeyValuePair<string, int> result = wordsWithLevenshteinDistance.OrderBy(x => x.Value).MinBy(x => x.Value);
            return result.Key;
        }

        public Dictionary<string, int> Run(string input, IEnumerable<string> comparedTo)
        {
            this.VerifyingIncomeParams(input, comparedTo);

            Dictionary<string, int> wordsWithLevenshteinDistance = new Dictionary<string, int>();
            foreach (string word in comparedTo)
            {
                wordsWithLevenshteinDistance.Add(word, input.LevenshteinDistance(word));
            }

            return wordsWithLevenshteinDistance;
        }

        private void VerifyingIncomeParams(string text, IEnumerable<string> words)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text) ||
               words.Count() == 0)
            {
                _logger.LogError("Incorrect income params");
                throw new ArgumentNullException("Incorrect income params");
            }

            var duplicates = words.GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();
            if (duplicates.Count > 0)
            {
                _logger.LogError($"The {nameof(words)} collection has duplicate values: {string.Join(',', duplicates)}");
                throw new ArgumentException($"The {nameof(words)} collection has duplicate values: {string.Join(',', duplicates)}");
            }
        }
    }
}