<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="WOS_Website.JobKandidaten" Codebehind="JobKandidaten.aspx.vb"%>
<%@ Register Src="~/UserControls/TwoColumnTable.ascx" TagName="JobCandidateDetails" TagPrefix="Sputnik" %>
<%@ Register Src="~/UserControls/SendMessage.ascx" TagName="SendMessage" TagPrefix="Sputnik" %>

<asp:Content ID="PageHeader" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">

        // some initializations on document ready
        $(document).ready(function () {
            // [enter]-key in the seachbox
            $(".SearchBox").keypress(function (e) {
                if (e.keyCode == 13) {
                    $(".BtnSearch").click();
                    return false;
                }
            });

            // init jquery tabs on document ready
            $("#tabs").tabs();
        });

        // load detail data asynchronously
        function ShowJobCandidatesDetail(candidateId) {
            PageMethods.LoadJobCandidateDetail(candidateId, onJobCandidateDetailLoadFinished);
        }

        // callback: insert new tab with detail data
        function onJobCandidateDetailLoadFinished(candidateDetail) {

            // Check for valid candidate details.
            if (candidateDetail.length == 0) {
                return;
            }

            // Transform VB StringDictionary to JS Array
            var detailsArray = new Array();
            var tabId = "";
            var maNr = "";
            var profession = "";
            var mail = "";
            for (index in candidateDetail) {
                // Handle Mandatory fields
                if (candidateDetail[index].Key == "id") {
                    tabId = "tabId" + candidateDetail[index].Value;
                }
                else if (candidateDetail[index].Key == "user_email") {
                    mail = candidateDetail[index].Value;
                }
                else if (candidateDetail[index].Key == "manr") {
                    maNr = candidateDetail[index].Value;
                }
                else {
                    if (candidateDetail[index].Key == "ma_beruf") {
                        profession = candidateDetail[index].Value;
                    }
                    detailsArray[candidateDetail[index].Key] = candidateDetail[index].Value;
                }
            }

            if (tabId.length == 0 || profession.length == 0 || mail.length == 0 || maNr.length == 0) {
                // Some of the mandatory fields are not provided.
                return;
            }

            var tab = $("#" + tabId);
            if (tab.length == 0) {
                // Since tab element does not exist, it was not yet opened
                $("div#tabTemplate").clone().attr("id", tabId).appendTo("div#tabs");
                // fill data into the fields
                for (key in detailsArray) {
                    $("#" + tabId + ' .' + key).html(detailsArray[key]);
                }
                var title = '<% = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_SEND_MESSAGE_TITLE") %>';
                var number = '<% = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_SEND_MESSAGE_NUMBER") %>';
                var user_1 = '<% = Session("UserName") %>';
                var strQuery = '<% = Session("strQuery") %>';
                
                var replyToEmail = '<% = Session("UserEmail") %>';
                $("#" + tabId + " .user_email").attr("href", "javascript: openSendMessageDialog('" + title + "','" + mail + "', '" + profession + " (" + number + " " + maNr + ")', '" + user + "', '" + replyToEmail + "');");
                $("#" + tabId + " .printerFriendly").attr("href", "javascript: printWebPart('" + tabId + "', 600, 700);");

                var mafScheinObj = $("#" + tabId + " .mafschein");
                var mafScheinTexts = $("#" + tabId + " .mafschein").text().split('#');
                var newMafScheinText = "";
                for (mafScheinIndex in mafScheinTexts) {
                    if (mafScheinTexts[mafScheinIndex].length != 0) {
                        if (mafScheinIndex != 0) {
                            newMafScheinText += ', ';
                        }
                        newMafScheinText += mafScheinTexts[mafScheinIndex];
                    }
                }
                mafScheinObj.text(newMafScheinText);

                var professionObj = $("#" + tabId + " .ma_beruf");
                var professions = professionObj.text().split(/, /g);
                professionObj.text("");
                for (professionIndex in professions) {
                    $('<span>' + professions[professionIndex] + '</span><br>').appendTo(professionObj);
                }

                var branchObj = $("#" + tabId + " .branches");
                var branches = branchObj.text().split(/, /g);
                branchObj.text("");
                for (branchIndex in branches) {
                    $('<span>' + branches[branchIndex] + '</span><br>').appendTo(branchObj);
                }

                // add tab with close icon
                if (profession.length > 20) {
                    profession = profession.substr(0, 20) + "...";
                }
                $("#tabs").tabs("add", "#" + tabId, profession);
                
                $('a[href$="' + tabId + '"]').parent().append('<span class="ui-icon ui-icon-close">Remove Tab</span>')
                // open created tab
                $("#tabs").tabs("select", tabId);
            }
        }

        // close icon: removing the tab on click
        $("#tabs span.ui-icon-close").live("click", function () {
           var $tabs = $("#tabs")
            var index = $("li", $tabs).index($(this).parent());
            $tabs.tabs("remove", index);
        });
    </script>
</asp:Content>
<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>
    <div id="tabs">
        <ul>
            <li id="list"><a href="#contentWrapper">Liste</a></li>
        </ul>
        <div id="contentWrapper" style="position: relative">
        <asp:Panel ID="pnlNumberOfJobCandidates" runat="server">
            <iwc:Label ID="lblNumberOfJobCandidatesInfo" runat="server" TextKey="TEXT_CUSTOMER_NUMBERS_OF_CANDIDATES" />
            <asp:Label ID="lblNumberOfJobCandidatesInfoAmount" runat="server" />
            <br />
            <br />
        </asp:Panel>
        <div id="serachPanel" style="position: absolute; right: 0px; top: 0px">
            <asp:TextBox ID="txtSearchJob" CssClass="SearchBox" Width="150" runat="server" />
            <iwc:Button ID="btnSearch" CssClass="BtnSearch" runat="server" TextKey="TEXT_GENERAL_BUTTON_SEARCHS" />
        </div>
        <div id="gridWrapper">
            <asp:GridView ID="grid" runat="server" GridLines="None" DataKeyNames="ID" CssClass="grid"
                AutoGenerateColumns="false" AllowPaging="True" PageSize="18">
                <RowStyle CssClass="rowStyle" />
                <FooterStyle CssClass="footerStyle" />
                <PagerStyle CssClass="pagerStyle" />
                <SelectedRowStyle CssClass="selectedRowStyle" />
                <HeaderStyle CssClass="headerStyle" />
                <EditRowStyle CssClass="editRowStyle" />
                <PagerSettings Mode="NumericFirstLast" />
                <AlternatingRowStyle CssClass="alternatingRowStyle" />
                <Columns>
                    <iwc:TemplateField TextKey="TEXT_CANDIDATE_PROFESSION" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60%" HeaderStyle-Width="60%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkJobCandidateDetail" runat="server" />
                        </ItemTemplate>
                    </iwc:TemplateField>
                    <iwc:TemplateField TextKey="TEXT_CANDIDATE_YEAR" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40%" HeaderStyle-Width="40%">
                        <ItemTemplate>
                            <asp:Label ID="lblAgeGroup" runat="server" />
                        </ItemTemplate>
                    </iwc:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
    </div>
    <div id="tabTemplateContainer" style="display: none; position: absolute; top: 10px;left: 0px;">
        <div id="tabTemplate" style="height: 410px;  overflow: auto;">
            <Sputnik:JobCandidateDetails ID="pnlJobCandidateDetail" runat="server" />
        </div>
    </div>
    <Sputnik:SendMessage ID="pnlSendMessage" runat="server" />
</asp:Content>
