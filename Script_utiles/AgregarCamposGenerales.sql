USE Agro_Transactor
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
		
	IF  NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE (TABLE_NAME=@table_name) AND (COLUMN_NAME='operacion'))
	exec('ALTER TABLE dbo.' + @table_name + ' ADD operacion char(1) NULL')

	IF  NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE (TABLE_NAME=@table_name) AND (COLUMN_NAME='sincronizado'))
	exec('ALTER TABLE dbo.' + @table_name + ' ADD sincronizado bit NULL')
	
	IF  NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE (TABLE_NAME=@table_name) AND (COLUMN_NAME='id_cliente_maestro'))
	exec('ALTER TABLE dbo.' + @table_name + ' ADD id_cliente_maestro int NULL')
	
	--ALTER TABLE dbo.Campana ADD	sincronizado bit NULL
	--ALTER TABLE dbo.Campana ADD	id_cliente_maestro int NULL
	--exec('SELECT * FROM ' + @table_name + ' WHERE sincronizado = 0 ORDER BY numero_operacion')
		
		
	FETCH NEXT FROM cTabla INTO @table_name

END

CLOSE cTabla
DEALLOCATE cTabla


