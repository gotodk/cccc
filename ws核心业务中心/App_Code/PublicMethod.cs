using FMDBHelperClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// PublicMethod 的摘要说明
/// </summary>
[WebService(Namespace = "http://corebusiness.aftipc.com/", Description = "V1.00->xxx")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class PublicMethod : System.Web.Services.WebService
{

    public PublicMethod()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    /// <summary>
    /// 初始化一个DataSet用于结果的返回
    /// </summary>
    /// <returns></returns>
    private static DataSet InitialDataset()
    {
        DataSet ds = new DataSet();
        //要返回的数据集中的数据表
        DataTable dt = new DataTable();
        dt.TableName = "返回值单条";
        dt.Columns.Add("执行结果", typeof(string));
        dt.Columns.Add("提示文本", typeof(string));
        dt.Columns.Add("附件信息1", typeof(string));
        dt.Columns.Add("附件信息2", typeof(string));
        //dt.Rows.Add(new string[] { "err", "初始化" });
        ds.Tables.Add(dt);
        return ds;
    }

   
    

    [WebMethod(MessageName = "获取IP地址", Description = "获取登录方当前的IP")]
    public string GetIP()
    {
        string result = String.Empty;
        result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (null == result || result == String.Empty)
        { result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; }
        if (null == result || result == String.Empty) { result = HttpContext.Current.Request.UserHostAddress; }
        if (null == result || result == String.Empty || !IsIP(result)) { return "0.0.0.0"; }
        return result;
    }

    [WebMethod(MessageName = "是否是IP", Description = "检查字符串是否是ip地址的格式")]

    private bool IsIP(string ip) { return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"); }


 

    /// <summary>
    /// 获取n个交易日的截止日期
    /// </summary>
    /// <param name="startdate">开始日期，格式为yyyy-mm-dd</param>
    /// <param name="days">交易日天数</param>
    [WebMethod(MessageName = "获取交易日截止日期", Description = "获取n个交易日的截止日期")]
    public DataSet  GetJYREndDate(string StartDate, int days)
    {
        DataSet dsreturn = InitialDataset().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });
        try
        {
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable htparam = new Hashtable();

            htparam.Add("@startdate", StartDate);
            htparam.Add("@days", days);

            string sql = "select convert(varchar(10),riqi,120) as riqi from (select top (@days) *,row_number() OVER(ORDER BY  riqi) as xuhao  from ZZZ_riqi where daylx='工作日' and riqi>=@startdate) as tab where xuhao=@days";

           Hashtable return_ht = I_DBL.RunParam_SQL(sql,"日期",htparam);
            if ((bool)(return_ht["return_float"]))
            {
                DataTable redb = ((DataSet)return_ht["return_ds"]).Tables["日期"].Copy();
                if (redb.Rows[0]["riqi"].ToString() != "")
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = redb.Rows[0]["riqi"].ToString();     
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取交易日截止日期失败";
                }
            }          
        }
        catch
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行异常！";
        }
        return dsreturn;
    }

    /// <summary>
    /// 用户锁定，针对一个用户进行的业务锁定，调用此锁的地方，只能同时处理此用户一次业务。只有返回1，才能继续进行业务处理，否则取消业务处理
    /// </summary>
    /// <param name="khbh">客户编号</param>
    /// <returns>只有返回1，才能继续进行业务处理，否则取消业务处理，提示系统忙。0代表发生意外。2代表强制解锁了，也必须取消业务处理。</returns>
    [WebMethod(MessageName = "用户锁定", Description = "后台操作-进行更删改查操作时用户锁定")]
    public string LockBegin_ByUser(string khbh)
    {
        string state = UserLock.LockBegin_ByUser(khbh);

        return state;
    }

    /// <summary>
    /// 解除用户锁定,与LockBegin_ByUser配对
    /// </summary>
    /// <param name="khbh">客户编号</param>
    /// <returns>这个返回值没什么用</returns>
    [WebMethod(MessageName = "用户解锁", Description = "后台操作-进行更删改查操作时用户解锁")]
    public string LockEnd_ByUser(string khbh)
    {
        string state = UserLock.LockEnd_ByUser(khbh);

        return state;
    }

 

}
