using System;
using System.Runtime.Serialization;

namespace ToDoList
{
    [DataContract]
    class KBItem : IComparable
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int Status { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Id, this.Description);
        }

        public int CompareTo(object obj)
        {
            KBItem item2 = obj as KBItem;
            if (this.Id > item2.Id)
                return 1;
            else if (this.Id < item2.Id)
                return -1;
            else
                return 0;
        }
    }
}
