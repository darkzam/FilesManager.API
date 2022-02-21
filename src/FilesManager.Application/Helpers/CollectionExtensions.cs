using System;
using System.Collections.Generic;
using System.Linq;

namespace FilesManager.Application.Helpers
{
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            var random = new Random();
            var row = random.Next(0, collection.Count());

            return collection.ElementAt(row);
        }
    }
}
