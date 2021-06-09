'------------------------------------
' File: DataFormatterManager.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Stores the available formatter objects.
''' </summary>
Public Class DataFormatterManager

    '--Fields--

    Private m_DataFormatters As New Dictionary(Of String, DataFormatterAbs)

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()

        ' Formatters for general data tables
        m_DataFormatters.Add("GeneralDataTable2Formatter", New GeneralDataTable2Formatter)
        m_DataFormatters.Add("GeneralDataTable3Formatter", New GeneralDataTable3Formatter)
        m_DataFormatters.Add("GeneralDataTable4Formatter", New GeneralDataTable4Formatter)
        m_DataFormatters.Add("GeneralDataTable5Formatter", New GeneralDataTable5Formatter)
        m_DataFormatters.Add("GeneralDataTable6Formatter", New GeneralDataTable6Formatter)

        ' Formatters for assignment of personnel tables
        m_DataFormatters.Add("AssignmentOfPersonnelTable1Formatter", New AssignmentOfPersonnelTable1Formatter)
        m_DataFormatters.Add("AssignmentOfPersonnelTable2Formatter", New AssignmentOfPersonnelTable2Formatter)
        m_DataFormatters.Add("AssignmentOfPersonnelTable3Formatter", New AssignmentOfPersonnelTable3Formatter)
        m_DataFormatters.Add("AssignmentOfPersonnelTable4Formatter", New AssignmentOfPersonnelTable4Formatter)
        m_DataFormatters.Add("AssignmentOfPersonnelTable5Formatter", New AssignmentOfPersonnelTable5Formatter)
        m_DataFormatters.Add("AssignmentOfPersonnelTable6Formatter", New AssignmentOfPersonnelTable6Formatter)

        ' Formatters for sales figures tables
        m_DataFormatters.Add("SalesFiguresTable1Formatter", New SalesFiguresTable1Formatter)
        m_DataFormatters.Add("SalesFiguresTable2Formatter", New SalesFiguresTable2Formatter)
        m_DataFormatters.Add("SalesFiguresTable3Formatter", New SalesFiguresTable3Formatter)
        m_DataFormatters.Add("SalesFiguresTable4Formatter", New SalesFiguresTable4Formatter)
        m_DataFormatters.Add("SalesFiguresTable5Formatter", New SalesFiguresTable5Formatter)
        m_DataFormatters.Add("SalesFiguresTable6Formatter", New SalesFiguresTable6Formatter)

        ' Formatters for candidates data tables
        m_DataFormatters.Add("CandidatesDataTable1Formatter", New CandidatesDataTable1Formatter)
        m_DataFormatters.Add("CandidatesDataTable2Formatter", New CandidatesDataTable2Formatter)
        m_DataFormatters.Add("CandidatesDataTable3Formatter", New CandidatesDataTable3Formatter)
        m_DataFormatters.Add("CandidatesDataTable4Formatter", New CandidatesDataTable4Formatter)
        m_DataFormatters.Add("CandidatesDataTable5Formatter", New CandidatesDataTable5Formatter)
        m_DataFormatters.Add("CandidatesDataTable6Formatter", New CandidatesDataTable6Formatter)

        ' Formatters for customer data tables
        m_DataFormatters.Add("CustomerDataTable1Formatter", New CustomerDataTable1Formatter)
        m_DataFormatters.Add("CustomerDataTable2Formatter", New CustomerDataTable2Formatter)
        m_DataFormatters.Add("CustomerDataTable3Formatter", New CustomerDataTable3Formatter)
        m_DataFormatters.Add("CustomerDataTable4Formatter", New CustomerDataTable4Formatter)

    End Sub

    ''' <summary>
    ''' Gets a data formatter by its name.
    ''' </summary>
    Public Function GetDataFormatterByName(ByVal dataFormatterName As String) As DataFormatterAbs

        If Not (m_DataFormatters.ContainsKey(dataFormatterName)) Then
            Return Nothing
        End If

        Return m_DataFormatters(dataFormatterName)

    End Function

End Class
