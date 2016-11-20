using System;
using System.Diagnostics;

namespace Cuemon.Runtime.Caching
{
    internal class CacheAsyncState<TResult>
    {
        public CacheAsyncState(CacheCollection cache, string key, string group)
        {
            Cache = cache;
            Key = key;
            Group = group;
        }

        public CacheAsyncState<TResult> With<TTuple>(FuncFactory<TTuple, TResult> method)
            where TTuple : Template
        {
            EndInvoke = method.EndExecuteMethod;
            return this;
        }

        public Func<IAsyncResult, TResult> EndInvoke;

        public string Key { get; set; }

        public string Group { get; set; }

        public CacheCollection Cache { get; set; }

        public static void Callback(IAsyncResult asyncResult)
        {
            try
            {
                CacheAsyncState<TResult> asyncState = asyncResult.AsyncState as CacheAsyncState<TResult>;
                if (asyncState == null) { return; }
                TResult result = asyncState.EndInvoke(asyncResult);
                asyncState.Cache.Set(asyncState.Key, asyncState.Group, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}