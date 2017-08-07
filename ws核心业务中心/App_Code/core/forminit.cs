using FMDBHelperClass;
using FMipcClass;
using FMPublicClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// 服务中常用的一些基本方法，集中于此方便调用
/// 2017年7月份 new
/// </summary>
public class forminit
{
    public forminit()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 获取省市区信息 wyh 2014.06.03 add 获取是省市区基本信息 
    /// </summary>
    /// <returns>包含省市区信息的Dataset</returns>
    public DataSet dsSSQ()
    {
        DataSet dsssq = new DataSet();
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet ds = new DataSet();
        Hashtable htresult = new Hashtable();
        //获取省市区
        DataTable dt_sheng = new DataTable("省");

        htresult = I_DBL.RunProc("select p_number as 省编号,p_namestr as 省名,'0' as 省标记 from AAA_CityList_Promary", "省");
        dt_sheng = ((DataSet)htresult["return_ds"]).Tables["省"].Copy();


        DataTable dt_shi = new DataTable("市");
        htresult = I_DBL.RunProc("select AAA_CityList_City.c_number as 市编号,AAA_CityList_City.c_namestr as 市名,AAA_CityList_City.p_number as 所属省编号,(select AAA_CityList_Promary.p_namestr from AAA_CityList_Promary where AAA_CityList_Promary.p_number=AAA_CityList_City.p_number) as 所属省名 from AAA_CityList_City", "市");
        dt_shi = ((DataSet)htresult["return_ds"]).Tables["市"].Copy();


        DataTable dt_qu = new DataTable("区");
        htresult = I_DBL.RunProc("select AAA_CityList_qu.q_number as 区编号,AAA_CityList_qu.q_namestr as 区名,AAA_CityList_qu.c_number as 所属市编号,(select AAA_CityList_City.c_namestr from AAA_CityList_City where AAA_CityList_qu.c_number=AAA_CityList_City.c_number) as 所属市名 from AAA_CityList_qu", "区");
        dt_qu = ((DataSet)htresult["return_ds"]).Tables["区"].Copy();

        dsssq.Tables.Add(dt_sheng);
        dsssq.Tables.Add(dt_shi);
        dsssq.Tables.Add(dt_qu);
        return dsssq;
    }

    /// <summary>
    /// 获取动态参数
    /// </summary>
    /// <returns></returns>
    public DataSet GetDynamicParameters()
    {
        DataSet paras = new DataSet();

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");       
        Hashtable htresult = new Hashtable();
        
        DataTable dt_paras = new DataTable("动态参数");

        htresult = I_DBL.RunProc("SELECT [XM] as 项目,[SZ] as 数值 FROM pDTCSB  WHERE [SFYX]='是'", "动态参数");
        dt_paras = ((DataSet)htresult["return_ds"]).Tables["动态参数"].Copy();

        paras.Tables.Add(dt_paras);
        return paras;


    }

    /// <summary>
    /// 获取所有商品名称
    /// </summary>
    /// <returns></returns>
    public DataSet GetAllSPBH()
    {
        DataSet paras = new DataSet();

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable htresult = new Hashtable();

        DataTable dt_paras = new DataTable("商品拼音列表");

        htresult = I_DBL.RunProc("SELECT PrdID as 商品编号, PrdName as 商品名称, GGXH as 型号规格 FROM pSPXXB  WHERE SPZT='T' ", "商品拼音列表");
        dt_paras = ((DataSet)htresult["return_ds"]).Tables["商品拼音列表"].Copy();
        //加载拼音
        dt_paras.Columns.Add("简拼");
        dt_paras.Columns.Add("全拼");
        for (int p = 0; p < dt_paras.Rows.Count; p++)
        {
            dt_paras.Rows[p]["全拼"] = PTHelper.Convert(dt_paras.Rows[p]["商品名称"].ToString()).ToLower();
            dt_paras.Rows[p]["简拼"] = PTHelper.GetHeadLetters(dt_paras.Rows[p]["商品名称"].ToString()).ToLower();
        }
        paras.Tables.Add(dt_paras);
        return paras;


    }


