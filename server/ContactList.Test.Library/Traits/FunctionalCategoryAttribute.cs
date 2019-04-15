using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ContactList.Test.Library.Traits
{
    public class FunctionalCategoryAttribute : TestCategoryBaseAttribute
    {
        private string _group;
        private string _subGroup;

        public FunctionalCategoryAttribute(string group, string subGroup)
        {
            _group = group;
            _subGroup = subGroup;
        }

        public override IList<string> TestCategories
        {
            get
            {
                return new List<string>() { $"FunctionalTests-{ _group }-{_subGroup}" };
            }
        }
    }
}
