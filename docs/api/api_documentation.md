# Documentação da API - Eco Oportunidades

## 1. Visão Geral

Esta API RESTful foi desenvolvida em .NET 8.0 para alimentar a plataforma Eco Oportunidades. Ela gerencia o fluxo de dados entre o frontend (Angular) e o banco de dados (Azure SQL), com foco em empregabilidade local e sustentável (ODS 11).

Base URL Local: ttps://trabalhoapacitacaounifor-fsgegjfkfbg8h8du.chilecentral-01.azurewebsites.net/api/

Autenticação: Bearer Token (JWT)

Formato de Dados: JSON

## 2. Autenticação e Usuários

### Registrar Novo Usuário

Cria uma conta para Candidato ou Empresa.

Endpoint: POST /api/Usuarios/registrar

Acesso: Público (Anônimo)

Corpo da Requisição (JSON):

{
  "nome": "Kevin Yuri",
  "email": "kevin@exemplo.com",
  "senha": "SenhaForte123!",
  "perfil": "candidato",
  "telefone": "(85) 99999-8888",
  "bairroResidencia": "Bom Jardim"  // [ODS 11] Obrigatório para match local
}


### Login (Autenticação)

Gera um token JWT para acesso aos recursos protegidos.

Endpoint: POST /api/Usuarios/login

Acesso: Público

Retorno (200 OK):

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiration": "2025-12-01T23:59:00Z",
  "usuario": {
    "id": "guid-do-usuario",
    "nome": "Kevin Yuri",
    "bairroResidencia": "Bom Jardim"
  }
}


## 3. Módulo de Vagas (Oportunidades)

### Listar Vagas

Retorna todas as vagas disponíveis.

Endpoint: GET /api/Vagas

Acesso: Público

### Criar Nova Vaga

Cadastra uma oportunidade de emprego.

Endpoint: POST /api/Vagas

Acesso: Restrito (Perfil: Empresa)

Corpo da Requisição (JSON):

{
  "titulo": "Coletor de Resíduos Eletrônicos",
  "descricao": "Vaga para atuar na triagem de componentes para reciclagem.",
  "empresa": "EcoTech Solutions",
  "local": "Fortaleza - CE",
  "tipoContrato": "CLT",
  "bairro": "Centro",
  "zonaDaCidade": "Zona Norte",
  "ehVagaVerde": true,
  "aceitaRemoto": false
}


### Detalhes da Vaga

Endpoint: GET /api/Vagas/{id}

### Atualizar Vaga

Endpoint: PUT /api/Vagas/{id}

### Excluir Vaga

Endpoint: DELETE /api/Vagas/{id}

## 4. Módulo de Cursos (Capacitação Verde)

### Listar Cursos

Retorna cursos de capacitação profissional.

Endpoint: GET /api/Cursos

Acesso: Público

### Criar Curso

Cadastra um novo curso ou oficina.

Endpoint: POST /api/Cursos

Acesso: Restrito (Perfil: Admin/Empresa)

Corpo da Requisição (JSON):

{
  "nome": "Instalação de Painéis Solares",
  "instituicao": "Instituto Sol",
  "cargaHoraria": "40h",
  "modalidade": "Presencial",
  "dataInicio": "2025-01-15T09:00:00Z",
  "focadoEmSustentabilidade": true,
  "impactoComunitario": "Capacita jovens para o mercado de energia limpa, gerando renda local."
}


## 5. Módulo de Inscrições

### Realizar Inscrição

Inscreve o usuário logado em uma Vaga ou Curso.

Endpoint: POST /api/Inscricoes

Acesso: Restrito (Requer Token JWT)

Corpo da Requisição (JSON):

{
  "usuarioId": "guid-do-usuario",
  "vagaId": "guid-da-vaga", // Opcional (se for curso)
  "cursoId": null,          // Opcional (se for vaga)
  "status": "Pendente"
}


## 6. Códigos de Status HTTP Utilizados

200 OK: Requisição realizada com sucesso.

201 Created: Recurso criado com sucesso (POST).

204 No Content: Sucesso, mas sem retorno (PUT/DELETE).

400 Bad Request: Erro de validação nos campos.

401 Unauthorized: Token JWT inválido ou ausente.

403 Forbidden: Usuário logado não tem permissão para esta ação.

404 Not Found: Recurso não encontrado.

500 Internal Server Error: Erro inesperado no servidor.

