CREATE PROCEDURE [dbo].[RefreshSubjects]
AS
BEGIN
 SET NOCOUNT ON;
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'ENGLISH')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'ENGLISH',101,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'KISWAHILI')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'KISWAHILI',102,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'MATHEMATICS')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'MATHEMATICS',121,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'BIOLOGY')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'BIOLOGY',231,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'PHYSICS')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'PHYSICS',232,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'CHEMISTRY')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'CHEMISTRY',233,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'HISTORY')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'HISTORY',311,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'GEOGRAPHY')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'GEOGRAPHY',312,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'CRE')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'CRE',313,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'AGRICULTURE')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'AGRICULTURE',443,100,0)
 IF NOT EXISTS (SELECT * FROM [Institution].[Subject] WHERE NameOfSubject = 'BUSINESS')
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES ([dbo].GetNewID('Institution.Subject'),'BUSINESS',565,100,0)
 
END



GO
GRANT EXECUTE
    ON OBJECT::[dbo].[RefreshSubjects] TO [Principal]
    AS [dbo];

