<%@ Page Title="" Language="C#" MasterPageFile="~/adminht/MasterPageMain.master" AutoEventWireup="true" CodeFile="demo_dblist.aspx.cs" Inherits="demo_dblist" %>

 
<%@ Register Src="~/pucu/wuc_css_onlygrid.ascx" TagPrefix="uc1" TagName="wuc_css_onlygrid" %>
<%@ Register Src="~/pucu/wuc_content_onlygrid.ascx" TagPrefix="uc1" TagName="wuc_content_onlygrid" %>
<%@ Register Src="~/pucu/wuc_script_onlygrid.ascx" TagPrefix="uc1" TagName="wuc_script_onlygrid" %>





<asp:Content ID="Content1" ContentPlaceHolderID="sp_head" runat="Server">
    <!-- 附加的head内容 -->
 
    <uc1:wuc_css_onlygrid runat="server" ID="wuc_css_onlygrid" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sp_daohang" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="sp_pagecontent" runat="Server">

 

    <!-- 附加的右侧主要功能切换区内容,不含导航 -->
    <uc1:wuc_content_onlygrid runat="server" ID="wuc_content_onlygrid" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="sp_script" runat="Server">
    <uc1:wuc_script_onlygrid runat="server" ID="wuc_script_onlygrid" />


 

 


         <!-- **********图表点击事件******** -->
     <script type="text/javascript">
         jQuery(function ($) {
             $("#sales-charts").bind("plotclick", function (event, pos, item) {
               //  alert("You clicked at " + pos.x + ", " + pos.y);
                 //item: {
                 //        datapoint: the point, e.g. [0, 2]
                 //        dataIndex: the index of the point in the data array
                 //        series: the series object
                 //        seriesIndex: the index of the series
                 //    pageX, pageY: the global screen coordinates of the point
                 //}
                 if (item) {
                     bootbox.alert
                     bootbox.alert(item.series['label'] + "|" + item.datapoint[0].toString() + "|" + Object.keys(item.series.xaxis.categories)[item.datapoint[0]] + "|" + item.datapoint[1].toString());
                 }
             });
             $("#piechart-placeholder").bind("plotclick", function (event, pos, item) {
                 //alert("You clicked at " + pos.x + ", " + pos.y);
                 // axis coordinates for other axes, if present, are in pos.x2, pos.x3, ...
                 // if you need global screen coordinates, they are pos.pageX, pos.pageY

                 if (item) {
                     
                     bootbox.alert(item.series['label'] + "|" + item.datapoint[0].toString() + "|" + item.datapoint[1].toString());
                 }
             });
         });
     </script>
            <!-- **********其他特殊控制******** -->
     <script type="text/javascript">
         jQuery(function ($) {

    


             if (getUrlParam("onlychart") == "yes") {

                 $("#mysearchtop").hide();
                 $("#kuaijiedaanniuquyu").hide();
                 $("#zheshiliebiaoquyu").hide();
             }
           


             //默认查看今日
             //var myDate = new Date();
             //$('.date-picker').datepicker({ autoclose: true, })
             //$('#CreateTime1').datepicker('setDate', myDate);
             //$('#CreateTime2').datepicker('setDate', myDate);
          
         });
     </script>

</asp:Content>

