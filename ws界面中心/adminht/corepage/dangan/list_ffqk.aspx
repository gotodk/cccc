<%@ Page Title="" Language="C#" MasterPageFile="~/adminht/MasterPageMain.master" AutoEventWireup="true" CodeFile="list_ffqk.aspx.cs" Inherits="dangan_list_ffqk" %>

 
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

    <!-- 自定义新增 -->
        <div id="dialog-message-test" class="hide">
        











<div class="row">
                                    <div class="col-sm-12">

<span>待发送邀请码：<span id="yqmshow"></span></span>
                                        <br/>
              <div class="input-group">
																	<span class="input-group-addon">
																		<i class="ace-icon fa fa-phone-square"></i>
																	</span>

																	<input type="text" class="form-control search-query" placeholder="输入朋友的手机号" id="sh_shoujihao">
																	<span class="input-group-btn">
																		<button type="button" class="btn btn btn-sm btn-danger  " id="sh_send">
																			<span class="ace-icon fa fa-gift icon-on-right bigger-110"></span>
																			发送邀请码短信
																		</button>
																	</span>
																</div>
                                    </div>
                                    <!-- /.col -->
                                </div>
                                <!-- /.row -->
















    </div>
    <!-- #dialog-message -->


    <!-- 附加的右侧主要功能切换区内容,不含导航 -->
    <uc1:wuc_content_onlygrid runat="server" ID="wuc_content_onlygrid" />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="sp_script" runat="Server">
    <uc1:wuc_script_onlygrid runat="server" ID="wuc_script_onlygrid" />


     <!-- **********自定义新增脚本******** -->
     <script type="text/javascript">
          var nowloginuser = "<%=UserSession.唯一键%>";
         $("#zheshiliebiaoquyu").attr('teshuwhere', " userid='" + nowloginuser + "'   ");



         window.setInterval(function () {
             if ($(".hasbzff").length < 1) {
                 $("a[href^='/dth.aspx']").each(function () {

                     var p_td = $(this).parent('td');
                     var SN = $(this).text();
                     if (SN != "nosend")
                     {
                         p_td.html("<button type='button' onclick=\"openeditdialog('" + SN + "');\" class='btn btn-minier btn-success hasbzff'>发给朋友</button>");
                     }
                     else
                     {
                         p_td.html("-----");
                     }
                     
                 });
             }

         }, 500);



         $(document).on('click', "#sh_send", function () {
             dialog_tanchuang_zdy.dialog("close");
             
             $.ajax({
                 type: "GET",
                 url: "/adminht/corepage/dangan/ddzx_zhuce.aspx",
                 dataType: "html",
                 data: "ajaxrun=zhuce&jkname=fs_yqm&sjh=" + $("#sh_shoujihao").val() + "&yqm=" + $("#yqmshow").text() + "&fsr=" + nowloginuser,
                 success: function (msg) {
                     if (msg == "") {
                         bootbox.alert("发送短信完成");
                     }
                     else {
                         
                         bootbox.alert("发送短信失败，"+msg);
                     }
                 },
                 error: function () {
                     alert("调用接口出现意外失败");
                 }

             });
         });

     </script>
         <!-- **********自定义新增脚本******** -->
     <script type="text/javascript">
         var dialog_tanchuang_zdy = null;
         function openeditdialog(SN) {
             $("#yqmshow").html(SN);
             dialog_tanchuang_zdy = $("#dialog-message-test").removeClass('hide').dialog({
                 modal: true,
                 title: "<div class='widget-header widget-header-small'><h4 class='smaller'><i class='ace-icon fa fa-check'></i> 发送邀请码给朋友</h4></div>",
                 width: ($(window).width() > 700 ? "450" : "95%"),
                 buttons: [
                     {
                         text: "  取消  ",
                         "class": "btn btn-xs",
                         click: function () {
                             $(this).dialog("close");
                         }
                     } 
                 ]
             });
         }
     </script>

</asp:Content>

