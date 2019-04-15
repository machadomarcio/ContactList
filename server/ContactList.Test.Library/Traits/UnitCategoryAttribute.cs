using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ContactList.Test.Library.Traits
{
    public class UnitCategoryAttribute : TestCategoryBaseAttribute
    {
        private string _group;
        private string _subGroup;

        public UnitCategoryAttribute(string group, string subGroup)
        {
            _group = group;
            _subGroup = subGroup;
        }

        public override IList<string> TestCategories
        {
            get
            {
                return new List<string>() { $"UnitTests-{ _group }-{ _subGroup }" };
            }
        }
    }
}
