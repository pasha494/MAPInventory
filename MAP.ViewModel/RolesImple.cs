using MAP.Inventory.Common.Controls;
using MAP.Inventory.DAL;
using MAP.Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.ModelImple
{
    public class RolesImple
    {
        LookUps _LookUps = new LookUps();
        General _General = new General();
         
        public RolesImple()
        {

        }

        public MapTree[] GetRolesFeatureActionData()
        {
            MapTree _roleTree = null;
            DataTable dt = getRolesFeatureActionData();
            _roleTree = fillRoleTree(null, dt);
            return new MapTree[] { _roleTree };
        }

        public List<RolesModel> GetRolesData()
        {
            List<RolesModel> RolesList = new List<RolesModel>();

            DataTable dt = getRolesData(0);// fetch all the rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RolesList.Add(new RolesModel()
                {
                    RoleId = Convert.ToInt32(dt.Rows[i]["NodeNo"].ToString()),
                    RoleName = dt.Rows[i]["Name"].ToString(),
                    IsSystemRole = Convert.ToBoolean(dt.Rows[i]["SystemRole"])
                });
            }

            return RolesList;
        }
         
        public  RolesModel  GetRolesData(int RoleId)
        {
            RolesModel _roleModel = null; 

            DataTable dt = getRolesData(RoleId); 

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                _roleModel = new RolesModel()
                {
                    RoleId = Convert.ToInt32(dt.Rows[i]["NodeNo"].ToString()),
                    RoleName = dt.Rows[i]["Name"].ToString(),
                    RoleData = dt.Rows[i]["Data"].ToString(),
                    IsSystemRole = Convert.ToBoolean(dt.Rows[i]["SystemRole"])
                };
            }

            return _roleModel;
        }

        public long SaveRoleData(int RoleId, string RoleName, string RoleData)
        {
            long flg = 0;
            try
            {
                ArrayList al = new ArrayList(); 
                al.Add(RoleId);
                al.Add(RoleName);
                al.Add(RoleData);
                al.Add(Convert.ToInt32(_LookUps.GetSessionObject("UserID")));

                _General.Set(al, "sp_InsertUpdateRoles", out flg); 

            }
            catch (Exception ex)
            {
                
            } 
            return flg; 
        }

        public long DeleteRole(int RoleId)
        {
            long flg = 0;
            try
            {
                if (RoleId > 0)
                {
                    ArrayList al = new ArrayList();
                    al.Add(RoleId);
                    _General.Set(al, "sp_DeleteRole", out flg);
                }

            }
            catch (Exception ex)
            {
                RoleId = -494;
            }
            return flg;
        }



        private MapTree fillRoleTree(MapTree _tree, DataTable dt)
        {

            if (_tree == null)
            {
                _tree = new MapTree();
                DataView dv = dt.DefaultView;
                dv.RowFilter = " parentId=0";
                _tree.id = Convert.ToInt32(dv[0]["id"]);
                _tree.field = dv[0]["field"].ToString();
                _tree.text = dv[0]["text"].ToString();
                _tree.indeterminate = Convert.ToBoolean(dv[0]["indeterminate"].ToString());
                _tree.@checked = Convert.ToBoolean(dv[0]["checked"].ToString());
                // _tree.value = true;
            }

            if (_tree != null && dt != null)
            {
                string rowFilter = " parentId=" + _tree.id;
                DataRow[] rows = dt.Select(rowFilter);

                if (rows != null)
                {
                    for (int i = 0; i < rows.Length; i++)
                    {
                        MapTree _childNode = new MapTree();
                        _childNode.id = Convert.ToInt32(rows[i]["id"]);
                        _childNode.field = rows[i]["field"].ToString();
                        _childNode.text = rows[i]["text"].ToString();
                        _childNode.indeterminate = Convert.ToBoolean(rows[i]["indeterminate"].ToString());
                        _childNode.@checked = Convert.ToBoolean(rows[i]["checked"].ToString());
                        _childNode.text = rows[i]["text"].ToString();
                        _tree.children.Add(_childNode);
                        fillRoleTree(_childNode, dt);
                    }
                }
            }

            return _tree;
        }

        private DataTable getRolesData(int RoleId)
        {
            DataTable dt = new DataTable();

            try
            {
                DataSet ds = new DataSet();

                ds = _General.Get(new ArrayList() { RoleId }, "sp_GetRoles", 0);
                dt = ds.Tables[0];

            }
            catch (Exception ex)
            {

            }

            return dt;
        }
         
        private DataTable getRolesFeatureActionData()
        {
            DataTable dt = new DataTable();

            try
            {
                DataSet ds = new DataSet();

                ds = _General.Get(new ArrayList(), "sp_GetRoleFeatureActionData", 0);
                dt = ds.Tables[0];

            }
            catch (Exception ex)
            {

            }

            return dt;
        }

    }
}
