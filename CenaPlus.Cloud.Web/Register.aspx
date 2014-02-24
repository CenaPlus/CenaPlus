<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CenaPlus.Cloud.Web.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <style type="text/css">
        body {
            background: rgb(0, 49, 101);
        }

        #header-wrapper {
            background-color: rgb(0, 49, 101);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="Context">
        <div id="header-featured">
            <div id="banner-wrapper">
                <div id="banner" class="container">
                    <div id="Register_Step_1" class="Register">
                        <h2>服务条款</h2>
                        <p>总则</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;为使用Cena+的服务，您应当阅读并遵守《Cena+服务协议》（以下简称“本协议”）。请您务必审慎阅读、充分理解各条款内容，特别是免除或者限制责任的条款，以及开通或使用某项服务的单独协议。限制、免责条款可能以黑体加粗形式提示您注意。
除非您已阅读并接受本协议所有条款，否则您无权使用Cena+提供的服务。您使用Cena+的服务即视为您已阅读并同意上述协议的约束。如果您未满18周岁，请在法定监护人的陪同下阅读本协议，并特别注意未成年人使用条款。</p>
                        <p>协议的范围</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;本协议是您与Cena+之间关于用户使用Cena+相关服务所订立的协议。“Cena+”是指哈尔滨市精灵软件开发有限责任公司旗下Cena+开发组及其相关服务可能存在的运营关联单位。“用户”是指使用Cena+相关服务的使用人，在本协议中更多地称为“您”。本协议项下的服务是指Cena+向用户提供的包括但不限于评测系统、云题库、云比赛等服务（以下简称“本服务”）。</p>
                        <p>帐号安全</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您注册的帐号为Cena+云帐号，您可以使用该账号登录任何加入Cena+云计划的Cena+服务器，同时该帐号也包括了在云题库服务中进行购买及出售等操作的权限，请您妥善保管好您的帐号及密码，由于帐号遗失而对您造成损失的责任需要您自行承担。</p>
                        <p>用户个人信息保护</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;保护用户的个人信息是Cena+的一项基本原则。您在注册帐号或使用本服务的过程中，可能需要填写一些必要的信息。若国家法律法规有特殊规定的，您需要填写真实的身份信息。若您填写的信息不完整，则无法使用本服务或在使用过程中受到限制。一般情况下，您可随时浏览、修改自己提交的信息，但出于安全性和身份识别（如提现银行卡修改等操作）的考虑，您可能无法修改注册时提供的初始注册信息及其他验证信息。Cena+将运用各种安全技术和程序建立完善的管理制度来保护您的个人信息，以免遭受未经授权的访问、使用或披露。Cena+不会将您的个人信息转移或披露给任何非关联的第三方，除非：相关法律法规或法院、政府机关要求；作为合并、收购、资产转让或类似交易的一部分而转移；或为Cena+提供您要求的服务所必需。</p>
                        <p>按现状提供服务</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您理解并同意，Cena+的服务是按照现有技术和条件所能达到的现状提供的。Cena+会尽最大努力向您提供服务，确保服务的连贯性和安全性；但Cena+不能随时预见和防范法律、技术以及其他风险，包括但不限于不可抗力、病毒、木马、黑客攻击、系统不稳定、第三方服务瑕疵、政府行为等原因可能导致的服务中断、数据丢失以及其他的损失和风险。</p>
                        <p>设备需求</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您应当理解，您使用Cena+的服务需自行准备与相关服务有关的终端设备（如电脑、路由器、服务器等装置），并承担所需的费用（如电话费、上网费等费用）。您理解并同意，您使用本服务时会耗用您的终端设备，使用一切云服务时会消耗您的带宽等资源。</p>
                        <p>广告</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您同意Cena+可以在提供服务的过程中自行或由第三方广告商向您发送广告、推广或宣传信息（包括商业与非商业信息），其方式和范围可不经向您特别通知而变更。Cena+可能为您提供选择关闭广告信息的功能，但任何时候您都不得以本协议未明确约定或Cena+未书面许可的方式屏蔽、过滤广告信息。Cena+依照法律的规定对广告商履行相关义务，您应当自行判断广告信息的真实性并为自己的判断行为负责，除法律明确规定外，您因依该广告信息进行的交易或前述广告商提供的内容而遭受的损失或损害，Cena+不承担责任。您同意，对Cena+服务中出现的广告信息，您应审慎判断其真实性和可靠性，除法律明确规定外，您应对依该广告信息进行的交易负责。</p>
                        <p>收费服务</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cena+的部分服务是以收费方式提供的，如您使用收费服务，请遵守相关的协议。Cena+可能根据实际需要对收费服务的收费标准、方式进行修改和变更，Cena+也可能会对部分免费服务开始收费。前述修改、变更或开始收费前，Cena+将在相应服务页面进行通知或公告。如果您不同意上述修改、变更或付费内容，则应停止使用该服务。</p>
                        <p>云题库</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;云题库和Cena+帐号具有强关联，因此欲使用此项服务必须为Cena+的合法用户，任何一个用户都可以免费使用云题库中的免费资源，同时可以上传免费题目，但欲出售题目时，您需要通过Cena+的实名认证（这个过程需要1~3个工作日）。云题库中的题目只可以在Cena+绑定的五个终端中使用，您不得以任何方式破解获取题目，同时您也不可以将题目进行描述性变更出售他人或提供他人。在云题库中购买的题目，您只具有使用权，只允许在本地进行查看及描述性的修改，提供本地环境下的Cena+客户端使用。题目费用Cena+使用人民币结算，在您成功出售题目时，Cena+会收取一定的费用作为服务费，具体数额详见相关服务的页面。</p>
                        <p>知识产权声明</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cena+在本服务中提供的内容（包括但不限于程序、题目、网页、图片、文字等）的知识产权归Cena+所有，用户在使用本服务中所产生的内容的知识产权归用户或相关权利人所有。除另有特别声明外，Cena+提供本服务时所依托软件的著作权、专利权及其他知识产权均归Cena+所有。Cena+在本服务中所使用的“Cena+”、“Smart software”及字母C形象等商业标识，其著作权或商标权归Cena+所有。上述及其他任何本服务包含的内容的知识产权均受到法律保护，未经Cena+、用户或相关权利人书面许可，任何人不得以任何形式进行使用或创造相关衍生作品。</p>
                        <p>用户违法行为</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;用户不得使用本服务从事违法行为，同时不得通过本服务提供的相关产品中可能存在的漏洞对Cena+整体服务造成危害，Cena+保留对违反协议的用户提起诉讼的权利，同时Cena+有权在您违反本服务时终止对您提供服务，我们会将您帐户中的预付费余额返还给您注册时填写的银行卡信息中，如果没有填写银行卡信息则需您自行与Cena+客服取得联系，如果当月有未出帐收入，则未出帐部分收入作废。</p>
                        <p>未成年人使用条款</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;若用户年龄未超过18周岁，则为未成年人，未成年人应在监护人监护、指导下阅读本服务条款。若未成年人违反本条款，其监护人需连带承担相关责任。</p>
                        <p>管辖与法律适用</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;本协议签订地为中华人民共和国广东省深圳市南山区。本协议的成立、生效、履行、解释及纠纷解决，适用中华人民共和国大陆地区法律（不包括冲突法）。若您和Cena+之间发生任何纠纷或争议，首先应友好协商解决；协商不成的，您同意将纠纷或争议提交本协议签订地有管辖权的人民法院管辖。本协议所有条款的标题仅为阅读方便，本身并无实际涵义，不能作为本协议涵义解释的依据。本协议条款无论因何种原因部分无效或不可执行，其余条款仍有效，对双方具有约束力。</p>
                        <p>协议的生效与变更</p>
                        <p style="text-align: left">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;您使用Cena+的服务即视为您已阅读本协议并接受本协议的约束。Cena+有权在必要时修改本协议条款。您可以在相关服务页面查阅最新版本的协议条款。本协议条款变更后，如果您继续使用Cena+提供的软件或服务，即视为您已接受修改后的协议。如果您不接受修改后的协议，应当停止使用Cena+提供的软件或服务。</p>
                        <a href="javascript: void(0);" class="button" id="To_Step_2">接受服务条款</a>
                    </div>
                    <div id="Register_Step_2" class="Register">
                        <h2>开始注册</h2>
                        <p>在注册前，我们需要进行电子邮箱地址的验证</p>
                        <p>
                            <input id="Email" type="text" style="height: 50px; width: 480px; font-size: 36px; text-align: center;" />
                        </p>
                        <a href="javascript: void(0);" class="button" id="To_Step_3">下一步
                        </a>
                    </div>
                    <div id="Register_Step_3" class="Register">
                        <h2>验证您的邮箱</h2>
                        <p>我们已经向您刚刚填写的电子邮箱中发送了一封确认信，请将确认信中的验证码填写到下面的文本框中</p>
                        <p>
                            <input id="EmailCode" type="text" style="height: 50px; width: 480px; font-size: 36px; text-align: center;" />
                        </p>
                        <a href="javascript: void(0);" class="button" id="To_Step_4">下一步
                        </a>
                    </div>
                    <div id="Register_Step_4" class="Register">
                        <h2>填写您的信息</h2>
                        <p>用户名称</p>
                        <p>
                            <input type="text" id="Username" style="height: 50px; width: 480px; font-size: 36px; text-align: center;" />
                        </p>
                        <p>密码</p>
                        <p>
                            <input type="password" id="Password" style="height: 50px; width: 480px; font-size: 36px; text-align: center;" />
                        </p>
                        <p>密码重复</p>
                        <p>
                            <input type="password" id="Confirm" style="height: 50px; width: 480px; font-size: 36px; text-align: center;" />
                        </p>
                        <a href="javascript: void(0);" class="button" id="To_Finish">完成注册
                        </a>
                    </div>
                    <div id="Info" class="Register">
                        <h2 id="Info_Header">填写您的信息</h2>
                        <p id="Info_Content">用户名称</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        function checkemail(str) {
            var sReg = /[_a-zA-Z\d\-\.]+@[_a-zA-Z\d\-]+(\.[_a-zA-Z\d\-]+)+$/;
            if (!sReg.test(str)) {
                return false;
            }
            return true;
        }
        $("#navRegister").addClass("current_page_item");
        function CastInfo(header, content)
        {
            $(".Register").hide();
            $("#Info_Header").html(header);
            $("#Info_Content").html(content);
            $("#Info").fadeIn();
        }
        $(document).ready(function () {
            $(".Register").hide();
            $("#Register_Step_1").fadeIn();
        });
        $("#To_Step_2").click(function () {
            $(".Register").hide();
            $("#Register_Step_2").fadeIn();
        });
        $("#To_Step_3").click(function () {
            if (!checkemail($("#Email").val()))
            {
                CastInfo("错误", "您填写的Email地址不合法，请重新尝试。");
                setTimeout(function () {
                    $(".Register").hide();
                    $("#Register_Step_2").fadeIn();
                }, 3000);
                return;
            }
            CastInfo("请稍候...", "");
            $.post("/Ajax/Register.SendMail.aspx", {
                Email : $("#Email").val()
            }, function (feedback) {
                if (feedback == "Failed")
                {
                    CastInfo("错误", "您在短时间内尝试了过多次数的帐号注册操作");
                }
                else if (feedback == "Existed")
                {
                    CastInfo("错误", "您输入的电子邮箱已经注册过Cena+了");
                }
                else if (feedback == "WrongAddress") {
                    CastInfo("错误", "您输入的Email地址不合法，可能是您使用GMail中的'+'地址或使用临时邮箱导致的");
                }
                else {
                    $(".Register").hide();
                    $("#Register_Step_3").fadeIn();
                }
            });
        });
        $("#To_Step_4").click(function () {
            CastInfo("请稍候...", "");
            $.post("/Ajax/Register.EmailAuthentication.aspx", {
                Code: $("#EmailCode").val()
            }, function (data) {
                if (data == "OK") {
                    $(".Register").hide();
                    $("#Register_Step_4").fadeIn();
                }
                else {
                    CastInfo("错误", "电子邮箱验证失败，请使用正确的邮箱地址再次尝试。");
                    setTimeout(function () {
                        $(".Register").hide();
                        $("#Register_Step_3").fadeIn();
                    }, 3000);
                }
            });
            $("#To_Finish").click(function () {
                if ($("#Password").val() != $("#Confirm").val()) {
                    CastInfo("错误", "密码与密码重复不一致，请返回修改！");
                    setTimeout(function () {
                        $(".Register").hide();
                        $("#Register_Step_4").fadeIn();
                    }, 3000);
                    return;
                }
                $.post("/Ajax/Register.Finish.aspx", {
                    Username: $("#Username").val(),
                    Password: $("#Password").val()
                }, function (data) {
                    if (data == "OK") {
                        CastInfo("谢谢您", "感谢您注册成为Cena+会员，请点击右上方的登录以进行更多操作。");
                    }
                    else if (data == "Error") {
                        CastInfo("错误", "注册过程中发生错误，请重试！");
                    }
                    else if (data == "Existed") {
                        CastInfo("错误", "您注册的用户名已经存在，请返回重试！");
                        setTimeout(function () {
                            $(".Register").hide();
                            $("#Register_Step_4").fadeIn();
                        }, 3000);
                    }
                });
            });
        });
    </script>
</asp:Content>
