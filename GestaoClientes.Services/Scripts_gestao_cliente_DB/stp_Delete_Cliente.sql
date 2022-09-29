use GestaoClientes_DB
GO

IF OBJECT_ID('stp_Delete_Cliente') IS NOT NULL
   DROP PROCEDURE stp_Delete_Cliente
GO

CREATE PROCEDURE stp_Delete_Cliente 
 
 @id_cliente int = null,                          
 @cpf varchar(11) = null  
  
 as  
  
 declare @contSelect int = (SELECT count(*) from cliente   
  where cpf = @cpf OR id_cliente = @id_cliente)  
  
 if(@id_cliente is null and @cpf is null or (@contSelect = 0 or @contSelect > 1))  
 begin  
  select 0 as id_cliente, 'Id/CPF inválidos ou não correspondentes.' as msg,* from status_codes where id_status = 400  
 RETURN  
 END  
  
 if exists(SELECT 1 from cliente   
  where cpf = @cpf)  
 BEGIN  
  DELETE cliente where cpf = @cpf  
  SELECT 'Apagado com sucesso' as msg, * from status_codes where id_status = 200  
 RETURN  
 END  
 if exists(SELECT 1 from cliente   
  where id_cliente = @id_cliente)  
 BEGIN  
  DELETE cliente where id_cliente = @id_cliente  
  SELECT  'Apagado com sucesso' as msg, * from status_codes where id_status = 200  
 RETURN  
 END  
   
GO
GRANT EXECUTE ON stp_Delete_Cliente TO PUBLIC
GO