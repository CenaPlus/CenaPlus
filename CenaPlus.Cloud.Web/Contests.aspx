<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contests.aspx.cs" Inherits="CenaPlus.Cloud.Web.Contests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
        body {
            background: rgb(24, 82, 0);
        }
        #header-wrapper {
            background-color: rgb(24, 82, 0);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Context">
        <div id="header-featured">
            <div id="banner-wrapper">
                <div id="banner" class="container">
                    <div id="Main">
                        <h2>云比赛</h2>
                        <p>您可以在本页面中查看所有加入云计划的Cena+服务器提供的比赛。</p>
                    </div>
                </div>
            </div>
        </div>
        <div id="wrapper">
            <div id="page" class="container">
                <div style="width:950px;padding-bottom: 100px;margin: auto;">
                    <p style="text-align: left">比赛列表缓存于：<%=CenaPlus.Cloud.Web.Bll.ContestHelper.LastRefreshTime %></p>
                    <table style="width:100%">
                        <thead>
                            <tr>
                                <th>状态</th>
                                <th>比赛名称</th>
                                <th>比赛时间</th>
                                <th>时长</th>
                                <th>赛制</th>
                                <th>服务器</th>
                            </tr>
                        </thead>
                        <tbody id="lstContest">

                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script>
        var page = 0;
        $("#navContests").addClass("current_page_item");
        $(document).ready(function () {
            $("#Main").hide();
            $("#Main").fadeIn();
            LoadContests(page);
            page++;
        });
        function LoadContests(page_index)
        {
            $.post("/Ajax/Contests.GetList.aspx", {
                Page: page_index
            }, function (data) {
                $(data).unbind().find("Contest").each(function () {
                    var Title = $(this).find("Title").text();
                    var BeginTime = $(this).find("BeginTime").text();
                    var Length = $(this).find("Length").text() + " 小时";
                    var Server = $(this).find("Server").text();
                    var Status = $(this).find("Status").text();
                    if (Status == "Live")
                        Status = "<span style='color:red'>" + Status + "</span>";
                    else if (Status == "Pending")
                        Status = "<span style='color:skyblue'>" + Status + "</span>";
                    var Type = $(this).find("Type").text();
                    var Row = "<tr><td>" + Status + "</td><td>" + Title + "</td><td>" + BeginTime + "</td><td>" + Length + "</td><td>" + Type + "</td><td>" + Server + "</td><tr>";
                    $("#lstContest").append(Row);
                });
            });
        }
    </script>
</asp:Content>
