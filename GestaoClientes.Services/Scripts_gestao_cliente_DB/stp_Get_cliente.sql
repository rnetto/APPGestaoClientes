use GestaoClientes_DB
GO

IF OBJECT_ID('stp_Get_Cliente') IS NOT NULL
	DROP PROCEDURE stp_Get_Cliente
GO
            
CREATE PROCEDURE stp_Get_Cliente 

 @id_cliente INT = null,                        
 @nome varchar(255) = NULL,                        
 @cpf varchar(11) = NULL,                        
 @sexo varchar(10) = NULL,
 @id_tipo_cliente int = null,
 @tipo_cliente_descricao varchar(50) = null,
 @id_situacao_cliente int = null,
 @situacao_cliente_descricao varchar(50) = null,
 @pagnum int = 1,
 @itemsqtd int = 10

AS

 declare @totalPag int

if @pagnum < 0
 begin
SET @pagnum = @pagnum * (-1)
end

if @itemsqtd < 0
 begin
SET @itemsqtd = @itemsqtd * (-1)
end

SELECT
	@totalPag = CEILING(CAST(COUNT(*) AS NUMERIC(18, 2)) / (CAST(@itemsqtd AS NUMERIC(18, 2))))
FROM cliente c
LEFT JOIN tipo_cliente tc
	ON c.id_tipo_cliente = tc.id_tipo_cliente
LEFT JOIN situacao_cliente sc
	ON c.id_situacao_cliente = sc.id_situacao_cliente
WHERE (@id_cliente IS NULL
OR c.id_cliente = @id_cliente)
AND (@nome IS NULL
OR c.nome LIKE '%' + @nome + '%')
AND (@cpf IS NULL
OR c.cpf LIKE '%' + @cpf + '%')
AND (@sexo IS NULL
OR c.sexo LIKE @sexo)
AND (@id_tipo_cliente IS NULL
OR c.id_tipo_cliente = @id_tipo_cliente)
AND (@tipo_cliente_descricao IS NULL
OR tc.descricao_tipo_cliente LIKE '%' + @tipo_cliente_descricao + '%')
AND (@id_situacao_cliente IS NULL
OR c.id_situacao_cliente = @id_situacao_cliente
AND (@situacao_cliente_descricao IS NULL
OR sc.descricao_situacao_cliente LIKE '%' + @situacao_cliente_descricao + '%'))

if @totalPag < 1
BEGIN
	SELECT 0 as id_cliente,* from status_codes where id_status = 404
RETURN
END

if @pagnum > @totalPag
 begin
SET @pagnum = @totalPag
end

if @pagnum < 1
 begin
SET @pagnum = 1
end
SELECT
	c.id_cliente
   ,c.nome
   ,c.cpf
   ,c.sexo
   ,tc.id_tipo_cliente
   ,tc.descricao_tipo_cliente
   ,sc.id_situacao_cliente
   ,sc.descricao_situacao_cliente
FROM cliente c
LEFT JOIN tipo_cliente tc
	ON c.id_tipo_cliente = tc.id_tipo_cliente
LEFT JOIN situacao_cliente sc
	ON c.id_situacao_cliente = sc.id_situacao_cliente
WHERE (@id_cliente IS NULL
OR c.id_cliente = @id_cliente)
AND (@nome IS NULL
OR c.nome LIKE '%' + @nome + '%')
AND (@cpf IS NULL
OR c.cpf LIKE '%' + @cpf + '%')
AND (@sexo IS NULL
OR c.sexo LIKE @sexo)
AND (@id_tipo_cliente IS NULL
OR c.id_tipo_cliente = @id_tipo_cliente)
AND (@tipo_cliente_descricao IS NULL
OR tc.descricao_tipo_cliente LIKE '%' + @tipo_cliente_descricao + '%')
AND (@id_situacao_cliente IS NULL
OR c.id_situacao_cliente = @id_situacao_cliente
AND (@situacao_cliente_descricao IS NULL
OR sc.descricao_situacao_cliente LIKE '%' + @situacao_cliente_descricao + '%'))
order by c.nome, c.id_cliente

Offset (@pagnum - 1) * @itemsqtd rows FETCH next @itemsqtd rows only

GO
GRANT EXECUTE ON stp_Get_Cliente TO PUBLIC
GO