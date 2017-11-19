using MAP.Inventory.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.ModelImple
{
    public class CommonImple
    {
        General _General = new General();

        public DataSet GetNextPrevDocData(int DocType)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = _General.Get(new ArrayList() { DocType }, "sp_GetNextPrevDocData", 0);
            }
            catch (Exception ex)
            {
             
            }

            return ds;
        }


    }
}
