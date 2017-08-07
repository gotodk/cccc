using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

public partial class topsdk_test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        divshow.InnerHtml = "";
       //阿里大鱼短信接口测试
        ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23618633", "59833b957efba563d7221316c6921ec8");
        AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
        req.Extend = "guid-xxxx";
        req.SmsType = "normal";
        req.SmsFreeSignName = "镜海盟";
        req.SmsParam = "{\"yzm\":\"1234\"}";
        req.RecNum = "18663178263";
        req.SmsTemplateCode = "SMS_44350181";
        AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
        divshow.InnerHtml = rsp.ErrMsg;
    }
}