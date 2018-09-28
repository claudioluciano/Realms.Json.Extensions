using Newtonsoft.Json;
using Realms;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CL.Realms.Json.Extensions
{
    public static class RealmJsonExtensions
    {
        /// <summary>
        /// Find a object by primary key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="realm"></param>
        /// <param name="type"></param>
        /// <param name="primaryKey"></param>
        /// <param name="isIntegerPK"></param>
        /// <returns>The object or null</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static RealmObject FindByPrimaryKey<TKey, T>(Realm realm, TKey primaryKey) where T : RealmObject
        {
            if (primaryKey is string)
                return realm.Find(typeof(T).Name, primaryKey as string);

            if (primaryKey is long || primaryKey is long?)
                return realm.Find(typeof(T).Name, primaryKey as long?);

            return null;
        }

        /// <summary>
        /// Create a Generic non-managed RealmObject Copy
        /// </summary>
        /// <returns>The non-managed copy.</returns>
        /// <param name="realmObject">Realm object.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NonManagedCopy<T>(this RealmObject realmObject) where T : RealmObject
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(realmObject));
        }

        /// <summary>
        /// Create a non-managed copy of Realm's Query Result.
        /// </summary>
        /// <param name="results">Realm collection</param>
        /// <typeparam name="T">Type of the <see cref="RealmObject"/> in the results.</typeparam>
        /// <returns>The collection of non-managed realm objects.</returns>
        public static IQueryable<T> NonManagedCopy<T>(this IQueryable<T> results) where T : RealmObject
        {
            var realmCollectionCopy = CopyRealmCollectionElements(results);
            return realmCollectionCopy.AsQueryable();
        }

        /// <summary>
        /// Create a non-managed copy of Realm's Query Result.
        /// </summary>
        /// <param name="list">Realm collection</param>
        /// <typeparam name="T">Type of the <see cref="RealmObject"/> in the results.</typeparam>
        /// <returns>The collection of non-managed realm objects.</returns>
        public static IList<T> NonManagedCopy<T>(this IList<T> list) where T : RealmObject
        {
            return CopyRealmCollectionElements(list);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IList<T> CopyRealmCollectionElements<T>(IEnumerable<T> collection) where T : RealmObject
        {
            var realmCollectionCopy = new List<T>();

            foreach (var element in collection)
            {
                var nonManagedElementCopy = NonManagedCopy<T>(element);
                realmCollectionCopy.Add(nonManagedElementCopy);
            }

            return realmCollectionCopy;
        }
    }
}
