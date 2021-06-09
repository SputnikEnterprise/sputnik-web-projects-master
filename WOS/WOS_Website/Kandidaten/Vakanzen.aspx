<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="WOS_Website.Vakanzen" Codebehind="Vakanzen.aspx.vb" %>
<%@ Register Src="~/UserControls/TwoColumnTable.ascx" TagName="VakanzDetails" TagPrefix="Sputnik" %>
<%@ Register Src="~/UserControls/SendMessage.ascx" TagName="SendMessage" TagPrefix="Sputnik" %>

<asp:Content ID="PageHeader" ContentPlaceHolderID="head" runat="Server">
    <style>
        #tabs
        {
            margin-top: 1em;
        }
        #tabs li .ui-icon-close
        {
            float: left;
            margin: 0.4em 0.2em 0 0;
            cursor: pointer;
        }
    </style>
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

        // load detail data asynchronous
        function ShowVacancyDetail(vacancyId, customerId) {
            PageMethods.LoadVacancyDetail(vacancyId, customerId, onVacancyDetailLoadFinished);
        }

        // callback: insert new tab with detail data
        function onVacancyDetailLoadFinished(vacancyDetail) {

            // Check for valid vacancy details.
            if (vacancyDetail.length == 0) {
                return;
            }

            // Transform VB StringDictionary to JS Array
            var detailsArray = new Array();
            var tabId = "";
            var description = "";
            var mail = "";
            var vakNr = "";
            for (index in vacancyDetail) {
                // Handle Mandatory fields
                if (vacancyDetail[index].Key == "id") {
                    tabId = "tabId" + vacancyDetail[index].Value;
                }
                else if (vacancyDetail[index].Key == "user_email") {
                    mail = vacancyDetail[index].Value;
                }                
                else {
                    if (vacancyDetail[index].Key == "bezeichnung") {
                        description = vacancyDetail[index].Value;
                    }
                    else if (vacancyDetail[index].Key == "vaknr") {
                        vakNr = vacancyDetail[index].Value;
                    }
                    detailsArray[vacancyDetail[index].Key] = vacancyDetail[index].Value;
                }
            }

            if (tabId.length == 0 || description.length == 0 || mail.length == 0 || vakNr.length == 0) {
                // Some of the mandatory fields are not provided.
                return;
            }

            var tab = $("#" + tabId);
            if (tab.length == 0) {
                // Since tab element does not exist, it was not yet opened
                $("div#tabTemplate").clone().attr("id", tabId).appendTo("div#tabs");
                tabId = "#" + tabId;
                // fill data into the fields
                $(tabId + " .vaknr").html(vakNr);
                for (key in detailsArray) {
                    $(tabId + ' .' + key).html(detailsArray[key]);
                }

                var title = '<% = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_SEND_MESSAGE_TITLE") %>';
                var number = '<% = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_SEND_MESSAGE_NUMBER") %>';
                var user = '<% = Session("UserName") %>';
                var replyToEmail = '<% = Session("UserEmail") %>';

                $(tabId + " .user_email").attr("href", "javascript: openSendMessageDialog('" + title + "','" + mail + "', '" + description + " (" + number + " " + vakNr + ")', '" + user + "', '" + replyToEmail + "');");
                $(tabId + " .printerFriendly").attr("href", "javascript: printWebPart('" + tabId.substr(1) + "', 700, 800);");
                // add tab with close icon
                if (description.length > 20) {
                    description = description.substr(0, 20) + "...";
                }
                $("#tabs").tabs("add", tabId, description);
                $('a[href$="' + tabId + '"]').parent().append('<span class="ui-icon ui-icon-close">Remove Tab</span>');
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
            <asp:Panel ID="pnlNumberOfVacancies" runat="server">
                <iwc:Label ID="lblNumberOfVacanciesInfo" runat="server" TextKey="TEXT_CANDIDATES_NUMBERS_OF_VACANCIES" />
                <asp:Label ID="lblNumberOfVacanciesInfoAmount" runat="server" />
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
                        <iwc:TemplateField TextKey="TEXT_CAPTION_DESCRIPTION" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" ItemStyle-Width="60%" HeaderStyle-Width="60%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkVacancyDetail" runat="server"/>
                            </ItemTemplate>
                        </iwc:TemplateField>
                        <iwc:BoundField DataField="JobOrt" TextKey="TEXT_ADDRESS_LOCATION" ReadOnly="True" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%" HeaderStyle-Width="30%" />
                        <iwc:BoundField DataField="Vak_Kanton" TextKey="TEXT_ADDRESS_CANTON" ReadOnly="True" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="10%" HeaderStyle-Width="10%" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </div>
    <div style="display: none; position: absolute; top: 10px;left: 0px;">
        <div id="tabTemplate" style="height: 410px;  overflow: auto;">
            <Sputnik:VakanzDetails ID="pnlVacancyDetail" runat="server" />
        </div>
    </div>
    <Sputnik:SendMessage ID="pnlSendMessage" runat="server" />
</asp:Content>