    /// <summary>
    /// 获取商品分类为初始化
    /// </summary>
    /// <returns></returns>
    public DataSet GetInitFL()
    {
        DataSet paras = new DataSet();

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable htresult = new Hashtable();

        DataTable dt_paras = new DataTable("商品分类");

        htresult = I_DBL.RunProc("SELECT *,dbo.fn_Sort_Tree(SortName,SortParentPath,',',(SELECT COUNT(0) FROM pSPLBB B WHERE B.SortParentID=A.SortParentID AND B.SortOrder>A.SortOrder)) AS SortNameTree,(SELECT COUNT(0) FROM pSPLBB B WHERE B.SortParentID=A.SortParentID AND B.SortOrder<A.SortOrder) AS SortMoveUp,(SELECT -COUNT(0) FROM pSPLBB B where B.SortParentID=A.SortParentID AND B.SortOrder>A.SortOrder) AS SortMoveDown FROM pSPLBB A  where m_show='不隐藏' order by SortOrder ASC", "商品分类");
        dt_paras = ((DataSet)htresult["return_ds"]).Tables["商品分类"].Copy();

        paras.Tables.Add(dt_paras);
        return paras;


    }

    /// <summary>
    /// 获取动态参数
    /// </summary>
    /// <param name="I_DBL">数据连接</param>
    /// <returns></returns>
    public DataSet GetDynamicParameters(I_Dblink I_DBL)
    {
        DataSet paras = new DataSet();
       
        Hashtable htresult = new Hashtable();

        DataTable dt_paras = new DataTable("动态参数");

        htresult = I_DBL.RunProc("SELECT [XM] as 项目,[SZ] as 数值 FROM pDTCSB  WHERE [SFYX]='是'", "动态参数");

        if ((bool)(htresult["return_float"]))
        {
            DataSet ds = (DataSet)htresult["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == 1)
            {               
                dt_paras = ds.Tables["动态参数"].Copy();    

            }           
        } 
        dt_paras = ((DataSet)htresult["return_ds"]).Tables["动态参数"].Copy();
        paras.Tables.Add(dt_paras);
        return paras;


    }

    /// <summary>
    /// 判断商品是否有效
    /// </summary>
    /// <param name="pridID"></param>
    /// <returns></returns>
    public bool ValPrd(string pridID)
    {
        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable htresult = new Hashtable();

        DataTable dt_prds = new DataTable("商品列表");

        htresult = I_DBL.RunProc("select SPZT from pSPXXB where PrdID='"+ pridID + "'", "商品列表");
        dt_prds = ((DataSet)htresult["return_ds"]).Tables["商品列表"].Copy();

        if (dt_prds.Rows.Count > 0 && dt_prds.Rows[0]["SPZT"].ToString().Trim() == "T")
        { return true; }
        else
        { return false; }
       
    }


    /// <summary>
    /// 根据合同编号获取买方账号的经纪人以及区代等相关信息
    /// </summary>
    /// <param name="I_DBL"></param>
    /// <param name="htbh"></param>
    /// <returns></returns>
    public DataTable GetBuyAccountNumbersByHTID(I_Dblink I_DBL, string htbh)
    {

        String sql = "select BA.UserId as '区代账号',GLJJRuserid as '经纪人账号' ,HTID as '合同编号' ,BSSQY as '所属区域' , BUserId as '买方账号', BFullName as '买方名称'  from tHTXXB left join  (select * from uQYDLXXB where uQYDLXXB.SFYX='T' ) as BA  on  BSSQY= (DLQY_Prov +'|'+ DLQY_city +'|'+DLQY_qu ) left join uUserInfo on BUserId=uUserInfo.UserId where HTID=@HTID ";

        Hashtable htin = new Hashtable();
        htin["@HTID"] = htbh;

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", htin);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == 1)
            {
                DataTable AccountNumber = new DataTable();
                AccountNumber = ds.Tables["dtResult"].Copy();
                AccountNumber.TableName = "关联账号信息";

                return AccountNumber;

            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }    

    /// <summary>
    /// 根据合同编号获取卖方账号的经纪人以及区代等相关信息
    /// </summary>
    /// <param name="I_DBL"></param>
    /// <param name="htbh"></param>
    /// <returns></returns>
    public DataTable GetSellAccountNumbersByHTID(I_Dblink I_DBL, string htbh)
    {

        String sql = "select SA.UserId as '区代账号',GLJJRuserid as '经纪人账号' ,HTID  as '合同编号',SSSQY as '所属区域',SUserId as '卖方账号',SFullName as '卖方名称' from tHTXXB left join  (select * from uQYDLXXB where uQYDLXXB.SFYX='T' ) as SA  on  SSSQY= (DLQY_Prov +'|'+ DLQY_city +'|'+DLQY_qu ) left join uUserInfo on BUserId=uUserInfo.UserId where HTID=@HTID ";

        Hashtable htin = new Hashtable();
        htin["@HTID"] = htbh;

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", htin);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == 1)
            {
                DataTable AccountNumber = new DataTable();
                AccountNumber = ds.Tables["dtResult"].Copy();
                AccountNumber.TableName = "关联账号信息";

                return AccountNumber;

            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// 根据合同编号获取买卖方经纪人以及区代等相关信息
    /// </summary>
    /// <param name="I_DBL"></param>
    /// <param name="htbh"></param>
    /// <returns></returns>
    public DataTable GetAllAccountNumbersByHTID(I_Dblink I_DBL, string htbh)
    {
        String sql = "select B.UserId as '买方区代账号', B.GLJJRuserid as '买方经纪人账号', B.BSSQY as '买方所属区域', B.BUserId as '买方账号',B.BFullName as '买方名称',S.UserId as '卖方区代账号', S.GLJJRuserid as '卖方经纪人账号', S.SSSQY as '卖方所属区域', S.SUserId as '卖方账号',S.SFullName as '卖方名称'　from (select BA.UserId,GLJJRuserid,HTID  ,BSSQY , BUserId, BFullName   from tHTXXB left join  (select * from uQYDLXXB where uQYDLXXB.SFYX='T' ) as BA on  BSSQY= (DLQY_Prov +'|'+ DLQY_city +'|'+DLQY_qu ) left join uUserInfo on BUserId=uUserInfo.UserId) B join (select SA.UserId ,GLJJRuserid  ,HTID  ,SSSQY ,SUserId ,SFullName  from tHTXXB left join  (select * from uQYDLXXB where uQYDLXXB.SFYX='T' ) as SA  on  SSSQY= (DLQY_Prov +'|'+ DLQY_city +'|'+DLQY_qu ) left join uUserInfo on BUserId=uUserInfo.UserId) S on B.HTID=S.HTID where B.HTID = @HTID ";

        Hashtable htin = new Hashtable();
        htin["@HTID"] = htbh;

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", htin);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == 1)
            {
                DataTable AccountNumber = new DataTable();
                AccountNumber = ds.Tables["dtResult"].Copy();
                AccountNumber.TableName = "关联账号信息";

                return AccountNumber;

            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// 根据用户编号获取相应的经纪人编号
    /// </summary>
    /// <param name="I_DBL"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public DataTable GetAccountNumber(I_Dblink I_DBL,string userId)
    {
        
        String sql = "select uUserInfo.UserId as '用户账号',GLJJRuserid as '经纪人账号' from uUserInfo  where uUserInfo.UserId =@UserId ";

        Hashtable htin = new Hashtable();
        htin["@UserId"] = userId;

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", htin);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == 1)
            {
                DataTable AccountNumber = new DataTable();
                AccountNumber = ds.Tables["dtResult"].Copy();
                AccountNumber.TableName = "关联账号信息";

                return AccountNumber;

            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
      
    }

    /// <summary>
    /// 计算劳务所得税   
    /// A、0<收益基数<=800，个税=0，收益=收益基数
    /// B、800<收益基数<=4000，个税=（收益基数-800）*0.2，收益=收益基数-个税
    /// C、4000<收益基数<=25000，个税=收益基数*0.8*0.2，收益=收益基数-个税
    /// D、25000<收益基数<=62500，个税=收益基数*0.8*0.3-2000，收益=收益基数-个税
    /// E、收益基数>62500，个税=收益基数*0.8*0.4-7000，收益=收益基数-个税
    /// </summary>
    /// <param name="income"></param>
    /// <returns></returns>
    public double GetiTaxes(double income)
    {
        if (income <= 800)
        {
            return 0;
        }
        else if (800 < income && income <= 4000)
        {
            return (income - 800) * 0.2;
        }

        else if (4000 < income && income <= 25000)
        {
            return income * 0.8 * 0.2;
        }
        else if (25000 < income && income <= 625000)
        {
            return income * 0.8 * 0.3 - 2000;
        }
        else if (income > 625000)
        {
            return income * 0.8 * 0.4 - 7000;
        }
        else
        {
            return 0;
        }
        
    }


    /// <summary>
    /// 根据提货单号获取对应的发货单号以及发货单状态
    /// </summary>
    /// <param name="I_DBL"></param>
    /// <param name="thNumber"></param>
    /// <returns></returns>
    public DataTable GetFHNumber(I_Dblink I_DBL, string thNumber,string fhlx)
    {
        String sql = "select FHDID,FHDLX, THDID,FHDZT,FHSL from tFHXXB where THDID=@THDID and FHDLX=@FHDLX and( FHDZT is null or  FHDZT!='完成')";

        Hashtable htin = new Hashtable();
        htin["@THDID"] = thNumber;
        htin["@FHDLX"] = fhlx;

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", htin);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == 1)
            {
                DataTable fh = new DataTable();
                fh = ds.Tables["dtResult"].Copy();
                fh.TableName = "发货信息";

                return fh;

            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
        
    }

    /// <summary>
    /// 根据合同编号获取其相应的未完成的提货单的相关信息
    /// </summary>
    /// <param name="I_DBL"></param>
    /// <param name="htid"></param>
    /// <returns></returns>
    public DataTable GetTHRelativeHT(I_Dblink I_DBL,string htid)
    {
        String sql = "select THDID,HTID,PrdID,DJHK,THDZT,THDstate from tTHDXXB where THDZT !='完成' and HTID=@HTID ";

        Hashtable htin = new Hashtable();
        htin["@HTID"] = htid;        

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", htin);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null )
            {
                DataTable th = new DataTable();
                th = ds.Tables["dtResult"].Copy();
                th.TableName = "相关信息";

                return th;

            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取多个用户账户余额信息
    /// </summary>
    /// <param name="I_DBL"></param>
    /// <param name="users"></param>
    /// <returns></returns>
    public DataTable GetCurrentMState(I_Dblink I_DBL, params string[] users)
    {
        Hashtable idpara = new Hashtable();

        //查询字符串
        string s = "";

        //添加参数，构造字符串
        for (int i = 0; i < users.Count(); i++)
        {
            idpara.Add("@" + i.ToString(), users[i]);

            if (i == 0)
            {
                s = "@" + i.ToString();
            }
            else
            {
                s += ",@" + i.ToString();
            }
        }

        //多条记录使用in，单条不使用
        string str = "SELECT [UserId] as 账户编号,ISNULL([XTMoney],0) as 系统余额,ISNULL([CGMoney],0) as 存管余额,ISNULL([XYJF],0) as 信用积分,[XYDJ] as 信用等级 FROM mZHYEXYB   WHERE [UserId] ";
        String sql = users.Count() == 1 ? str + "=" + s : str + "in (" + s + ")";                       

        Hashtable ht = I_DBL.RunParam_SQL(sql, "dtResult", idpara);
        if ((bool)(ht["return_float"]))
        {
            DataSet ds = (DataSet)ht["return_ds"];
            if (ds != null && ds.Tables["dtResult"] != null && ds.Tables["dtResult"].Rows.Count == users.Count())
            {
                DataTable dtUInfo = new DataTable();
                dtUInfo = ds.Tables["dtResult"].Copy();
                dtUInfo.TableName = "余额与信用";
                return dtUInfo;

            }           
        }
        return null;

    }
    

}