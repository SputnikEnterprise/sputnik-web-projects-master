<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeBehind="NewPropose_Feedback.aspx.vb" Inherits="WOS_Website.NewPropose_Feedback" %>

<asp:Content ID="PageHeader" ContentPlaceHolderID="head" Runat="Server">
    <title><iwc:Literal TextKey="TEXT_PAGETITLE_OPEN_PROPOSE" runat="server" /></title>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div>
        <asp:Panel ID="pnlEditFeedback" runat="server">
            <iwc:Label ID="lblFeedback" runat="server" TextKey="TEXT_CUSTOMER_PROPPOSE_FEEDBACK_TITLE" Font-Bold="true" />
            <br />
            <br />
            <iwc:Label ID="lblEditFeedback" runat="server" TextKey="TEXT_CUSTOMER_PROPPOSE_FEEDBACK_OF_NUMBER" />
            <br />
            <br />
            <asp:TextBox ID="txtFeedback" CaptionStyle-Font-Names="memo" SkinID="None" runat="server" Native="False" TextMode="MultiLine" Width="650px" Rows="20"></asp:TextBox>
            <div style="margin: 6px 6px 6px auto; align-content:center; ">
                <iwc:Button ID="btnSendFeedback" runat="server" TextKey="TEXT_DOCUMENT_ACTION_SEND_FEEDBACK" style="margin: 6px 6px 6px 0px; padding: 6px" OnClick="btnSendFeedback_Click" />
                <iwc:Button ID="btnCancelFeedback" runat="server" TextKey="TEXT_DOCUMENT_ACTION_CANCEL_FEEDBACK" style="margin: 6px 6px 6px 6px; padding: 6px" OnClick="btnCancelFeedback_Click" />
            </div>
            <br />
            <iwc:Label ID="lblFeedbackError" runat="server" TextKey="TEXT_SEND_FEEDBACK_ERROR" ForeColor="Red" Font-Bold="true" />
            <iwc:Label ID="lblFeedbackEmpty" runat="server" TextKey="TEXT_SEND_FEEDBACK_EMPTY" ForeColor="Red" Font-Bold="true" />
        </asp:Panel>
    </div>

</asp:Content>

