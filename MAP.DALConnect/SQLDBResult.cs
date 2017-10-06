using System.Collections;
using System.Data;

namespace MAP.Inventory.DAL
{
    public class SQLDBResult
    {
        public DataSet Contents = new DataSet();
        public Hashtable Parameters = new Hashtable();
        public object Return;
    }
}
