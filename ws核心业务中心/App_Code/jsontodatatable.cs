using FMPublicClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

/// <summary>
/// jsontodatatable 的摘要说明
/// </summary>
public static class jsontodatatable
{
    public static DataTable ToDataTable(this string json)
    {
        json = StringOP.uncMe(json, "mima");
        DataTable dataTable = new DataTable("接收的子表数据");  //实例化
        DataTable result;

        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
        ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
        if (arrayList.Count > 0)
        {
            foreach (Dictionary<string, object> dictionary in arrayList)
            {
                if (dictionary.Keys.Count<string>() == 0)
                {
                    result = dataTable;
                    return result;
                }
                if (dataTable.Columns.Count == 0)
                {
                    foreach (string current in dictionary.Keys)
                    {
                        dataTable.Columns.Add(current, dictionary[current].GetType());
                    }
                }
                DataRow dataRow = dataTable.NewRow();
                foreach (string current in dictionary.Keys)
                {
                    dataRow[current] = dictionary[current];
                }

                dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
            }
        }

        result = dataTable;
        return result;
    }


    /// <summary>
    /// 解析出用于统计分析图表的标准数据
    /// </summary>
    /// <param name="lx">类型，包括"饼图数据、曲线图数据、柱状图数据"三种</param>
    /// <param name="line">传入影响线条dt数量的列名，空值代表只有一条线并用自定义名字命名，饼图时代表项目列</param>
    /// <param name="line_name">只有一条线时的自定义名称，line空值时这个参数才有效</param>
    /// <param name="linecolor">传入颜色代码数组，空值为默认十个颜色，这个数组内的颜色数量必须大于预估的线条dt数量</param>
    /// <param name="Xstr">传入影响x轴的列名，饼图时代表百分比列</param>
    /// <param name="Ystr">传入影响y轴的列名，饼图时代表颜色列。饼图时传入空，使用默认十个颜色</param>
    /// <param name="olddt_db">传入待解析的datatable</param>
    /// <param name="group">分组开关，空值不自动汇总，yes，则自动汇总累加相同X轴数据的值</param>
    /// <returns></returns>
    public static DataSet re_chart(string lx, string line, string line_name, string[] linecolor, string Xstr, string Ystr, DataTable olddt_db,string group)
    {
        try
        {
            DataTable olddt = olddt_db.Copy();
            olddt.Columns.Add("sp_buchong", typeof(System.String));
            DataSet dsre = new DataSet();
            if (linecolor == null)
            {
                linecolor = new string[] { "#68BC31", "#2091CF", "#FEE074", "#FF0084", "#73880A", "#3F4C6B", "#FF1A00", "#CDEB8B", "#36393D", "#FFFF88" };
            }
            if (lx == "饼图数据")
            {
                DataTable dt_bing;
                if (Ystr == null || Ystr.Trim() == "")
                {
                    dt_bing = olddt.DefaultView.ToTable("饼图数据", true, new string[] { line, Xstr, "sp_buchong" }).Copy();


                    for (int i = 0; i < linecolor.Length; i++)
                    {
                        if (i < dt_bing.Rows.Count)
                        {
                            dt_bing.Rows[i][2] = linecolor[i];
                        }
                    }
                }
                else
                {
                    dt_bing = olddt.DefaultView.ToTable("饼图数据", true, new string[] { line, Xstr, Ystr });
                }
                dt_bing.Columns[0].ColumnName = "项目名";
                dt_bing.Columns[0].Caption = "项目名";
                dt_bing.Columns[1].ColumnName = "百分数";
                dt_bing.Columns[1].Caption = "百分数";
                dt_bing.Columns[2].ColumnName = "颜色";
                dt_bing.Columns[2].Caption = "颜色";
                dsre.Tables.Add(dt_bing.Copy());
                return dsre;

            }
            if (lx == "曲线图数据" || lx == "柱状图数据")
            {
                if (line == null || line.Trim() == "")
                {
                    DataTable dtc2 = new DataTable(lx);
                    dtc2.Columns.Add("项目名");
                    dtc2.Columns.Add("颜色");
                    dtc2.Rows.Add(new string[] { line_name, linecolor[0] });

                    DataTable dt_qz_sub;
                    dt_qz_sub = olddt.DefaultView.ToTable(lx + "-" + line_name, true, new string[] { Xstr, Ystr });
                    dt_qz_sub.Columns[0].ColumnName = "X轴";
                    dt_qz_sub.Columns[0].Caption = "X轴";
                    dt_qz_sub.Columns[1].ColumnName = "Y轴";
                    dt_qz_sub.Columns[1].Caption = "Y轴";
                    dsre.Tables.Add(dtc2.Copy());
                    dsre.Tables.Add(dt_qz_sub.Copy());
                    return dsre;
                }
                else
                {
                    //找出dt线的数量和每一个名字
                    DataTable dtc2 = olddt.DefaultView.ToTable(lx, true, new string[] { line, "sp_buchong" });
                    dtc2.Columns[0].ColumnName = "项目名";
                    dtc2.Columns[0].Caption = "项目名";
                    dtc2.Columns[1].ColumnName = "颜色";
                    dtc2.Columns[1].Caption = "颜色";
                    for (int i = 0; i < linecolor.Length; i++)
                    {
                        if (i < dtc2.Rows.Count)
                        {
                            dtc2.Rows[i][1] = linecolor[i];
                        }
                    }


                    dsre.Tables.Add(dtc2.Copy());

                    for (int i = 0; i < dtc2.Rows.Count; i++)
                    {
                        DataTable dt_qz_sub;
                        DataView view = new DataView(olddt);
                        view.RowFilter = line+"='" + dtc2.Rows[i][0].ToString() + "'";
                        dt_qz_sub = view.ToTable(lx + "-" + dtc2.Rows[i][0].ToString(), true, new string[] { Xstr, Ystr });
                        dt_qz_sub.Columns[0].ColumnName = "X轴";
                        dt_qz_sub.Columns[0].Caption = "X轴";
                        dt_qz_sub.Columns[1].ColumnName = "Y轴";
                        dt_qz_sub.Columns[1].Caption = "Y轴";

                        if (group == null)
                        {
                            group = "";
                        }
                        if(group == "yes")
                        {
                            Common.DataSetHelper dsh = new Common.DataSetHelper();
                            DataTable dt_group = dsh.SelectGroupByInto(lx + "-" + dtc2.Rows[i][0].ToString(), dt_qz_sub, "X轴 X轴,sum(Y轴) Y轴", "", "X轴");
                            dt_qz_sub.Clear();
                            dt_qz_sub = dt_group.Copy();
                        }


                        dsre.Tables.Add(dt_qz_sub.Copy());
                    }
                    return dsre;



                }
            }
            return null;

        }
        catch (Exception ex)
        {
            string aa = "";
            return null;
        }
    }

 

    }