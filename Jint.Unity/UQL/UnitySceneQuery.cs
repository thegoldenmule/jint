using System.Text.RegularExpressions;
using UnityEngine;

namespace Jint.Unity
{
    /// <summary>
    /// Pulls apart query details and runs a query on a Unity object.
    /// </summary>
    public class UnitySceneQuery
    {
        /// <summary>
        /// Regexes to tease apart the details of a query.
        /// </summary>
        private readonly Regex NAME_QUERY_REGEX = new Regex(@"([\w]+)((\[(\d)?:(\d)?\])|$)");
        private readonly Regex PROPERTY_QUERY_REGEX = new Regex(@"\((@|(@@))([\w]+)\s*(([<>]=?)|==)\s*([\w]+)\)((\[(\d)?:(\d)?\])|$)");

        private readonly string _query;
        private readonly string _propName;
        private readonly string _op;
        private readonly string _propValue;
        private readonly bool _isCollection;
        private readonly int _startIndex;
        private readonly int _endIndex;

        /// <summary>
        /// True iff the query string was valid.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Constructs a new query object from a string.
        /// </summary>
        /// <param name="query"></param>
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
                _propName = "name";
                _op = "==";
                _propValue = match.Groups[1].Value;
                _isCollection = "" != match.Groups[2].Value;
                if (!int.TryParse(match.Groups[4].Value, out _startIndex))
                {
                    IsValid = false;
                    return;
                }

                if (!int.TryParse(match.Groups[5].Value, out _endIndex))
                {
                    IsValid = false;
                    return;
                }

                IsValid = true;

                return;
            }
            
            match = PROPERTY_QUERY_REGEX.Match(_query);
            if (match.Success) {
                _propName = match.Groups[3].Value;
                _op = match.Groups[4].Value;
                _propValue = match.Groups[6].Value;
                if (!int.TryParse(match.Groups[9].Value, out _startIndex))
                {
                    IsValid = false;
                    return;
                }

                if (!int.TryParse(match.Groups[10].Value, out _endIndex))
                {
                    IsValid = false;
                    return;
                }

                IsValid = true;
            }
        }

        /// <summary>
        /// Executes a query on a specific context.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Execute(Transform context)
        {
            if (!IsValid)
            {
                return false;
            }

            // TODO: subclasses!
            if (_propName == "name")
            {
                return context.name == _propValue;
            }

            return false;
        }
    }
}