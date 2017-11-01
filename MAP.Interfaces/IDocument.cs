using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Interface
{
    public interface IDocument
    {
        int DocID { get; set; }
        string DocName { get; set; }
        string DocDate { get; set; }

        //Methods..
        void init();//gets the latest document number
        void EditDocument(int DocID);
        DataSet GetGirdData(int DocID);
        string SaveDocument(out long ret);
        long DeleteDocument(int DocID);

    }
}
