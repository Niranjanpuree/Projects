DECLARE @basePath NVARCHAR(200) 
DECLARE @bulkSQL NVARCHAR(MAX)
SET @basePath= (SELECT TOP 1 [BasePath] FROM [dbo].[BaseFilePath] WHERE Code = 'csv');
SET @bulkSQL = 'BULK INSERT QuestionaireManagerType
				FROM ''' + @basePath + 'QuestionaireManagerType.csv' + '''
				WITH
				(
				FIRSTROW = 2,
				FORMAT = ''CSV'',
				FIELDTERMINATOR = '','',  --CSV field delimiter
				ROWTERMINATOR = ''\n'',   --Use to shift the control to next row
				TABLOCK,
				KEEPNULLS
				)'
EXEC(@bulkSQL)

