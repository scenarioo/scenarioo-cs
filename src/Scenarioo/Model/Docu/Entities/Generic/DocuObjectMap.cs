using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Scenarioo.Model.Docu.Entities.Generic
{
    /// <summary>
    /// Container for docu objects.
    /// </summary>
    /// <remarks>
    /// This "Map" is a list because of serialization reasons. There won't be many items
    /// in the list so the map-access will be enough fast with LINQ.
    /// </remarks>
    public class DocuObjectMap : IList<DocuObject>
    {
        private readonly IList<DocuObject> _docuObjects = new List<DocuObject>();

        public void Add(string groupTitle, params DocuObject[] docuObjects)
        {
            var docuObject = new DocuObject(labelKey: groupTitle);
            docuObject.Properties.AddRange(docuObjects);

            _docuObjects.Add(docuObject);
        }

        /// <summary>
        /// Shortcut to directly modify an object with the given labelKey.
        /// </summary>
        public DocuObject this[string key]
        {
            get
            {
                return _docuObjects.FirstOrDefault(o => o.LabelKey == key);
            }
        }

        #region IList<T>

        public IEnumerator<DocuObject> GetEnumerator()
        {
            return _docuObjects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(DocuObject item)
        {
            _docuObjects.Add(item);
        }

        public void Clear()
        {
            _docuObjects.Clear();
        }

        public bool Contains(DocuObject item)
        {
            return _docuObjects.Contains(item);
        }

        public void CopyTo(DocuObject[] array, int arrayIndex)
        {
            _docuObjects.CopyTo(array, arrayIndex);
        }

        public bool Remove(DocuObject item)
        {
            return _docuObjects.Remove(item);
        }

        public int Count
        {
            get { return _docuObjects.Count; }
        }

        public bool IsReadOnly
        {
            get { return _docuObjects.IsReadOnly; }
        }

        public int IndexOf(DocuObject item)
        {
            return _docuObjects.IndexOf(item);
        }

        public void Insert(int index, DocuObject item)
        {
            _docuObjects.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _docuObjects.RemoveAt(index);
        }

        public DocuObject this[int index]
        {
            get
            {
                return _docuObjects[index];
            }
            set
            {
                _docuObjects[index] = value;
            }
        }

        #endregion

    }
}