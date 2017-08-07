using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// NeteaseMessage 的摘要说明
/// </summary>
public class NeteaseMessage
{
    public NeteaseMessage()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    static string url = "https://api.netease.im/sms/sendcode.action";
    static string urlv = "https://api.netease.im/sms/verifycode.action";

    static string appKey = "aab808390eb8fea7e7627f6919961ad0";
    static string appSecret = "f0f048f4543e";

    string ContentType = "application/x-www-form-urlencoded;charset=utf-8";

    //发送验证码
    public string SendCode(string mobile, string deviceId)
    {
        int templateid = 0;
        int codeLen = 6;
        //生成6位随机数
        string nonce = new Random().Next(100000, 999999).ToString();
        //系统当前的UTC时间戳
        string curTime = DateTimeToStamp(DateTime.Now).ToString();

        //三个参数拼接的字符串进行sha1哈希计算的草的16进制字符串，有效期为5分钟
        string checkSum = getCheckSum(appSecret, nonce, curTime);

        //拼接发送消息的头部
        string post = "mobile=" + mobile + "&templateid=" + templateid;

        if (!string.IsNullOrEmpty(deviceId))
        {
            post += "&deviceId=" + deviceId;
        }

        post = post + "&codeLen=" + codeLen;


        byte[] btBodys = Encoding.UTF8.GetBytes(post);

        System.Net.WebRequest wReq = System.Net.WebRequest.Create(url);
        wReq.Method = "POST";
        wReq.Headers.Add("AppKey", appKey);
        wReq.Headers.Add("Nonce", nonce);
        wReq.Headers.Add("CurTime", curTime);
        wReq.Headers.Add("CheckSum", checkSum);
        wReq.ContentLength = btBodys.Length;
        wReq.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

        using (var wsr = wReq.GetRequestStream())
        {
            wsr.Write(btBodys, 0, btBodys.Length);
        }

        System.Net.WebResponse wResp = wReq.GetResponse();
        System.IO.Stream respStream = wResp.GetResponseStream();

        string result;
        using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, System.Text.Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        //Json数据，obj是网易生成的验证码
        return result;


    }
    //校验验证码
    public string VerifyCode(string mobile, string code)
    {
        string nonce = new Random().Next(100000, 999999).ToString();
        string curTime = DateTimeToStamp(DateTime.Now).ToString();
        //string checkSum = SHA1_Hash(appSecret + nonce + curTime);
        string checkSum = getCheckSum(appSecret, nonce, curTime);

        string post = "mobile=" + mobile + "&code=" + code;

        byte[] btBodys = Encoding.UTF8.GetBytes(post);

        System.Net.WebRequest wReq = System.Net.WebRequest.Create(urlv);
        wReq.Method = "POST";
        wReq.Headers.Add("AppKey", appKey);
        wReq.Headers.Add("Nonce", nonce);
        wReq.Headers.Add("CurTime", curTime);
        wReq.Headers.Add("CheckSum", checkSum);
        wReq.ContentLength = btBodys.Length;
        wReq.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

        using (var wsr = wReq.GetRequestStream())
        {
            wsr.Write(btBodys, 0, btBodys.Length);
        }

        System.Net.WebResponse wResp = wReq.GetResponse();
        System.IO.Stream respStream = wResp.GetResponseStream();

        string result;
        using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, System.Text.Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        //Json数据
        return result;
    }

    //// 计算并获取CheckSum,方式一
    private static string SHA1_Hash(string str_sha1_in)
    {
        SHA1 sha1 = new SHA1CryptoServiceProvider();
        byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(str_sha1_in);
        byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
        string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
        str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
        return str_sha1_out;
    }


    // 计算并获取CheckSum  方式二
    public static String getCheckSum(String appSecret, String nonce, String curTime)
    {
        byte[] data = Encoding.Default.GetBytes(appSecret + nonce + curTime);
        byte[] result;

        SHA1 sha = new SHA1CryptoServiceProvider();
        // This is one implementation of the abstract class SHA1.  
        result = sha.ComputeHash(data);

        return getFormattedText(result);
    }

    // 计算并获取md5值  
    public static String getMD5(String requestBody)
    {
        if (requestBody == null)
            return null;

        // Create a new instance of the MD5CryptoServiceProvider object.  
        MD5 md5Hasher = MD5.Create();

        // Convert the input string to a byte array and compute the hash.  
        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(requestBody));

        // Create a new Stringbuilder to collect the bytes  
        // and create a string.  
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data   
        // and format each one as a hexadecimal string.  
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.  
        return getFormattedText(Encoding.Default.GetBytes(sBuilder.ToString()));
    }

    private static string getFormattedText(byte[] bytes)
    {
        int len = bytes.Length;
        StringBuilder buf = new StringBuilder(len * 2);
        for (int j = 0; j < len; j++)
        {
            buf.Append(HEX_DIGITS[(bytes[j] >> 4) & 0x0f]);
            buf.Append(HEX_DIGITS[bytes[j] & 0x0f]);
        }
        return buf.ToString();
    }

    private static char[] HEX_DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

    // 时间戳转为C#格式时间
    private static DateTime StampToDateTime(string timeStamp)
    {
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);

        return dateTimeStart.Add(toNow);
    }

    // DateTime时间格式转换为Unix时间戳格式
    private static int DateTimeToStamp(System.DateTime time)
    {
        //TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
        //Int32 ticks = System.Convert.ToInt32(ts.TotalSeconds);

        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;

    }
}


