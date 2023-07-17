USE MONITOREO_2
GO


DECLARE	
	@table_name varchar(50)

DECLARE cTabla CURSOR
FOR

SELECT name FROM sys.TABLES ORDER BY name

OPEN cTabla

FETCH NEXT FROM cTabla INTO @table_name

WHILE @@FETCH_STATUS = 0

BEGIN
		
	IF @table_name <> 'registro_pc' and @table_name <> 'ubicacion_campo' and @table_name <> 'ubicacion_lote'
	exec('TRUNCATE TABLE dbo.' + @table_name + '')
	IF @table_name <> 'registro_pc' and @table_name <> 'ubicacion_campo' and @table_name <> 'ubicacion_lote'
	exec('dbo.cop_' + @table_name + '_InsertOne')
	
	
		
	FETCH NEXT FROM cTabla INTO @table_name

END

CLOSE cTabla
DEALLOCATE cTabla