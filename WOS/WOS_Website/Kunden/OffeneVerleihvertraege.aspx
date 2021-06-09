<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="WOS_Website.OffeneVerleihvertraege" Codebehind="OffeneVerleihvertraege.aspx.vb" %>

<asp:Content ID="PageHeader" ContentPlaceHolderID="head" Runat="Server">
    <title><iwc:Literal TextKey="TEXT_PAGETITLE_OPEN_RENTAL_CONTRACTS" runat="server" /></title>
</asp:Content>
<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript" language="javascript">

    function ConfirmAction(postBackParameter, showPrintDialog, mail, fax) {
        if(showPrintDialog) {
            jAlert(
                '<%=MessageSignContract %> ' + mail + ', ' + fax,
                '<%=MessageSignContractTitle %>',
                function () { __doPostBack(postBackParameter, '') }
            )
        }
        else {
            __doPostBack(postBackParameter, '');
        }
    }

    function ConfirmAcceptContract(postBackParameter, showPrintDialog, mail, fax) {
        jConfirm(
            '<%=MessageAcceptContract %> ',
            '<%=MessageAcceptContractTitle %> ',
            function (confirmed) { if (confirmed) { ConfirmAction(postBackParameter, showPrintDialog, mail, fax) } }
        );
    }
    function ConfirmDeclineContract(postBackParameter) {
        jConfirm(
            '<%=MessageDeclineContract %> ',
            '<%=MessageDeclineContractTitle %> ', function (confirmed) { if (confirmed) { __doPostBack(postBackParameter, '') } } 
        );
    }
</script>

    <!-- Content:Start -->
    <div>
        <asp:Panel ID="pnlNumberOfDocs" runat="server">
            <iwc:Label ID="lblNumberOfDocsInfo" runat="server" TextKey="TEXT_GENERAL_NUMBERS_OF_DOCUMENTS" />
            <asp:Label ID="lblNumberOfDocsInfoAmount" runat="server" />
            <br />
            <br />
        </asp:Panel>
        <asp:GridView ID="grid" runat="server" GridLines="None"
            DataKeyNames="ID,User_eMail,User_Telefax" AutoGenerateColumns="false" CssClass="grid" AllowPaging="True"
            PageSize="18" OnRowCommand="GridView_RowCommand">
            <RowStyle CssClass="rowStyle" />
            <FooterStyle CssClass="footerStyle" />
            <PagerStyle CssClass="pagerStyle" />
            <SelectedRowStyle CssClass="selectedRowStyle" />
            <HeaderStyle CssClass="headerStyle" />
            <EditRowStyle CssClass="editRowStyle" />
            <AlternatingRowStyle CssClass="alternatingRowStyle" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="pdfIcon" ImageUrl="~/images/PDF_Icon.gif" runat="server"></asp:ImageButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <iwc:BoundField DataField="Doc_info" TextKey="TEXT_CAPTION_DOCUMENT" ReadOnly="True" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-HorizontalAlign="Left" />
                <iwc:BoundField DataField="Transfered_On" TextKey="TEXT_CAPTION_DATE" ReadOnly="True" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="User_eMail" ReadOnly="True" Visible="false" />
                <asp:BoundField DataField="User_Telefax" ReadOnly="True" Visible="false" />
               <asp:TemplateField>
                    <ItemTemplate>
                        <iwc:LinkButton ID="lnkAccept" CommandName="AcceptContract" TextKey="TEXT_DOCUMENT_ACTION_ACCEPT" runat="server" />&nbsp;
                        <iwc:LinkButton ID="lnkDecline" CommandName="DeclineContract" TextKey="TEXT_DOCUMENT_ACTION_DECLINE" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="100%">
                    <ItemTemplate>
                        <!-- Placeholder to fill up the rest of the available space -->
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <!-- Content:End -->
</asp:Content>

