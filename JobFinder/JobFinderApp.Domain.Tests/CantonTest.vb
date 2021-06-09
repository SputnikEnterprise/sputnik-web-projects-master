'------------------------------------
' File: CantonTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

<TestClass()>
Public Class CantonTest

    <TestMethod()>
    Public Sub CantonNameWithAbbreviation_CantonNameAndAbbreviationIsSet_CantonNameWithAbbreviationStringIsReturned()

        ' ----------Arrange----------
        Dim cantonFullname = "St. Gallen"
        Dim cantonAbbreviation = "SG"
        Dim canton As New Canton(cantonFullname, cantonAbbreviation)
        Dim expectedResult = String.Format("{0} ({1})", cantonFullname, cantonAbbreviation)

        ' ----------Act----------
        Dim cantonNameWithAbbreviation = canton.CantonNameWithAbbreviation

        ' ----------Assert----------
        Assert.IsTrue(cantonNameWithAbbreviation = expectedResult)

    End Sub

    <TestMethod()>
    Public Sub CantonNameWithAbbreviation_CantonAbbreviationIsNotSet_CantonAbbreviationIsReplacedByHyphenCharacter()

        ' ----------Arrange----------
        Dim cantonFullname = "St. Gallen"
        Dim cantonAbbreviation = String.Empty

        Dim canton As New Canton(cantonFullname, cantonAbbreviation)

        ' Canton abbreviation should be replaced by hyphen character.
        Dim expectedResult = String.Format("{0} (-)", cantonFullname)

        ' ----------Act----------
        Dim cantonNameWithAbbreviation = canton.CantonNameWithAbbreviation

        ' ----------Assert----------
        Assert.IsTrue(cantonNameWithAbbreviation = expectedResult)

    End Sub

    <TestMethod()>
    Public Sub CantonNameWithAbbreviation_CantonNameIsNotSet_CantonNameIsReplacedByHyphenCharacter()

        ' ----------Arrange----------
        Dim cantonFullname = String.Empty
        Dim cantonAbbreviation = "SG"

        Dim canton As New Canton(cantonFullname, cantonAbbreviation)

        ' Canton name should be replaced by hyphen character.
        Dim expectedResult = String.Format("- ({0})", cantonAbbreviation)

        ' ----------Act----------
        Dim cantonNameWithAbbreviation = canton.CantonNameWithAbbreviation

        ' ----------Assert----------
        Assert.IsTrue(cantonNameWithAbbreviation = expectedResult)

    End Sub

End Class
