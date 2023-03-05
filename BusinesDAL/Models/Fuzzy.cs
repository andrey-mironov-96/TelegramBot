using BusinesDAL.Models.Utils;
using WebParse.Models;

namespace BusinesDAL.Models
{
    public class Fuzzy
    {
        public int Weight { get; set; } = -1;

        public string Label { get; set; }

        public FuzzyType Type { get; set; }

        public AdmissionPlan AddmissionPlan { get; set; }
    }
}