<%@ Page Language="VB" AutoEventWireup="false" Inherits="WOS_Website.NotFound" MasterPageFile="~/MasterPage.master" Codebehind="NotFound.aspx.vb" %>

<asp:content id="PageHeader" contentplaceholderid="head" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 28px;
            width: 389px;
        }
    </style>
</asp:content>
<asp:content id="PageContent" contentplaceholderid="ContentPlaceHolder1" runat="Server">
<!-- Content:Start -->
      
    <div style="text-align: left">
        <strong><span style="font-family: Arial">
        <iwc:Literal ID="txtError" TextKey="TEXT_PAGE_NOT_FOUND_ERROR" runat="server" />
        <br />
        <iwc:Literal ID="txtErrorMessage" TextKey="TEXT_PAGE_NOT_FOUND_MESSAGE" runat="server" />
        <br />
        </span></strong>
    </div>
<!-- Content:End -->
</asp:content>
