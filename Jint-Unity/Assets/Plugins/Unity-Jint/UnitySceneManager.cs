using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JintUnity
{
    /// <summary>
    /// Responsible for running UQL queries.
    /// </summary>
    public class UnitySceneManager
    {
        /// <summary>
        /// Specifies whether we're running a recursive or flat search.
        /// </summary>
        private enum SearchMode
        {
            Flat,
            Recursive
        }

        /// <summary>
        /// Basic query method to find collections of objects.
        /// </summary>
        /// <param name="query">A query is made up of DisplayObject names and
        /// searches over either immediate children or descendants at arbitrary
        /// depth. A search over immediate children is given by "." while a
        /// recursive search is given by "..".</param>
        /// <param name="parent">The object to start the search from.</param>
        /// <returns></returns>
        public IEnumerable<Transform> Query(string query, Transform parent)
        {
            // since Unity doesn't have a proper object graph, there's no way
            // to traverse the hierarchy without a starting point
            if (null == parent)
            {
                return Enumerable.Empty<Transform>();
            }

            if (string.IsNullOrEmpty(query))
            {
                return Enumerable.Empty<Transform>();
            }
            
            // grab current context
            var current = new List<Transform>
            {
                parent
            };

            // define vars here
            int i, j, k, iLength, jLength, kLength;
            UnitySceneQuery sceneQuery;
            var results = new List<Transform>();

            // split at recursive queries
            var recur = false;
            var recursiveQueries = query.Split("..".ToCharArray());
            for (i = 0, iLength = recursiveQueries.Length; i < iLength; i++)
            {
                var recursiveQuery = recursiveQueries[i];

                // split into shallow queries
                var shallowQueries = recursiveQuery.Split('.').ToList();

                // ? I don't understand why split works this way.
                if ("" == shallowQueries[0])
                {
                    shallowQueries.RemoveAt(0);
                }

                // recursive query
                if (recur)
                {
                    var recursiveQueryString = shallowQueries[0];
                    shallowQueries.RemoveAt(0);

                    // create query
                    sceneQuery = new UnitySceneQuery(recursiveQueryString);

                    // execute query on each of the current nodes
                    results.Clear();
                    for (k = 0, kLength = current.Count; k < kLength; k++)
                    {
                        ExecuteQuery(
                            SearchMode.Recursive,
                            current[k],
                            sceneQuery,
                            results);
                    }

                    if (0 != results.Count)
                    {
                        current = results;
                    }
                    else
                    {
                        return results;
                    }
                }

                // perform shallow searches
                for (j = 0, jLength = shallowQueries.Count; j < jLength; j++)
                {
                    var shallowQueryString = shallowQueries[j];

                    // create query
                    sceneQuery = new UnitySceneQuery(shallowQueryString);

                    // execute query on each of the current nodes
                    results.Clear();
                    for (k = 0, kLength = current.Count; k < kLength; k++)
                    {
                        ExecuteQuery(
                            SearchMode.Flat,
                            current[k],
                            sceneQuery,
                            results);
                    }

                    if (0 != results.Count)
                    {
                        current = results;
                    }
                    else
                    {
                        return results;
                    }
                }

                recur = 0 == recursiveQuery.Length || 0 == i % 2;
            }

            return current;
        }

        /// <summary>
        /// Executes a query on childen.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="node"></param>
        /// <param name="query"></param>
        /// <param name="results"></param>
        private void ExecuteQuery(
            SearchMode mode,
            Transform node,
            UnitySceneQuery query,
            List<Transform> results)
        {
            for (int i = 0, len = node.childCount; i < len; i++)
            {
                var newChild = node.GetChild(i);
                if (query.Execute(newChild))
                {
                    results.Add(newChild);
                }

                if (SearchMode.Recursive == mode)
                {
                    ExecuteQuery(
                        SearchMode.Recursive,
                        newChild,
                        query,
                        results);
                }
            }
        }
    }
}