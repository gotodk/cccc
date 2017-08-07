using System;
using System.Collections.Generic;
using System.Web;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;
using System.Numerics;
using System.Web.Script.Serialization;

public class NoReSet_170318000011
{
 

    /// <summary>
    /// 初始化返回值数据集,执行结果只有两种ok和err(大多数情况是这个标准)
    /// </summary>
    /// <returns></returns>
    private DataSet initReturnDataSet()
    {
        DataSet ds = new DataSet();
        DataTable auto2 = new DataTable();
        auto2.TableName = "返回值单条";
        auto2.Columns.Add("执行结果");
        auto2.Columns.Add("提示文本");
        auto2.Columns.Add("附件信息1");
        auto2.Columns.Add("附件信息2");
        auto2.Columns.Add("附件信息3");
        auto2.Columns.Add("附件信息4");
        auto2.Columns.Add("附件信息5");
        ds.Tables.Add(auto2);
        return ds;
    }

    /// <summary>
    /// 增加数据
    /// </summary>
    /// <param name="parameter_forUI">前台表单传来的参数</param>
    /// <returns></returns>
    public DataSet NRS_ADD(DataTable parameter_forUI)
    {
         
        return null;
    }

    /// <summary>
    /// 编辑数据
    /// </summary>
    /// <param name="parameter_forUI">前台表单传来的参数</param>
    /// <returns></returns>
    public DataSet NRS_EDIT(DataTable parameter_forUI)
    {
        //接收转换参数
        Hashtable ht_forUI = new Hashtable();
        for (int i = 0; i < parameter_forUI.Rows.Count; i++)
        {
            ht_forUI[parameter_forUI.Rows[i]["参数名"].ToString()] = parameter_forUI.Rows[i]["参数值"].ToString();
        }


        //初始化返回值
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //参数合法性各种验证，这里省略
        if (ht_forUI["idforedit"].ToString().Trim() == "")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有明确的修改目标！";
            return dsreturn;
        }

        //System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^\w+$");
        //bool regre = reg1.IsMatch(ht_forUI["Uloginname"].ToString());
        if (ht_forUI["Uloginname"].ToString().IndexOf('<') >= 0 || ht_forUI["Uloginname"].ToString().IndexOf('>') >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "错误：登录账号含有禁用字符！";
            return dsreturn;
        }

        //只能修改自己
        if (ht_forUI["yhbsp_session_uer_UAid"].ToString().Trim() != ht_forUI["idforedit"].ToString().Trim())
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "只允许修改个人资料！";
            return dsreturn;
        }

        //开始真正的处理，这里只是演示，所以直接在这里写业务逻辑代码了

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = new Hashtable();
        ArrayList alsql = new ArrayList();
        Hashtable param = new Hashtable();
        param.Add("@UAid", ht_forUI["idforedit"].ToString());
        param.Add("@Uloginname", ht_forUI["Uloginname"].ToString());

        param.Add("@xingming", ht_forUI["xingming"].ToString());

        param.Add("@xingbie", ht_forUI["xingbie"].ToString());


        param.Add("@beizhu", ht_forUI["beizhu"].ToString());

        param.Add("@gongzuodi", ht_forUI["gongzuodi"].ToString());

        param.Add("@shoujihao", ht_forUI["shoujihao"].ToString());
        param.Add("@gudingdianhua", ht_forUI["gudingdianhua"].ToString());
        param.Add("@youxiang", ht_forUI["youxiang"].ToString());
        param.Add("@tongxundizhi", ht_forUI["tongxundizhi"].ToString());

        param.Add("@sfzh", ht_forUI["sfzh"].ToString());
        param.Add("@csrq", ht_forUI["csrq"].ToString());
        param.Add("@sheng", ht_forUI["yhb_city_Promary_shengshiqu"].ToString());
        param.Add("@shi", ht_forUI["yhb_city_City_shengshiqu"].ToString());
        param.Add("@qu", ht_forUI["yhb_city_Qu_shengshiqu"].ToString());
        param.Add("@minzu", ht_forUI["minzu"].ToString());
        param.Add("@yuanxiao", ht_forUI["yuanxiao"].ToString());
        param.Add("@wxhm", ht_forUI["wxhm"].ToString());
        param.Add("@qqhaoma", ht_forUI["qqhaoma"].ToString());
  

        //建档人和更新人 addtime,gxtime
        //param.Add("@addren", ht_forUI["yhbsp_session_uer_UAid"].ToString());
        param.Add("@gxren", ht_forUI["yhbsp_session_uer_UAid"].ToString());



   
        alsql.Add("UPDATE ZZZ_userinfo SET xingming=@xingming,xingbie=@xingbie,beizhu=@beizhu,gongzuodi=@gongzuodi,shoujihao=@shoujihao,gudingdianhua=@gudingdianhua,youxiang=@youxiang,sfzh=@sfzh,csrq=@csrq,sheng=@sheng,shi=@shi,qu=@qu,minzu=@minzu,yuanxiao=@yuanxiao,wxhm=@wxhm,qqhaoma=@qqhaoma,tongxundizhi=@tongxundizhi,gxren=@gxren,gxtime=getdate() where UAid=@UAid ");
      
        return_ht = I_DBL.RunParam_SQL(alsql, param);




        if ((bool)(return_ht["return_float"]))
        {

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "修改成功！";
        }
        else
        {
            //其实要记录日志，而不是输出，这里只是演示
            //dsreturn.Tables.Add(parameter_forUI.Copy());
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统故障，修改失败：" + return_ht["return_errmsg"].ToString();
        }





        return dsreturn;
    }

    /// <summary>
    /// 编辑数据前获取数据
    /// </summary>
    /// <param name="parameter_forUI">前台表单传来的参数</param>
    /// <returns></returns>
    public DataSet NRS_EDIT_INFO(DataTable parameter_forUI)
    {
        //接收转换参数
        Hashtable ht_forUI = new Hashtable();
        for (int i = 0; i < parameter_forUI.Rows.Count; i++)
        {
            ht_forUI[parameter_forUI.Rows[i]["参数名"].ToString()] = parameter_forUI.Rows[i]["参数值"].ToString();
        }


        //初始化返回值
        DataSet dsreturn = initReturnDataSet().Clone();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        //参数合法性各种验证，这里省略

        //开始真正的处理，这里只是演示，所以直接在这里写业务逻辑代码了

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = new Hashtable();
        Hashtable param = new Hashtable();
        param.Add("@UAid", ht_forUI["idforedit"].ToString());

        return_ht = I_DBL.RunParam_SQL("select  top 1 * from view_ZZZ_userinfo_ex where UAid=@UAid", "数据记录", param);

        if ((bool)(return_ht["return_float"]))
        {
            DataTable redb = ((DataSet)return_ht["return_ds"]).Tables["数据记录"].Copy();

            if (redb.Rows.Count < 1)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "没有找到指定数据!";
                return dsreturn;
            }
            redb.Rows[0]["Uloginpassword"] = StringOP.uncMe(redb.Rows[0]["Uloginpassword"].ToString(),"mima");
            dsreturn.Tables.Add(redb);


            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "意外错误，获取失败：" + return_ht["return_errmsg"].ToString();
        }


        return dsreturn;
    }


}
