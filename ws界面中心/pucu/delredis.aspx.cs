using FMDBHelperClass;
using FMipcClass;
using FMPublicClass;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pucu_delredis : System.Web.UI.Page
{
 

    protected void Page_Load(object sender, EventArgs e)
    {
        string fsid = Request["zheshiyige_FID"].ToString();
        long delshu = 0;
        try
        {
            RedisClient RC = RedisClass.GetRedisClient(null);
            string hashid = "ZlistCache_" + fsid;
            delshu = RC.Del(hashid);
        }
        catch {
            Response.Write("缓存清理失败，无法访问缓存！");
            return;
        }

        //返回下载地址和状态""
        if (delshu > 0)
        {
            Response.Write("缓存清理完成，下次刷新将获得最新数据并重新建立缓存！");
        }
        else
        {
            Response.Write("缓存不存在，无需清理！");
        }
       
    }
}