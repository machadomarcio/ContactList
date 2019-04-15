using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ContactList.Test.Library.Traits
{
    public class IntegrationCategoryAttribute : TestCategoryBaseAttribute
    {
        private string _group;
        private string _subGroup;

        public IntegrationCategoryAttribute(string group, string subGroup)
        {
            _group = group;
            _subGroup = subGroup;
        }

        public override IList<string> TestCategories
        {
            get
            {
                return new List<string>() { $"IntegrationTests-{ _group }-{ _subGroup }" };
            }
        }
    }
}
