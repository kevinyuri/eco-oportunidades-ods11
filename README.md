# Eco Oportunidades - Plataforma de Empregabilidade & Educação Verde (ODS 11)

## 1. Objetivo do Projeto

O Eco Oportunidades é uma solução tecnológica desenvolvida para atender à ODS 11 (Cidades e Comunidades Sustentáveis), focada no fortalecimento da economia local e na transição para cidades mais verdes.

A plataforma conecta moradores a empregos de impacto positivo (Green Jobs) e oferece capacitação ecológica dentro do próprio território, reduzindo a pegada de carbono gerada pelo transporte e fomentando a geração de renda local.

## 2. Funcionalidades Implementadas

[x] Mapeamento de Vagas Verdes: Identificação e destaque de oportunidades com impacto ambiental positivo.

[x] Portal de Eco-Capacitação: Oferta de cursos focados em habilidades sustentáveis (Ex: Horta Urbana, Energia Solar).

[x] Autenticação Segura: Login para Candidatos e Empresas utilizando tokens JWT.

[x] Gestão de vagas e cursos por empresas e administradores.

## 3. Tecnologias Utilizadas

O projeto foi construído utilizando uma stack moderna e robusta:

- Frontend: Angular 19 (Standalone Components) + PrimeNG 19 (Interface Responsiva).

- Backend: ASP.NET Core Web API (.NET 8.0) + Entity Framework.

- Banco de Dados: SQL Server (Hospedado no Microsoft Azure).

- Testes: xUnit + EF Core InMemory (Testes Unitários Automatizados).

- Infraestrutura: GitHub Pages (Frontend) e Azure App Service (Backend).

## 4. Instruções de Instalação e Execução

- Pré-requisitos

  - Node.js (v18+)

  - .NET SDK 8.0

  - SQL Server (Local ou Docker)

## - Passo 1: Configurando o Backend (.NET)

  - Acesse a pasta do servidor:
  - cd backend/src
    
- Inicie a API:

  - dotnet run
  - A API estará acessível em: https://localhost:44363

## - Passo 2: Configurando o Frontend (Angular)

  - Acesse a pasta da interface web:
  - cd frontend/web
  - Instale as dependências do projeto: npm install
  - Inicie o servidor de desenvolvimento: ng serve
  - Acesse a aplicação no navegador: URL Local: http://localhost:4200

## 6. Links de Acesso e Demonstração

- Versão API Swagger para testes: [https://trabalhoapacitacaounifor-fsgegjfkfbg8h8du.chilecentral-01.azurewebsites.net/swagger/index.html]
- Versão em Produção (Frontend): [https://kevinyuri.github.io/eco-oportunidades-web]

Equipe de Desenvolvimento

Kevin Yuri - Desenvolvedor Unifor (Matrícula: [2314493])
