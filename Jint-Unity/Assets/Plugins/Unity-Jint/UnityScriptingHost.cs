using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Jint;
using UnityEngine;

namespace JintUnity
{
    public class UnitySceneQuery
    {
        private readonly Regex NAME_QUERY_REGEX = new Regex(@"([\w]+)((\[(\d)?:(\d)?\])|$)");
        private readonly Regex PROPERTY_QUERY_REGEX = new Regex(@"\((@|(@@))([\w]+)\s*(([<>]=?)|==)\s*([\w]+)\)((\[(\d)?:(\d)?\])|$)");

        private readonly string _query;

        public string propName;
        public string op;
        public string propValue;
        public bool isCollection;
        public int startIndex;
        public int endIndex;

        public bool IsValid { get; private set; }

        public UnitySceneQuery(string query)
        {
            _query = query;

            IsValid = false;

            // Cases:
            // 1. name query
            // 2. property query

            var match = NAME_QUERY_REGEX.Match(_query);
            if (match.Success)
            {
                propName = "name";
                op = "==";
                propValue = match.Groups[1].Value;
                isCollection = "" != match.Groups[2].Value;
                if (!int.TryParse(match.Groups[4].Value, out startIndex))
                {
                    IsValid = false;
                    return;
                }

                if (!int.TryParse(match.Groups[5].Value, out endIndex))
                {
                    IsValid = false;
                    return;
                }

                IsValid = true;

                return;
            }
            
            match = PROPERTY_QUERY_REGEX.Match(_query);
            if (match.Success) {
                propName = match.Groups[3].Value;
                op = match.Groups[4].Value;
                propValue = match.Groups[6].Value;
                if (!int.TryParse(match.Groups[9].Value, out startIndex))
                {
                    IsValid = false;
                    return;
                }

                if (!int.TryParse(match.Groups[10].Value, out endIndex))
                {
                    IsValid = false;
                    return;
                }

                IsValid = true;
            }
        }

        public bool Execute(Transform context)
        {
            return false;
        }
    }

    public class UnitySceneManager
    {
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
                        ExecuteQueryRecursive(current[k], sceneQuery, results);
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
                        ExecuteQuery(current[k], sceneQuery, results);
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

        private void ExecuteQueryRecursive(Transform node, UnitySceneQuery query, List<Transform> results)
        {
            for (int i = 0, len = node.childCount; i < len; i++)
            {
                var newChild = node.GetChild(i);
                if (query.Execute(newChild))
                {
                    results.Add(newChild);
                }

                ExecuteQueryRecursive(newChild, query, results);
            }
        }

        private void ExecuteQuery(Transform node, UnitySceneQuery query, List<Transform> results)
        {
            for (int i = 0, len = node.childCount; i < len; i++) {
                var newChild = node.GetChild(i);
                if (query.Execute(newChild))
                {
                    results.Add(newChild);
                }
            }
        }
    }

    /// <summary>
    /// Hosts scripts and provides a default Unity API.
    /// </summary>
    public class UnityScriptingHost : Engine
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public UnityScriptingHost()
        {
            SetValue("Log", new UnityLogWrapper());
        }
    }
}