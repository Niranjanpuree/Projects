--start of rename of column of contract entity 

EXEC sp_rename 'Contract.Office_ContractRepresentative' , 'OfficeContractRepresentative', 'COLUMN'

EXEC sp_rename 'Contract.Office_ContractTechnicalRepresent', 'OfficeContractTechnicalRepresent', 'COLUMN'

EXEC sp_rename 'Contract.SelfPerformance_Percent', 'SelfPerformancePercent', 'COLUMN'

EXEC sp_rename 'Contract.G_A_Percent', 'GAPercent', 'COLUMN'

EXEC sp_rename 'Contract.Fee_Percent', 'FeePercent', 'COLUMN'

EXEC sp_rename 'Contract.BlueSkyAward_Amount' , 'BlueSkyAwardAmount', 'COLUMN'

EXEC sp_rename 'Contract.AppWageDetermine_DavisBaconAct' , 'AppWageDetermineDavisBaconAct', 'COLUMN'

EXEC sp_rename 'Contract.RevenueRecognitionEAC_Percent', 'RevenueRecognitionEACPercent', 'COLUMN'

EXEC sp_rename 'Contract.AppWageDetermine_ServiceContractAct' , 'AppWageDetermineServiceContractAct', 'COLUMN'

EXEC sp_rename 'Contract.FundingOffice_ContractRepresentative', 'FundingOfficeContractRepresentative', 'COLUMN'

EXEC sp_rename 'Contract.FundingOffice_ContractTechnicalRepresent', 'FundingOfficeContractTechnicalRepresent', 'COLUMN'

EXEC sp_rename 'Contract.Parent_ContractGuid' , 'ParentContractGuid', 'COLUMN'

--end of rename of column of contract entity

--start of rename of column of contract questionaire entity

EXEC sp_rename 'ContractQuestionaire.Report_LastReportDate', 'ReportLastReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.Report_NextReportDate', 'ReportNextReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.GSA_LastReportDate', 'GSALastReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.GSA_NextReportDate', 'GSANextReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.SB_LastReportDate', 'SBLastReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.SB_NextReportDate', 'SBNextReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.GQAC_LastReportDate', 'GQACLastReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.GQAC_NextReportDate', 'GQACNextReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.CPARS_LastReportDate', 'CPARSLastReportDate', 'COLUMN'

EXEC sp_rename 'ContractQuestionaire.CPARS_NextReportDate', 'CPRSNextReportDate', 'COLUMN'


--end of rename of column of contract questionaire

--start of rename of column of customer contact entity
EXEC sp_rename 'CustomerContact.Alt_PhoneNumber', 'AltPhoneNumber', 'COLUMN'

EXEC sp_rename 'CustomerContact.Alt_EmailAddress' , 'AltEmailAddress', 'COLUMN'
--end of rename of column of customer contact entity

--start of rename of column of project

EXEC sp_rename 'Project.Office_ProjectRepresentative', 'OfficeProjectRepresentative', 'COLUMN'

EXEC sp_rename 'Project.Office_ProjectTechnicalRepresent', 'OfficeProjectTechnicalRepresent', 'COLUMN'

EXEC sp_rename 'Project.G_A_Percent', 'GAPercent', 'COLUMN'

EXEC sp_rename 'Project.Fee_Percent', 'FeePercent', 'COLUMN'

EXEC sp_rename 'Project.SelfPerformance_Percent', 'SelfPerformancePercent', 'COLUMN'

EXEC sp_rename 'Project.BlueSkyAward_Amount', 'BlueSkyAwardAmount', 'COLUMN'

EXEC sp_rename 'Project.AppWageDetermine_DavisBaconAct', 'AppWageDetermineDavisBaconAct', 'COLUMN'

EXEC sp_rename 'Project.RevenueRecognitionEAC_Percent', 'RevenueRecognitionEACPercent', 'COLUMN'

EXEC sp_rename 'Project.AppWageDetermine_ServiceProjectAct', 'AppWageDetermingServiceProjectAct', 'COLUMN'

EXEC sp_rename 'Project.FundingOffice_ProjectRepresentative', 'FundingOfficeProjectRepresentative', 'COLUMN'

EXEC sp_rename 'Project.FundingOffice_ProjectTechnicalRepresent', 'FundingOfficeProjectTechnicalRepresent', 'COLUMN'

--end of rename of column of project


--start of rename of column of customer office list entity
EXEC sp_rename 'UsCustomerOfficeList.DEPARTMENT_ID', 'DepartmentId', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.DEPARTMENT_NAME', 'DepartmentName', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.CUSTOMER_CODE', 'CustomerCode', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.CUSTOMER_NAME', 'CustomerName', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.CONTRACTING_OFFICE_CODE', 'ContractingOfficeCode', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.CONTRACTING_OFFICE_NAME', 'ContractingOfficeName', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.START_DATE', 'StartDate', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.END_DATE', 'EndDate', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.ADDRESS_CITY', 'AddressCity', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.ADDRESS_STATE', 'AddressState', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.ZIP_CODE', 'ZipCode', 'COLUMN'

EXEC sp_rename 'UsCustomerOfficeList.COUNTRY_CODE', 'CountryCode', 'COLUMN'

--end of rename of column of Us customer office list entity

EXEC sp_rename 'RevenuePerformanceObligation.RevenueOverTime_PointInTime', 'RevenueOverTimePointInTime', 'COLUMN'


