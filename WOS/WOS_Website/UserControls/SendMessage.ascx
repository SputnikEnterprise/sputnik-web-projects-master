<%@  Control Language="VB" AutoEventWireup="False"
    Inherits="WOS_Website.SendMessage" Codebehind="SendMessage.ascx.vb" %>

<script type="text/javascript" language="javascript">

    var activeEmailAdress = "";
    var activeSubject = "";
    var activeName = "";
    
    function openSendMessageDialog(dlgTitle, emailAdress, subject, name, replyToEmail) {
        var dialogId = '#<% = pnlDialog.ClientID%>';
        // Use this to use postback
        //var dlg = $(dialogId).dialog({ title: dlgTitle, modal: true, width: 430 });
        // dlg.parent().appendTo($("form:first"));
        $(dialogId).dialog({ title: dlgTitle, modal: true, width: 430 });
        activeEmailAdress = emailAdress;
        activeSubject = subject;
        activeName = name;
        
        $('#<% = txtName.ClientID %>').val(name);
        $('#<% = txtReplyToEmail.ClientID %>').val(replyToEmail);
        $('#<% = txtSubject.ClientID %>').val(subject);
    }

    function sendMessage() {
        $.ajax({
            type: "POST",
            url: '<% = ResolveUrl("~") %>WebServices/EmailService.asmx/SendMessageTo',
            cache: false,
            data: "{'ToEmailAdress': '" + activeEmailAdress + "', 'Subject' : '" + activeSubject + "', 'Message' : 'Message from " + $('#<% = txtName.ClientID %>').val() + ": \r\n" + $('#<% = txtMessageBox.ClientID %>').val() + "', 'ReplyToEmail' : '" + $('#<% = txtReplyToEmail.ClientID %>').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            processdata: true,
            success: function (data, textStatus, jqXHR) {
                var sendSuccessful = data.d;
                if (!sendSuccessful) {
                    alert('<% = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_SEND_MESSAGE_ERROR") %>');
                }
                else {
                    var dialogId = '#<% = pnlDialog.ClientID %>';
                    $(dialogId).dialog('close');
                    $('#<% = txtMessageBox.ClientID %>').val('');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('<% = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_SEND_MESSAGE_ERROR") %>');
            }
        });
    }   
   
</script>

<div ID="pnlDialog" style="display: none; width: 430px; " runat="server">
    <div class="formLabel">
        <iwc:Literal runat="server" TextKey="TEXT_SEND_MESSAGE_NAME" />
    </div>
    <div class="formElement">
        <asp:TextBox id="txtName" runat="server" columns="40" />
    </div>
     <div class="formLabel">
        <iwc:Literal runat="server" TextKey="TEXT_SEND_MESSAGE_EMAIL" />
    </div>
    <div class="formElement">
        <asp:TextBox id="txtReplyToEmail" runat="server" columns="40" />
    </div>
    <div class="formLabel">
        <iwc:Literal runat="server" TextKey="TEXT_SEND_MESSAGE_SUBJECT" />
    </div>
    <div class="formElement">
        <asp:TextBox id="txtSubject" runat="server" columns="40" />
    </div>
    <div class="formLabel">
        <iwc:Literal runat="server" TextKey="TEXT_SEND_MESSAGE_CONTENT" />
    </div>
    <div class="formElement">
        <asp:TextBox id="txtMessageBox" runat="server" textmode="Multiline" rows="8" columns="40" />
    </div>
    <iwc:Button ID="btnSendMessage" runat="server" TextKey="TEXT_SEND_MESSAGE_SEND" OnClientClick="javascript: sendMessage()" />
</div>