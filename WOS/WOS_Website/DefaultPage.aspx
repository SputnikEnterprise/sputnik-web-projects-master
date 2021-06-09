<%@ Page Language="VB" AutoEventWireup="false" Inherits="WOS_Website.DefaultPage"
    MasterPageFile="~/MasterPage.master" Codebehind="DefaultPage.aspx.vb" %>

<asp:Content ID="PageHeader" ContentPlaceHolderID="head" runat="Server">
  <title><iwc:Literal TextKey="TEXT_GENERAL_TERMS_AND_CONDITIONS" runat="server" /></title>
</asp:Content>
<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Content:Start -->
    <div style="text-align: left">

        
        <asp:table style="width: inherit; border: none" id="tblTermsAndConditionsToConfirm" runat="server">
            <asp:TableRow>
                <asp:TableCell style="width: 130px; text-align: center"><a ID="btnAGBDownload" href="AGB.ashx?AGB.pdf" runat="server" target="_blank"> <asp:Image ImageUrl="~/images/agb_tou_pdf.png" runat="server" Width="100px" Height="100px" /></a></asp:TableCell>
                <asp:TableCell style="width: 25px"></asp:TableCell>
                <asp:TableCell><strong><iwc:Literal TextKey="TEXT_LOGIN_TERMS_AND_CONDITIONS" runat="Server" id="txtTermsAndConditions" /></strong></asp:TableCell>
                <asp:TableCell style="width: 50px"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow style="height: 25px">
                <asp:TableCell></asp:TableCell>
                <asp:TableCell style="width: 25px"></asp:TableCell>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell style="width: 50px"></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell style="width: 130px;"><iwc:Button style="vertical-align: middle; width: 130px; height: 30px; font-weight: bold" ID="btnAGBAccept" AutoPostBack="true" runat="server" TextKey="TEXT_LOGIN_BUTTON_TERMS_AND_CONDITIONS_CONFIRMIRMATION" /></asp:TableCell>
                <asp:TableCell style="width: 25px"></asp:TableCell>
                <asp:TableCell style="vertical-align: middle"><iwc:Literal TextKey="TEXT_LOGIN_TERMS_AND_CONDITIONS_CONFIRM" runat="Server" id="txtLoginTermsAndConditionsConfirm" /></asp:TableCell>
                <asp:TableCell style="width: 50px"></asp:TableCell>
            </asp:TableRow>
        </asp:table>

        <asp:table style="width: inherit; border: none" id="tblTermsAndConditionsAfterConfirmation" runat="server">
            <asp:TableRow>                
                <asp:TableCell><strong><iwc:Literal TextKey="TEXT_LOGIN_TERMS_AND_CONDITIONS_AFTER_CONFIRMATION" runat="Server" id="Literal2" /></strong></asp:TableCell>
                <asp:TableCell style="width: 50px"></asp:TableCell>
            </asp:TableRow>           
        </asp:table>
        
        
    </div>
    <!-- Content:End -->
</asp:Content>
