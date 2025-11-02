Feature: Authentication

	Como desenvolvedor
	Eu quero que os usuários possam fazer login na aplicação
	Para que possam acessar recursos protegidos

	@authentication @smoke
	Scenario: Login bem-sucedido com credenciais válidas
		Given que eu tenho um usuário cadastrado com:
			| Email                | Password  |
			| usuario@teste.com    | senha123  |
		When eu faço login com email "usuario@teste.com" e senha "senha123"
		Then a resposta deve ter status code 200
		And a resposta deve conter um token JWT
		And a resposta deve conter os dados do usuário autenticado
		And o token deve ser válido para acessar recursos protegidos

	@authentication @negative
	Scenario: Login falha com credenciais inválidas
		Given que eu tenho um usuário cadastrado com:
			| Email                | Password  |
			| usuario@teste.com    | senha123  |
		When eu faço login com email "usuario@teste.com" e senha "senhaErrada"
		Then a resposta deve ter status code 400
		And a resposta deve conter a mensagem "Usuário e/ou senha inválidos"
		And a resposta não deve conter um token

	@authentication @negative
	Scenario: Login falha com email inexistente
		When eu faço login com email "inexistente@teste.com" e senha "qualquersenha"
		Then a resposta deve ter status code 400
		And a resposta deve conter a mensagem "Usuário e/ou senha inválidos"
		And a resposta não deve conter um token

	@authentication @negative
	Scenario: Login falha com email vazio
		When eu faço login com email "" e senha "senha123"
		Then a resposta deve ter status code 400

	@authentication @negative
	Scenario: Login falha com senha vazia
		When eu faço login com email "usuario@teste.com" e senha ""
		Then a resposta deve ter status code 400