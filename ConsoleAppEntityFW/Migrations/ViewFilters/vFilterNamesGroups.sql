IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vFilterNameGroups]'))
EXEC dbo.sp_executesql @statement = N'
CREATE VIEW [dbo].[vFilterNameGroups]
AS
SELECT  NEWID() AS Id, fn.Id AS FilterNameId, fn.[Name] AS FilterName, fv.Id AS FilterValueId, fv.[Name] AS FilterValue
FROM [dbo].[tblFilterNames] as fn 
left join [dbo].[tblFilterNameGruops] as fng ON fn.Id = fng.FilterNameId
left join [dbo].[tblFilterValues] as fv ON fv.Id = fng.FilterValueId'