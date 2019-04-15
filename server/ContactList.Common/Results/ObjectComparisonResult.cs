using System.Collections.Generic;

namespace ContactList.Common.Results
{
    public class ObjectComparisonResult
    {
        public bool AreEqual { get; set; }

        public IList<ObjectComparisonDiffernce> Differences { get; set; }
    }

    public class ObjectComparisonDiffernce
    {
        public string PropertyName { get; set; }

        public string Object1Value { get; set; }

        public string Object2Value { get; set; }
    }
}