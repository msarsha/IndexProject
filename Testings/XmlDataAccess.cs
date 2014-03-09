using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DatabaseIndex.Entities;

namespace Testings
{
    public class XmlDataAccess<T>
    {
        private ICollection<T> _data;
        private string _path;

        public XmlDataAccess(string path)
        {
            _path = path;
            InitData();
        }

        private void InitData()
        {
            _data = new List<T>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            FileStream reader = new FileStream(_path, FileMode.Open);
            _data = (List<T>)serializer.Deserialize(reader);
        }

        public IEnumerable<T> GetData()
        {
            return _data;
        }

        public void Insert(T data)
        {
            try
            {
                _data.Add(data);
                IndexEvent.IndexEventHandler(this, new IndexEventArgs //firing up the event to update the index
                {
                    State = DataState.Add,
                    Row = data
                });
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Delete(T data)
        {
            try
            {
                _data.Remove(data);
                IndexEvent.IndexEventHandler(this, new IndexEventArgs //firing up the event to update the index
                {
                    State = DataState.Remove,
                    Row = data
                });
            }
            catch (Exception e)
            {
                
                throw new Exception(e.Message);
            }
        }
    }
}
