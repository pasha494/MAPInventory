using DALConnect;
using MAP.Logging;
using MAP.Models;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.ViewModel
{
    public class UsersImple
    {

        General _General = new General();
        public long SaveUsers(UsersModel User)
        {
            long flg = 0;
            try
            {
                ArrayList al = new ArrayList();

                al.Add(User.UserID);
                al.Add(User.Name);
                al.Add(User.Email);
                al.Add(User.Password);
                al.Add(User.IsAdmin);
                al.Add(User.RoleID);
                _General.Set(al, "sp_InsertUpdateUser", out flg);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method >SaveUsers()", ex);
            }

            return flg;
        }

        public UsersModel EditUser(int ID)
        {
            UsersModel objUsers = new UsersModel();

            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    objUsers.UserID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    objUsers.Name = dt.Rows[0]["username"].ToString();
                    objUsers.Email = dt.Rows[0]["email"].ToString();
                    objUsers.Password = dt.Rows[0]["Password"].ToString();
                    objUsers.IsAdmin = Convert.ToBoolean(dt.Rows[0]["IsAdmin"]);
                    if (dt.Rows[0]["RoleID"] != DBNull.Value)
                        objUsers.RoleID = Convert.ToInt32(dt.Rows[0]["RoleID"].ToString());
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method > EditUser(int ID)", ex);
            }

            return objUsers;
        }

        public DataTable GetGridData(int UserID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = _General.Get(new ArrayList() { UserID }, "sp_GetUsersData");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method > GetGridData(int UserID)", ex);
            }

            return dt;
        }

        public long DeleteUser(int UserID)
        {
            long flg = 0;

            try
            {
                _General.Set(new ArrayList() { UserID }, "sp_DeleteUser", out flg);

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method > DeleteUser(int UserID)", ex);
            }

            return flg;
        }

        public long ChangePassword(string OldPassword, string NewPassword, int UserID)
        {
            long flg = 0;
            try
            {
                if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
                {

                    ArrayList al = new ArrayList();

                    al.Add(OldPassword);
                    al.Add(NewPassword);
                    al.Add(UserID);
                    _General.Set(al, "Sp_ChangePassword", out flg);

                }
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method >ChangePassword(string OldPassword, string NewPassword)", ex);
            }
            return flg;
        }
    }
}
