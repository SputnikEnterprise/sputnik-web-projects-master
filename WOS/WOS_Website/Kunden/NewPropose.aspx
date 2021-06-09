<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" Inherits="WOS_Website.NewPropose" Codebehind="NewPropose.aspx.vb" %>

<asp:Content ID="PageHeader" ContentPlaceHolderID="head" Runat="Server">
    <title><iwc:Literal TextKey="TEXT_PAGETITLE_OPEN_PROPOSE" runat="server" /></title>
</asp:Content>

<asp:Content ID="PageContent" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
                        <asp:ImageButton ID="pdfIcon" ImageUrl="~/images/PDF_Icon.gif" runat="server" ></asp:ImageButton>
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
                        <iwc:LinkButton ID="lnkAccept" CommandName="AcceptContract" TextKey="TEXT_DOCUMENT_ACTION_INTERESTING" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <iwc:LinkButton ID="lnkDecline" CommandName="DeclineContract" TextKey="TEXT_DOCUMENT_ACTION_DECLINE" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                        <iwc:LinkButton ID="lnkFeeback" CommandName="ShowFeedback" TextKey="TEXT_DOCUMENT_ACTION_SEND_FEEDBACK_PROPOSE" runat="server" />
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
 

    <!-- Ressources and Scripts -->

    <script type="text/javascript">
        $(document).ready(function () {
            $("a.lightbox").fancybox();
        });
    </script>
    
    <script type="text/javascript">

        function ConfirmAction(postBackParameter, showPrintDialog, mail, fax) {
            if(showPrintDialog) {
                jAlert(
                    '<%= MessageSignPropose %> ' + mail + ', ' + fax,
                    '<%= MessageSignProposeTitle %>',
                    function () { __doPostBack(postBackParameter, '') }
                )
            }
            else {
                __doPostBack(postBackParameter, '');
            }
        }

        function ConfirmAcceptContract(postBackParameter, showPrintDialog, mail, fax) {
            jConfirm(
                '<%= MessageAcceptPropose %> ',
                '<%= MessageAcceptProposeTitle %> ',
                function (confirmed) { if (confirmed) { ConfirmAction(postBackParameter, showPrintDialog, mail, fax) } }
            );
        }

        function ConfirmDeclineContract(postBackParameter) {
            jConfirm(
                '<%= MessageDeclinePropose %> ',
                '<%= MessageDeclineProposeTitle %> ', function (confirmed) { if (confirmed) { __doPostBack(postBackParameter, '') } } 
            );
        }
       
        </script>

</asp:Content>

