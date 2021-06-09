<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="WOS_Website.Verleihvertraege" Codebehind="Verleihvertraege.aspx.vb" %>

<asp:Content ID="PageHeader" ContentPlaceHolderID="head" runat="Server">
  <title><iwc:Literal TextKey="TEXT_PAGETITLE_RENTAL_CONTRACTS" runat="server" /></title>
</asp:Content>
<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Content:Start -->
    <asp:ScriptManagerProxy ID="scriptManagerProxy" runat="server">
    </asp:ScriptManagerProxy>

    <div>
        <asp:Panel ID="pnlNumberOfDocs" runat="server">
            <iwc:Label ID="lblNumberOfDocsInfo" runat="server" TextKey="TEXT_GENERAL_NUMBERS_OF_DOCUMENTS" />
            <asp:Label ID="lblNumberOfDocsInfoAmount" runat="server" />
            <br />
            <br />
        </asp:Panel>
        <asp:GridView ID="grid" runat="server" GridLines="None" DataKeyNames="ID" CssClass="grid"
            AutoGenerateColumns="false" AllowPaging="True" PageSize="18">
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

                <iwc:TemplateField TextKey="TEXT_CAPTION_ACCEPTED"  HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                       <asp:Label Id="lblAcceptedDate" runat="server" />
                    </ItemTemplate>
                </iwc:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                      <iwc:LinkButton ID="lnkFoward" TextKey="TEXT_DOCUMENT_ACTION_FORWARD" runat="server" />
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
