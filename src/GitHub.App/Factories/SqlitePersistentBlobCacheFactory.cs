﻿using Akavache;
using Akavache.Sqlite3;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace GitHub.Factories
{
    [Export(typeof(IBlobCacheFactory))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SqlitePersistentBlobCacheFactory : IBlobCacheFactory
    {
        static readonly Logger log = LogManager.GetCurrentClassLogger();
        Dictionary<string, IBlobCache> cache = new Dictionary<string, IBlobCache>();

        public IBlobCache CreateBlobCache(string path)
        {
            Guard.ArgumentNotEmptyString(path, nameof(path));
            if (cache.ContainsKey(path))
                return cache[path];

            try
            {
                var c = new SQLitePersistentBlobCache(path);
                cache.Add(path, c);
                return c;
            }
            catch(Exception ex)
            {
                log.Error("Error while creating SQLitePersistentBlobCache for {0}. {1}", path, ex);
                return new InMemoryBlobCache();
            }
        }
    }
}