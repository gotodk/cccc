using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using FMDBHelperClass;
using FMipcClass;
using System.Collections;
using System.Data;
using FMPublicClass;
using System.Web.Script.Serialization;
using System.Numerics;
using System.Text.RegularExpressions;

/// <summary>
/// 核心业务的相关处理接口
/// </summary>
[WebService(Namespace = "http://corebusiness.aftipc.com/", Description = "V1.00->xxx")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
// [System.Web.Script.Services.ScriptService]
public class bsuser : System.Web.Services.WebService
{

    public bsuser()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }


    # region  基础前置方法

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
    /// 是否开启防篡改验证
    /// </summary>
    /// <returns></returns>
    private bool IsMD5check()
    {
        return true;
    }

    /// <summary>
    /// 测试该接口是否还活着(每个接口必备)
    /// </summary>
    /// <param name="temp">随便传</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(MessageName = "测试接口", Description = "测试该接口是否还活着(每个接口必备)")]
    public string onlinetest(string temp)
    {
        //根据不同的传入值，后续可以检查不同的东西，比如这个接口所连接的数据库，比如进程池，服务器空间等等。。。
        return "ok";
    }


    #endregion



 
    /// <summary>
    /// 验证邀请码是否可用于本次接受邀请。返回是否可用，并返回具体内容
    /// </summary>
    /// <param name="yqm"></param>
    /// <returns></returns>
    [WebMethod(MessageName = "邀请码检查", Description = "邀请码检查")]
    public string checkyqm(string yqm)
    {
      
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = new Hashtable();
        Hashtable param = new Hashtable();
        param.Add("@SN", yqm);

        return_ht = I_DBL.RunParam_SQL("select top 1 * from View_J_my_yqm_fflist where SN=@SN ", "数据记录", param);

        if ((bool)(return_ht["return_float"]))
        {
            DataTable redb = ((DataSet)return_ht["return_ds"]).Tables["数据记录"].Copy();

            if (redb.Rows.Count < 1)
            {
                return "err|您提供的邀请码不存在！";
            }
            else
            {
                DataRow dr = redb.Rows[0];
                if (dr["joinok"].ToString() != "未使用")
                {
                    return "err|一个邀请码只能接受一次邀请，此邀请码已被他人使用！";
                }
                else
                {
                    return "ok|邀请码有效，此验证码是来自“"+ dr["userid_name"].ToString() + "”的邀请！";
                }
               
            }

        }
        else
        {
            return "err|错误err，系统异常！";
        }

         
    }


    /// <summary>
    /// 账号信息重复检查，可验证账号，昵称，手机，邮箱
    /// </summary>
    /// <param name="ddstr">待检查的字符串</param>
    /// <param name="lx">字段名称</param>
    /// <returns></returns>
    [WebMethod(MessageName = "账号信息重复检查", Description = "账号信息重复检查")]
    public string check_pp_zh(string ddstr,string lx)
    {

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = new Hashtable();
        Hashtable param = new Hashtable();
        param.Add("@ddstr", ddstr.Trim());

        return_ht = I_DBL.RunParam_SQL("select top 1 UAid from view_ZZZ_userinfo_ex where "+ lx + "=@ddstr ", "数据记录", param);

        if ((bool)(return_ht["return_float"]))
        {
            DataTable redb = ((DataSet)return_ht["return_ds"]).Tables["数据记录"].Copy();

            if (redb.Rows.Count < 1)
            {
                return "ok";
            }
            else
            {
                return "err|有重复";

            }

        }
        else
        {
            return "err|检查出错";
        }


    }


    /// <summary>
    /// 通过唯一识别号检查权限(通用)
    /// </summary>
    /// <param name="UAid">要检查的UAid</param>
    /// <param name="enumNumber">要检查的权限权值枚举值(在配置中查阅)</param>
    /// <param name="SUfinal_type">当前用户某个类型的最终权值的类型（后台菜单权限,前台导航权限,全局独立权限,特殊权限,备用权限）</param>
    /// <returns>是否具备指定权限</returns>
    [WebMethod(MessageName = "通过唯一识别号检查权限", Description = "通过唯一识别号检查权限")]
    public string chekcAuth_from_UAid(string UAid,string enumNumber,string SUfinal_type)
    {


        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

        Hashtable param = new Hashtable();
        param.Add("@UAid", UAid);



        Hashtable return_ht = new Hashtable();

        //密码验证，其他相关用户状态验证要独立出去，不能混合在这个语句中
        return_ht = I_DBL.RunParam_SQL("select  top 1 UAid,Uloginname ,Uattrcode ,Unumber1  ,Unumber2  ,Unumber3,Unumber4,Unumber5,Uingroups,SuperUser,'0' as UfinalUnumber1,'0' as UfinalUnumber2,'0' as UfinalUnumber3,'0' as UfinalUnumber4,'0' as UfinalUnumber5 from auth_users_auths where UAid=@UAid", "用户信息", param);

        if ((bool)(return_ht["return_float"]))
        {
            DataTable redb = ((DataSet)return_ht["return_ds"]).Tables["用户信息"].Copy();

            if (redb.Rows.Count < 1)
            {
                return "找不到用户信息";
            }
            else
            {
                //状态字段判定
                if (redb.Rows[0]["Uattrcode"].ToString() == "1")
                {
                    return "员工已离职";
                }
                if (redb.Rows[0]["Uattrcode"].ToString() == "2")
                {
                    return "员工已被冻结";
                }
            }



            //重新处理权限，把最终的权值算出来赋上值，权限验证真正用的是这个
            //计算UfinalUnumber的新值
            BigInteger groupNum1 = 0;//组权限搞出来的值
            BigInteger groupNum2 = 0;
            BigInteger groupNum3 = 0;
            BigInteger groupNum4 = 0;
            BigInteger groupNum5 = 0;
            string tiaojian = redb.Rows[0]["Uingroups"].ToString();
            if (tiaojian.Trim() == "")
            { tiaojian = "0"; }
            Hashtable htyiju = I_DBL.RunProc("select Unumber1,Unumber2,Unumber3,Unumber4,Unumber5 from auth_group where SortID in (" + tiaojian + ")", "计算依据");
            if ((bool)(htyiju["return_float"]))
            {
                DataSet ds_yiju = ((DataSet)htyiju["return_ds"]).Copy();
                //累加所有组权限
                for (int i = 0; i < ds_yiju.Tables["计算依据"].Rows.Count; i++)
                {
                    groupNum1 = groupNum1 | BigInteger.Parse(ds_yiju.Tables["计算依据"].Rows[i]["Unumber1"].ToString());
                    groupNum2 = groupNum2 | BigInteger.Parse(ds_yiju.Tables["计算依据"].Rows[i]["Unumber2"].ToString());
                    groupNum3 = groupNum3 | BigInteger.Parse(ds_yiju.Tables["计算依据"].Rows[i]["Unumber3"].ToString());
                    groupNum4 = groupNum4 | BigInteger.Parse(ds_yiju.Tables["计算依据"].Rows[i]["Unumber4"].ToString());
                    groupNum5 = groupNum5 | BigInteger.Parse(ds_yiju.Tables["计算依据"].Rows[i]["Unumber5"].ToString());
                }
                //把组权限累加到直接权限上产生最终权限
                redb.Rows[0]["UfinalUnumber1"] = BigInteger.Parse(redb.Rows[0]["Unumber1"].ToString()) | groupNum1;
                redb.Rows[0]["UfinalUnumber2"] = BigInteger.Parse(redb.Rows[0]["Unumber2"].ToString()) | groupNum2;
                redb.Rows[0]["UfinalUnumber3"] = BigInteger.Parse(redb.Rows[0]["Unumber3"].ToString()) | groupNum3;
                redb.Rows[0]["UfinalUnumber4"] = BigInteger.Parse(redb.Rows[0]["Unumber4"].ToString()) | groupNum4;
                redb.Rows[0]["UfinalUnumber5"] = BigInteger.Parse(redb.Rows[0]["Unumber5"].ToString()) | groupNum5;


                //检查是否具备权限
                string SUfinal_Number = "";
                if (SUfinal_type == "后台菜单权限")
                {
                    SUfinal_Number = redb.Rows[0]["UfinalUnumber1"].ToString();
                }
                if (SUfinal_type == "前台导航权限")
                {
                    SUfinal_Number = redb.Rows[0]["UfinalUnumber2"].ToString();
                }
                if (SUfinal_type == "全局独立权限")
                {
                    SUfinal_Number = redb.Rows[0]["UfinalUnumber3"].ToString();
                }
                if (SUfinal_type == "特殊权限")
                {
                    SUfinal_Number = redb.Rows[0]["UfinalUnumber4"].ToString();
                }
                if (SUfinal_type == "备用权限")
                {
                    SUfinal_Number = redb.Rows[0]["UfinalUnumber5"].ToString();
                }


                //超级用户就直接返回
                if (redb.Rows[0]["SuperUser"].ToString() == "1")
                {
                    return "有权";
                }
                //用户权限
                BigInteger qx = BigInteger.Parse(SUfinal_Number);
                //枚举的要判定值
                BigInteger eun = BigInteger.Parse(enumNumber);
                //判定是否具备权限
                if ((qx & eun) == eun)
                {
                    return "有权";
                }
                else
                {
                    return "无权";
                }
 
            }
            else
            {
                //枚得不到正常的计算依据
                return "获取组权限时出现问题";
            }


        }
        else
        {
            return "意外错误，权限检查失败";
        }
    }

    /// <summary>
    /// 根据UAid获取用户某个信息
    /// </summary>
    /// <param name="UAid">UAid</param>
    /// <param name="colname">列名</param>
    /// <returns></returns>
    [WebMethod(MessageName = "获取账号某个信息", Description = "获取账号某个信息")]
    public string get_user_info_onecol(string UAid, string colname)
    {

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = new Hashtable();
        Hashtable param = new Hashtable();
        param.Add("@UAid", UAid.Trim());

        return_ht = I_DBL.RunParam_SQL("select top 1 "+ colname + " from view_ZZZ_userinfo_ex where UAid = @UAid ", "数据记录", param);

        if ((bool)(return_ht["return_float"]))
        {
            DataTable redb = ((DataSet)return_ht["return_ds"]).Tables["数据记录"].Copy();

            if (redb.Rows.Count < 1)
            {
                return "";
            }
            else
            {
                return redb.Rows[0][colname].ToString();

            }

        }
        else
        {
            return "";
        }


    }



    /// <summary>
    /// 提交注册资料
    /// </summary>
    /// <param name="parameter_forUI">参数</param>
    /// <returns>返回ok就是接口正常</returns>
    [WebMethod(MessageName = "提交注册资料", Description = "提交注册资料")]
    public string tijiaozhuceziliao(DataTable parameter_forUI)
    {
        //接收转换参数
        Hashtable ht_forUI = new Hashtable();
        for (int i = 0; i < parameter_forUI.Rows.Count; i++)
        {
            ht_forUI[parameter_forUI.Rows[i]["参数名"].ToString()] = parameter_forUI.Rows[i]["参数值"].ToString();
        }

        //进行详细的注册资料合法性验证
        if (ht_forUI["zhanghao"].ToString().Trim() == "")
        {
            return "err|zhanghao|没有填写“登录账号”！";
        }
        if (ht_forUI["zhanghao"].ToString().Trim().IndexOf('>') >=0 || ht_forUI["zhanghao"].ToString().Trim().IndexOf('<') >= 0)
        {
            return "err|zhanghao|“登录账号”含有禁用字符！";
        }
        string jc_zh = check_pp_zh(ht_forUI["zhanghao"].ToString().Trim(), "Uloginname") ;
        if (jc_zh != "ok")
        {
            return "err|zhanghao|“登录账号”已被使用，请更换！";
        }

        if (ht_forUI["mima"].ToString().Trim() == "")
        {
            return "err|mima|没有填写“登录密码”！";
        }
        if (ht_forUI["mima"].ToString().IndexOf(" ") >= 0 || Regex.IsMatch(ht_forUI["mima"].ToString(), @"[\u4e00-\u9fa5]"))
        {
            return "err|mima|“登录密码”中不允许含有空格和中文！";
        }

        if (ht_forUI["mima"].ToString() != ht_forUI["remima"].ToString())
        {
            return "err|remima|“重复密码”与“登录密码”不一致！";
        }
        if (ht_forUI["nicheng"].ToString().Trim() == "")
        {
            return "err|nicheng|没有填写“昵称”！";
        }
        string jc_nc = check_pp_zh(ht_forUI["nicheng"].ToString().Trim(), "xingming");
        if (jc_nc != "ok")
        {
            return "err|zhanghao|“昵称”已被使用，请更换！";
        }


        if (ht_forUI["shoujihao"].ToString().Trim() == "")
        {
            return "err|shoujihao|没有填写“手机号码”！";
        }
        //string jc_sj = check_pp_zh(ht_forUI["shoujihao"].ToString().Trim(), "shoujihao");
        //if (jc_sj != "ok")
        //{
        //    return "err|zhanghao|“手机号码”已被使用，请更换！";
        //}
        string sjh = ht_forUI["shoujihao"].ToString().Trim();
        Regex reg1 = new Regex(@"^[0-9]\d*$");
        if (sjh.Length != 11 || !reg1.IsMatch(sjh))
        {
            return "err|shoujihao|“手机号码”格式不正确！";
        }


        //进行短信验证码的验证
        if (ht_forUI["dxyzm"].ToString().Trim() == "")
        {
            return "err|dxyzm|没有填写“短信验证码”！";
        }
        string sjh_enc = StringOP.encMe(sjh, "mima");
        //取出数字，换算出验证码
        string yzm = Regex.Replace(sjh_enc, "[a-z]", "", RegexOptions.IgnoreCase);
        yzm = yzm.PadLeft(100, '0');
        yzm = yzm.Substring(95, 4);
        if (ht_forUI["dxyzm"].ToString().Trim() != yzm)
        {
            return "err|dxyzm|“短信验证码”有误！";
        }



        if (!ht_forUI.Contains("ppxingbie") || (ht_forUI["ppxingbie"].ToString().Trim() != "男" && ht_forUI["ppxingbie"].ToString().Trim() != "女"))
        {
            return "err|ppxingbie|没有选择“性别”！";
        }
        if (ht_forUI["yaoqingma"].ToString().Trim() == "")
        {
            return "err|yaoqingma|没有填写“邀请码”！";
        }

        

        //进行邀请码的验证
        string yqm = ht_forUI["yaoqingma"].ToString().ToUpper().Trim();
        string rerere = checkyqm(yqm);
        if (rerere.IndexOf("ok|") == 0)
        {

        }
        else
        {
            return "err|yaoqingma|" + rerere.Replace("err|","");
        }

        //保存注册信息

        //开始真正的处理，根据业务逻辑操作数据库
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = new Hashtable();
        ArrayList alsql = new ArrayList();
        Hashtable param = new Hashtable();
        //以可排序guid方式生成
        string guid = CombGuid.GetNewCombGuid("U");
        param.Add("@UAid", guid);
        param.Add("@Uloginname", ht_forUI["zhanghao"].ToString().Trim());

        //对密码进行加密
        string mima_enc = StringOP.encMe(ht_forUI["mima"].ToString().Trim(), "mima");
        param.Add("@Uloginpassword", mima_enc);

        param.Add("@xingming", ht_forUI["nicheng"].ToString());
        param.Add("@zhuangtai", "在职");
        param.Add("@zhiwei", "见习盟友");
        param.Add("@xingbie", ht_forUI["ppxingbie"].ToString().Trim());


        param.Add("@beizhu", "");

        param.Add("@gongzuodi", "");
        param.Add("@suoshuquyu", "1");//默认镜海盟部门
        param.Add("@shoujihao", ht_forUI["shoujihao"].ToString().Trim());
        param.Add("@gudingdianhua", "");
        param.Add("@youxiang", "");
        param.Add("@lingdao", "否");

        param.Add("@ss_yaoqingma", yqm);

        param.Add("@Uattrcode", "-1");

        //插入账号表
        alsql.Add("INSERT INTO  auth_users_auths(UAid ,Uloginname,Uloginpassword,Uattrcode) VALUES(@UAid ,@Uloginname,@Uloginpassword,@Uattrcode )");
        alsql.Add("INSERT INTO  ZZZ_userinfo(UAid ,xingming,zhuangtai,zhiwei,xingbie,beizhu,gongzuodi,suoshuquyu,shoujihao,gudingdianhua,youxiang,lingdao,ss_yaoqingma) VALUES(@UAid ,@xingming,@zhuangtai,@zhiwei,@xingbie,@beizhu,@gongzuodi,@suoshuquyu,@shoujihao,@gudingdianhua,@youxiang,@lingdao,@ss_yaoqingma)");
        //更新邀请码使用状态
        alsql.Add("update AAA_SJS set joinok=1,joinuser=@UAid,joinsj=getdate() where SN=@ss_yaoqingma and beok=1");


        //设置初始权限组
        param.Add("@morenqanxianshezhi", "25");//默认见习盟友
        alsql.Add("update auth_users_auths set Uingroups=@morenqanxianshezhi where UAid=@UAid");

        return_ht = I_DBL.RunParam_SQL(alsql, param);

        if ((bool)(return_ht["return_float"]))
        {
            return "ok|欢迎您正式成为镜海盟的一员！";
        }
        else
        {
            return "err|xxxx|"+ "系统故障，保存失败：" + return_ht["return_errmsg"].ToString();
        }

      

    }



 
 


    /// <summary>
    /// 检查右下角提醒
    /// </summary>
    /// <param name="UAid">登陆邮箱</param>
    /// <returns></returns>
    [WebMethod(MessageName = "检查右下角提醒", Description = "检查右下角提醒")]
    public DataSet CheckFormTrayMsg_XTKZ(string UAid)
    {
        //初始化返回值,先塞一行数据
        DataSet dsreturn = initReturnDataSet().Copy();
        dsreturn.Tables["返回值单条"].Rows.Add(new string[] { "err", "初始化" });

        try
        {
            if (string.IsNullOrEmpty(UAid))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "传递的参数不能为空";
                return dsreturn;
            }

            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");

          

      
            //执行语句                 
            Hashtable htCS = new Hashtable();
            htCS["@UserId"] = UAid;
            Hashtable HTSelect = I_DBL.RunParam_SQL("select * FROM pZHTXB WHERE UserId=@UserId and SFYXS = 'F'", "", htCS);
            if (!(bool)HTSelect["return_float"])
            {
                return null;
            }
            DataSet dstx = (DataSet)HTSelect["return_ds"];
            if (dstx != null && dstx.Tables[0].Rows.Count > 0)
            {
                Hashtable ht = new Hashtable();
                ht["@UserId"] = UAid;
                Hashtable returnHT = I_DBL.RunParam_SQL("update pZHTXB set SFYXS = 'T',XSTime=getdate() WHERE UserId=@UserId", ht);
                if (!(bool)returnHT["return_float"])
                {
                    return null;
                }
                if (returnHT["return_other"] != null && Convert.ToInt32(returnHT["return_other"].ToString()) > 0)
                {
                    dstx.Tables[0].TableName = "提醒数据";
                    dsreturn.Tables.Add(dstx.Tables[0].Copy());
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "找到了提醒";
                    return dsreturn;
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到提醒";
                    return dsreturn;
                }

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到提醒";
                return dsreturn;
            }

        }
        catch (Exception)
        {
            return null;
        }
    }



}
