USE GestaoClientes_DB
GO

DECLARE @cpf int = 10;

while @cpf < 50
begin

insert into cliente VALUES(CONCAT('cliente - 00', @cpf), CONCAT('000000000', @cpf), 'Masculino',2,1)

set @cpf = @cpf + 1
end

while @cpf < 100
begin

insert into cliente VALUES(CONCAT('cliente - 00', @cpf), CONCAT('000000000', @cpf), 'Feminino', 4,2)

set @cpf = @cpf + 1
end

while @cpf < 500
begin

insert into cliente VALUES(CONCAT('cliente - 0', @cpf), CONCAT('00000000', @cpf), 'Masculino', 1,2)

set @cpf = @cpf + 1
end

while @cpf < 1000
begin

insert into cliente VALUES(CONCAT('cliente - 0', @cpf), CONCAT('00000000', @cpf), 'Feminino', 3,1)

set @cpf = @cpf + 1
end

while @cpf < 1500
begin

insert into cliente VALUES(CONCAT('cliente - ', @cpf), CONCAT('0000000', @cpf), 'Feminino', 1,1)

set @cpf = @cpf + 1
end

while @cpf < 2000
begin

insert into cliente VALUES(CONCAT('cliente - ', @cpf), CONCAT('0000000', @cpf), 'Masculino', 0,0)

set @cpf = @cpf + 1
end

select * from cliente

--delete cliente
