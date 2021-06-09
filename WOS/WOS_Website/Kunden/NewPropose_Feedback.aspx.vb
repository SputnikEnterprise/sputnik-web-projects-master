'---------------------------------------------------------
' New propose.aspx.vb
'
' © by mf Sputnik Informatik GmbH
'---------------------------------------------------------

Partial Class NewPropose_Feedback
  Inherits CustomerBasePage

#Region "Properties"

#End Region


#Region "Methods"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    lblFeedbackError.Visible = False
    lblFeedbackEmpty.Visible = False

  End Sub


  Protected Sub btnSendFeedback_Click(ByVal sender As Object, ByVal e As EventArgs)

    Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
    Dim FeedbackID As Integer = Session("FeedbackID")
    Dim FeedbackContent As String = txtFeedback.Text

    If FeedbackID > 0 And Not String.IsNullOrEmpty(FeedbackContent) Then

      Try
				ExcecuteSQLNonQuery(String.Format("UPDATE Dbo.[tbl_Customer_WOSDocument_State] SET [Customer_Feedback] = '{0}', [Customer_Feedback_On] = GetDate(), [ViewedResult] = 1, [Viewed_On] = GetDate(), [NotifyAdvisor] = 'true' WHERE ID = (Select Top (1) FK_StateID From Kunden_Doc_Online Where ID = {1}) AND Customer_Feedback Is NULL", FeedbackContent, FeedbackID))

			Catch ex As Exception
				lblFeedbackError.Visible = True
        Response.Write(String.Format("Fehler: StoreFeedback, FeedbackContent: {1} FeedbackID: {2}", ex.Message, FeedbackContent, FeedbackID))

      Finally
        Response.Redirect("~/Kunden/NewPropose.aspx?" & appInfo.ZHDQueryParameterName & "=" & appInfo.CurrentGuidValue)

      End Try

    ElseIf String.IsNullOrEmpty(FeedbackContent) Then

      lblFeedbackEmpty.Visible = True

      End If

  End Sub


  Protected Sub btnCancelFeedback_Click(ByVal sender As Object, ByVal e As EventArgs)

    Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
    Response.Redirect("~/Kunden/NewPropose.aspx?" & appInfo.ZHDQueryParameterName & "=" & appInfo.CurrentGuidValue)

  End Sub

#End Region

End Class




