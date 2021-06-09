
Namespace DataTransferObject.PVLInfo.DataObjects


	<Serializable()>
	Public Class GAVPVLArchiveDatabaseDTO

		Public Property ID As Integer
		Public Property DbName As String
		Public Property DbConnstring As String

	End Class

	<Serializable()>
	Public Class GAVVersionDataDTO

		Public Property ID As Integer
		Public Property GAVNumber As Integer?
		Public Property GAVDate As DateTime?
		Public Property GAVInfo As String
		Public Property schema_version As String

	End Class


	<Serializable()>
	Public Class GAVAddressDataDTO

		Public Property ID As Integer
		Public Property GAVNumber As Integer?
		Public Property BerufBez As String
		Public Property GAV_Name As String
		Public Property GAV_ZHD As String
		Public Property GAV_Postfach As String
		Public Property GAV_Strasse As String
		Public Property GAV_PLZ As String
		Public Property GAV_Ort As String
		Public Property GAV_AdressNr As String
		Public Property GAV_Bank As String
		Public Property GAV_BankPLZOrt As String
		Public Property GAV_Bankkonto As String
		Public Property GAV_IBAN As String
		Public Property Kanton As String
		Public Property Organ As String

	End Class

	<Serializable()>
	Public Class PVLPublicationViewDataDTO
		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property User_ID As String
		Public Property ContractNumber As String
		Public Property VersionNumber As Integer?
		Public Property PublicationDate As DateTime?
		Public Property Title As String
		Public Property Content As String
		Public Property Viewed As Boolean?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class

	<Serializable()>
	Public Class GAVNameResultDTO

		Public Property id_meta As Integer
		Public Property gav_number As Integer
		Public Property ave As Boolean
		Public Property name_de As String
		Public Property name_fr As String
		Public Property name_it As String
		Public Property publication_date As DateTime?
		Public Property schema_version As String

		Public Property stdweek As Decimal?
		Public Property stdmonth As Decimal?
		Public Property stdyear As Decimal?
		Public Property fan As Decimal?
		Public Property fag As Decimal?
		Public Property resor_fan As Decimal?
		Public Property resor_fag As Decimal?
		Public Property van As Decimal?
		Public Property vag As Decimal?

		Public Property wan As Decimal?
		Public Property wag As Decimal?

		Public Property old_fag As Decimal?
		Public Property old_fan As Decimal?
		Public Property old_wag As Decimal?
		Public Property old_wan As Decimal?
		Public Property old_WAG_s As Decimal?
		Public Property old_WAN_s As Decimal?
		Public Property old_WAG_J As Decimal?
		Public Property old_WAN_J As Decimal?
		Public Property old_vag As Decimal?
		Public Property old_van As Decimal?
		Public Property old_VAG_s As Decimal?
		Public Property old_VAN_s As Decimal?
		Public Property old_VAG_J As Decimal?
		Public Property old_VAN_J As Decimal?
		Public Property PVL_Edition As Integer
		Public Property Created As DateTime?



		Public Property ave_validity_start As DateTime?
		Public Property ave_validity_end As DateTime?
		Public Property unia_validity_start As DateTime?
		Public Property unia_validity_end As DateTime?
		Public Property validity_start_date As DateTime?
		Public Property State As Integer
		Public Property Version As String
		Public Property GAVKanton As String
		Public Property ID_Calculator As Integer


		Public Property currdbname As String


	End Class


	<Serializable()>
	Public Class GAVCriteriasResultDTO
		Public Property ID_Criterion As Integer
		Public Property ID_Contract As Integer
		Public Property Element_ID As Integer
		Public Property name_de As String
		Public Property name_fr As String
		Public Property name_it As String

	End Class

	<Serializable()>
	Public Class GAVCriteriaValueResultDTO
		Public Property ID_Criterion As Integer
		Public Property ID_CriterionValue As Integer
		Public Property txtText As String
		Public Property txtTable As String

	End Class

	<Serializable()>
	Public Class GAVTaxonomyDTO
		Public Property ID_Taxonomy_Entry As Integer
		Public Property ID_Taxonomy As Integer
		Public Property Value As String

	End Class


	<Serializable()>
	Public Class GAVCalculationDTO

		Public Property ID_Calculation As Integer
		Public Property ID_Calculator As Integer
		Public Property basic_hourly_wage As String
		Public Property vacation_pay As String
		Public Property holiday_compensation As String
		Public Property compensation_13th_month_salary As String
		Public Property number_of_holidays As String
		Public Property monthly_wage As String
		Public Property number_of_vacation_days As String
		Public Property gross_hourly_wage As String
		Public Property percentage_vacation_pay As String
		Public Property percentage_holiday_compensation As String
		Public Property percentage_13th_month_salary As String
		Public Property calculation_vacation_pay As String
		Public Property calculation_holiday_compensation As String
		Public Property calculation_13th_month_salary As String
		Public Property has_13th_month_salary_compensation As Boolean
		Public Property sortPosition As Integer
		Public Property percentage_far_an As String
		Public Property percentage_far_ag As String
		Public Property calculation_far As String
		Public Property far_bvg_relevant As Boolean
		Public Property ID_AlternativeText As Integer

	End Class

	<Serializable()>
	Public Class GAVCategoryValueDTO
		Public Property ID_CategoryValue As Integer
		Public Property ID_Category As Integer
		Public Property ID_BaseCategory As Integer?
		Public Property Text_De As String

	End Class


	<Serializable()>
	Public Class GAVCategoryDTO

		Public Property ID_Category As Integer
		Public Property ID_Calculator As Integer
		Public Property ID_BaseCategory As Integer
		Public Property name_de As String
		Public Property name_fr As String
		Public Property name_it As String

	End Class

	<Serializable()>
	Public Class GAVNotificationDTO

		Public Property Id As Integer
		Public Property ID_CategoryValue As Integer
		Public Property gav_number As Integer
		Public Property Info As String

	End Class


	<Serializable()>
	Public Class FLGAVGruppe0ResultDTO
		Public Property GAVNumber As Integer
		Public Property Gruppe0Label As String

	End Class

	<Serializable()>
	Public Class FLGAVGruppe1ResultDTO
		Public Property Gruppe1Label As String

	End Class

	<Serializable()>
	Public Class FLGAVGruppe2ResultDTO
		Public Property Gruppe2Label As String

	End Class

	<Serializable()>
	Public Class FLGAVGruppe3ResultDTO
		Public Property Gruppe3Label As String

	End Class

	<Serializable()>
	Public Class FLGAVTextResultDTO
		Public Property GAVLabel As String

	End Class

	<Serializable()>
	Public Class FLGAVSalaryResultDTO
		Public Property ID As Integer
		Public Property GAVNr As Integer
		Public Property GAVLabel As String
		Public Property GavKanton As String
		Public Property Gruppe0 As String
		Public Property Gruppe1 As String
		Public Property Gruppe2 As String
		Public Property Gruppe3 As String
		Public Property GavText As String
		Public Property CalcFerien As Integer
		Public Property Calc13Lohn As Integer
		Public Property Minlohn As Decimal
		Public Property FeiertagLohn As Decimal
		Public Property Feierbtr As Decimal
		Public Property FerienLohn As Decimal
		Public Property Ferienbtr As Decimal
		Public Property Lohn13 As Decimal
		Public Property Lohn13btr As Decimal
		Public Property StdLohn As Decimal
		Public Property Monatslohn As Decimal
		Public Property Mittagszulagen As Decimal
		Public Property FAG As Decimal
		Public Property FAN As Decimal
		Public Property WAG As Decimal
		Public Property WAN As Decimal
		Public Property VAG As Decimal
		Public Property VAN As Decimal
		Public Property FAG_S As Decimal
		Public Property FAN_S As Decimal
		Public Property WAG_S As Decimal
		Public Property WAN_S As Decimal
		Public Property VAG_S As Decimal
		Public Property VAN_S As Decimal
		Public Property FAG_M As Decimal
		Public Property FAN_M As Decimal
		Public Property WAG_M As Decimal
		Public Property WAN_M As Decimal
		Public Property VAG_M As Decimal
		Public Property VAN_M As Decimal
		Public Property FAG_J As Decimal
		Public Property FAN_J As Decimal
		Public Property WAG_J As Decimal
		Public Property WAN_J As Decimal
		Public Property VAG_J As Decimal
		Public Property VAN_J As Decimal
		Public Property GueltigAb As DateTime?
		Public Property GueltigBis As DateTime?
		Public Property ZusatzFeier As String
		Public Property Zusatz13Lohn As String
		Public Property Ferientext as String
		Public Property Lohn13text As String
		Public Property StdWeek As Integer
		Public Property StdMonth As Integer
		Public Property StdYear As Integer
		Public Property F_Alter as String
		Public Property L_Alter As String
		Public Property Zusatz1 as String
		Public Property Zusatz2 As String
		Public Property Zusatz3 as String
		Public Property Zusatz4 As String
		Public Property Zusatz5 as String
		Public Property Zusatz6 As String
		Public Property Zusatz7 as String
		Public Property Zusatz8 As String
		Public Property Zusatz9 as String
		Public Property Zusatz10 As String
		Public Property Zusatz11 as String
		Public Property Zusatz12 As String

	End Class



End Namespace
