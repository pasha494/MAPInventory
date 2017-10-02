using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace Flortex.Inventory.Models
{
    public class Users
    {

        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string efDate { get; set; }
        public int RoleID { get; set; }
        public Users()
        {
            UserID = 0;
        }

        public int SaveUsers()
        {
            int flg = 0;
            try
            {
                List<string> Params = new List<string>();
                ArrayList al = new ArrayList();

                Params.Add("@NodeNo");
                Params.Add("@Name");
                Params.Add("@Email");
                Params.Add("@Password");
                Params.Add("@IsAdmin");
                Params.Add("@RoleID");

                al.Add(UserID);
                al.Add(Name);
                al.Add(Email);
                al.Add(Password);
                al.Add(IsAdmin);
                al.Add(RoleID);

                flg = DAL.ExecuteSP("sp_InsertUpdateUser", Params, al);
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method >SaveUsers()", ex);
            }

            return flg;
        }

        public void EditUser(int ID)
        {

            try
            {
                DataTable dt = GetGridData(ID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.UserID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
                    this.Name = dt.Rows[0]["username"].ToString();
                    this.Email = dt.Rows[0]["email"].ToString();
                    this.Password = dt.Rows[0]["Password"].ToString();
                    this.IsAdmin = Convert.ToBoolean(dt.Rows[0]["IsAdmin"]);
                    if (dt.Rows[0]["RoleID"] != DBNull.Value)
                        this.RoleID = Convert.ToInt32(dt.Rows[0]["RoleID"].ToString());
                }

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method > EditUser(int ID)", ex);
            }
        }

        public DataTable GetGridData(int UserID)
        {
            DataTable dt = null;

            try
            {
                DataSet ds = new DataSet();
                ds = DAL.GetDataSet("sp_GetUsersData", new List<string>() { "@UserID" }, new ArrayList() { UserID });
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method > GetGridData(int UserID)", ex);
            }

            return dt;
        }

        public int DeleteUser(int UserID)
        {
            int flg = 0;

            try
            {

                flg = DAL.ExecuteSP("sp_DeleteUser", new List<string>() { "@NodeNo" }, new ArrayList() { UserID });

            }
            catch (Exception ex)
            {
                PLog.Error("Error::Class > Users, Method > DeleteUser(int UserID)", ex);
            }

            return flg;
        }

        public int ChangePassword(string OldPassword, string NewPassword)
        {
            int flg = 0;
            try
            {
                if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrEmpty(NewPassword))
                {
                    List<string> Params = new List<string>();
                    ArrayList al = new ArrayList();

                    Params.Add("@OldPassword");
                    Params.Add("@NewPassword");
                    Params.Add("@UserID");

                    al.Add(OldPassword);
                    al.Add(NewPassword);
                    al.Add(Convert.ToInt32(LookUps.GetSessionObject("UserID")));
                    flg = DAL.ExecuteSP("Sp_ChangePassword", Params, al);


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
