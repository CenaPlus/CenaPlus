<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Rating.aspx.cs" Inherits="CenaPlus.Cloud.Web.Rating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
        body {
            background: rgb(98, 0, 0);
        }
        #header-wrapper {
            background-color: rgb(98, 0, 0);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Context">
        <div id="header-featured" class="Banner_Frame">
            <div id="banner-wrapper">
                <div id="banner" class="container">
                    <div id="Main">
                        <h2>积分榜</h2>
                        <p>每位选手的能力值是根据每场积分赛排名而累积变化的，每半年为一个赛季，每年的1月1日、8月1日更新赛季数据。</p>
                    </div>
                </div>
            </div>
        </div>
        <div id="wrapper">
            <div id="page" class="container">
                <div style="width:950px;padding-bottom: 100px;margin: auto;">
                    <table style="width:100%">
                        <thead>
                            <tr>
                                <th>排名</th>
                                <th>头像</th>
                                <th>昵称</th>
                                <th>能力值</th>
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
        $("#navRating").addClass("current_page_item");
        $(document).ready(function () {
            $("#Main").hide();
            $("#Main").fadeIn();
            setTimeout(function () {
                $(".Banner_Frame").slideUp("slow");
            }, 3000);
        });
        $(window).bind("beforeunload", function () {
            $("#Main").hide();
            $("#Main").slideDown();
        });
    </script>
</asp:Content>
