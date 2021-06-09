<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="SPS.VacanciesFields._Default"  EnableViewState="false" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <title>HTML Editor</title>

  <script type="text/javascript" src="tinymce/jscripts/tiny_mce/tiny_mce.js"></script>

    <asp:Literal ID="tinyMCEInitPlaceholder" runat=server />

    <!-- Script functions to expose tinyMCE internals that get called from code using InvokeScript method. -->
    <script type="text/javascript">
      function GetContent() {
        return tinyMCE.get('tinyMceEditor').getContent();
      }

      function SetContent(htmlContent) {
        tinyMCE.get('tinyMceEditor').setContent(htmlContent);
      }
    </script>

</head>

<body>
    <form id="form1" runat="server">
      <asp:Literal ID="tinyMceEditorPlaceholder" runat=server /> 
    </form>
</body>
</html>
