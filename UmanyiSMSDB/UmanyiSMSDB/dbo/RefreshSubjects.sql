CREATE PROCEDURE [dbo].[RefreshSubjects]
AS
BEGIN
 SET NOCOUNT ON;
 DELETE FROM [Institution].[Subject];
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (1,'ENGLISH',101,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (2,'KISWAHILI',102,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (3,'MATHEMATICS',121,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (4,'BIOLOGY',231,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (5,'PHYSICS',232,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (6,'CHEMISTRY',233,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (7,'HISTORY',311,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (8,'GEOGRAPHY',312,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (9,'CRE',313,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (10,'AGRICULTURE',443,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (11,'COMPUTER',451,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (12,'FRENCH',501,100,0)
 INSERT INTO [Institution].[Subject] (SubjectID,NameOfSubject,Code,MaximumScore,IsOptional) VALUES (13,'BUSINESS',565,100,0)
 
END


