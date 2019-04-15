using System;

namespace ContactList.Common.Contexts
{
    public interface IDatabaseFactory : IDisposable
    {
        Context GetContext<Context>(String connectionString = null) where Context : BaseContext;
    }
}
