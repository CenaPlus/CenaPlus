<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CenaPlus.Cloud.Web.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
        body {
            background: rgb(135, 56, 0);
        }

        #header-wrapper {
            background-color: rgb(135, 56, 0);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Context">
        <div id="header-featured">
            <div id="banner-wrapper">
                <div id="banner" class="container">
                    <div class="Login" id="Login_Main">
                        <h2>登录</h2>
                        <p>用户名</p>
                        <p>
                            <input id="Username" type="text" style="height: 50px; width: 480px; font-size: 36px; text-align: center;" /></p>
                        <p>密码</p>
                        <p>
                            <input id="Password" type="password" style="height: 50px; width: 480px; font-size: 36px; text-align: center;" /></p>
                        <a href="javascript: void(0);" class="button" id="BtnLogin">登录</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="/Forgot" class="button">找回密码</a>
                    </div>
                    <div class="Login" id="Info">
                        <h2 id="Info_Header"></h2>
                        <p id="Info_Content"></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function CastInfo(header, content) {
            $(".Login").hide();
            $("#Info_Header").html(header);
            $("#Info_Content").html(content);
            $("#Info").fadeIn();
        }
        $("#navLogin").addClass("current_page_item");

        $(document).ready(function () {
            $(".Login").hide();
            $("#Login_Main").fadeIn();
        });
        $("#BtnLogin").click(function () {
            CastInfo("请稍候...", "");
            $.post("/Ajax/Login.aspx", {
                Username: $("#Username").val(),
                Password: $("#Password").val()
            }, function (data) {
                if (data == "Error") {
                    CastInfo("错误", "用户名或密码错误!");
                    setTimeout(function () {
                        $(".Login").hide();
                        $("#Login_Main").fadeIn();
                    }, 3000);
                    return;
                }
                else if (data == "Forbidden") {
                    CastInfo("错误", "您在短时间内尝试登录失败次数过多，系统暂时禁止您执行登录操作！");
                    return;
                }
                else {
                    CastInfo("登录成功", "浏览器将在3秒后跳转至之前访问的页面。");
                    setTimeout(function () {
                        self.location = "<%=Request.UrlReferrer==null?"/":Request.UrlReferrer.ToString()%>";
                    }, 3000);
                }
            });
        });
    </script>
</asp:Content>
