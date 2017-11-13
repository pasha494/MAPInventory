using MAP.Inventory.DAL;
using MAP.Inventory.Logging;
using MAP.Inventory.Model;
using System;
using System.Collections;
using System.Data;
using System.Web;
using MAP.Inventory.Common.Controls;
using System.Collections.Generic;

namespace MAP.Inventory.ModelImple
{
    public class LoginImple
    {

        General _General = new General();

        public int CheckLogin(string UserEmail, string Password)
        {
            int flg = 0;

            DataSet ds = new DataSet();
            ds = _General.Get(new ArrayList() { UserEmail }, "sp_GetUserDetails");
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (Password == ds.Tables[0].Rows[0]["Password"].ToString())
                    {
                        flg = 1;
                        CreateUserSession(ds.Tables[0]);
                    }
                    else
                        flg = -2;// Wrong password..

                }
                else
                    flg = -1;//Invalid user id..  
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Login, Method > CheckLogin(string UserEmail, string Password)", ex);
            }

            return flg;
        }

        public void CreateUserSession(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                SessionManager objSession = new SessionManager();
                objSession.UserID = Convert.ToInt32(dt.Rows[0]["id"].ToString());
                objSession.UserName = dt.Rows[0]["USerName"].ToString();
                objSession.Email = dt.Rows[0]["Email"].ToString();
                objSession.IsAdmin = Convert.ToBoolean(dt.Rows[0]["IsAdmin"]);
                if (dt.Rows[0]["RoleID"] != DBNull.Value)
                    objSession.RoleID = Convert.ToInt32(dt.Rows[0]["RoleID"].ToString());
                else
                    objSession.RoleID = 0;

                if (dt.Rows[0]["RoleData"] != null && !string.IsNullOrEmpty(dt.Rows[0]["RoleData"].ToString()))
                {
                    objSession.RoleFeatures= AddRoleFeatureActions(objSession, dt.Rows[0]["RoleData"].ToString());
                }

                HttpContext.Current.Session.Add("SessionManager", objSession);
            }
        }


        Dictionary<string, bool> AddRoleFeatureActions(SessionManager objSession, string RoleData)
        {
            Dictionary<string, bool> roleFeatureActions = new Dictionary<string, bool>();
            try
            {
                MapTree[] tree = Newtonsoft.Json.JsonConvert.DeserializeObject<MapTree[]>(RoleData);

                if (tree != null && tree.Length > 0)
                {
                   
                    LoadRoleFeatureActions(tree[0], ref roleFeatureActions);

                }
            }
            catch (Exception ex)
            {

            }

            return roleFeatureActions;
        }


        void LoadRoleFeatureActions(MapTree tree, ref Dictionary<string, bool> FeatureActions)
        {
            if (!FeatureActions.ContainsKey(tree.field))
                FeatureActions.Add(tree.field, (tree.@checked || tree.indeterminate));

            if (tree.children != null && tree.children.Count > 0)
            {
                for (int i = 0; i < tree.children.Count; i++)
                {
                    LoadRoleFeatureActions(tree.children[i], ref FeatureActions);
                }
            }

        }
    }
}
