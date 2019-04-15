using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ContactList.Common.Exceptions;

namespace ContactList.Common.Contexts
{
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        [ThreadStatic]
        private static Dictionary<Type, BaseContext> _contexts;

        private static object _lock = new object();

        public static void CleanContexts()
        {
            _contexts?.Select(i => i.Value).ToList().ForEach(i => i?.Dispose());

            _contexts = new Dictionary<Type, BaseContext>();
        }

        public static List<BaseContext> GetContexts()
        {
            List<BaseContext> contexts = new List<BaseContext>();

            if (_contexts == null) return contexts;

            contexts.AddRange(_contexts.Values);

            return contexts;
        }

        public static void AddContext(Type type, BaseContext context)
        {
            if (_contexts == null) _contexts = new Dictionary<Type, BaseContext>();

            _contexts.Add(type, context);
        }

        public static void CleanAllDbContexts ()
        {
            var ctxs = GetContexts();

            ctxs.ForEach(i => i.Dispose());

            CleanContexts();
        }

        public TContext GetContext<TContext>(string connectionString = null) where TContext : BaseContext
        {
            BaseContext context;

            var type = typeof(TContext);

            lock (_lock)
            {
                if (_contexts == null) _contexts = new Dictionary<Type, BaseContext>();

                if (!_contexts.TryGetValue(type, out context))
                {
                    context = CreateContext(type, connectionString);

                    _contexts.Add(type, context);
                }

                if (context.Database.Connection.State != System.Data.ConnectionState.Open)
                    context.Database.Connection.Open();
            }
            return (TContext)context;
        }

        private BaseContext CreateContext(Type type, string connectionString)
        {
            try
            {
                ConstructorInfo constructorInfo = type.GetConstructor(new[] { typeof(string) });

                BaseContext context = (BaseContext)constructorInfo.Invoke(new object[] { connectionString });
                context.Database.UseTransaction(null);

                return context;
            }
            catch (Exception e)
            {
                throw new ConnectionStringException(e.InnerException?.Message);
            }        
        }

        public override void Dispose()
        {
            if (_contexts != null)
            {
                foreach (var item in _contexts)
                {
                    item.Value?.Dispose();
                }

                _contexts.Clear();
            }

            base.Dispose();
        }

        public void Begin()
        {

        }

        public void End(Exception ex = null)
        {
            Dispose();
        }
    }
}
