use GestaoClientes_DB
GO

IF OBJECT_ID('stp_Put_Cliente') IS NOT NULL
   DROP PROCEDURE stp_Put_Cliente
GO

CREATE PROCEDURE stp_Put_Cliente 

 @id_cliente int = null,
 @nome varchar(255) = null,                        
 @cpf varchar(11) = null,                        
 @sexo varchar(10) = NULL,
 @id_tipo_cliente int = null,
 @id_situacao_cliente int = null
 
AS

declare @contSelect int = (SELECT count(*) from cliente 
		where cpf = @cpf OR id_cliente = @id_cliente)

if(@id_cliente is null or not EXISTS(SELECT 1 from cliente where id_cliente = @id_cliente)
	or @nome is null or @cpf is null or (@contSelect = 0 or @contSelect > 1))
	begin
		select 0 as id_cliente, 'Id/CPF inválidos.' as msg, * from status_codes where id_status = 400
	RETURN
END

if(@id_tipo_cliente is not NULL AND not EXISTS(SELECT * from tipo_cliente where id_tipo_cliente = @id_tipo_cliente))
	begin
		select 0 as id_cliente, 'Id tipo cliente não existe.' as msg, * from status_codes where id_status = 400
	RETURN
END

if(@id_situacao_cliente is not NULL AND not EXISTS(SELECT * from situacao_cliente where id_situacao_cliente = @id_situacao_cliente))
	begin
		select 0 as id_cliente, 'Id situação cliente não existe.' as msg, * from status_codes where id_status = 400
	RETURN
END

 UPDATE cliente SET 
 nome = @nome,                        
 cpf = @cpf,                        
 sexo = @sexo,
 id_tipo_cliente = @id_tipo_cliente,
 id_situacao_cliente = @id_situacao_cliente
 
 where id_cliente = @id_cliente

 select (SELECT id_cliente from cliente where id_cliente = @id_cliente) as id_cliente, 'Alteração realizada com sucesso.' as msg, * from status_codes where id_status = 200

GO
GRANT EXECUTE ON stp_Put_Cliente TO PUBLIC
GO