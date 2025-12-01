Especificação de Requisitos - Eco Oportunidades (ODS 11)

1. Definição de Atores (Perfis de Usuário)

O sistema possui dois níveis de acesso distintos, garantindo a segurança e a integridade das operações:

🟢 1.1 Candidato
Permissões: Visualizar vagas/cursos r realizar inscrições

🛡️ 1.3 Empresa

Permissões: Responsável principal pelo cadastro de Vagas e Cursos de Capacitação.

2. Requisitos Funcionais (RF)

Módulo de Autenticação e Perfil

- [RF01] Cadastro de Usuário: O sistema deve permitir o cadastro informando Nome, Email, Senha, Telefone e Perfil (Candidato ou Empresa).

- [RF02] Login: Autenticação via E-mail e Senha (JWT).

Módulo de Vagas (Oportunidades)

- [RF03] Publicar Vaga: Permitir que usuários com perfil Empresa cadastrem novas vagas.

  - Campos ODS 11: Deve ser possível marcar a vaga como "Vaga Verde" (Sustentável) e definir "Zona da Cidade".

- [RF04] Listagem de Vagas: Exibir listagem pública de vagas com destaque visual para oportunidades sustentáveis e localização.

- Módulo de Capacitação (Cursos)

- [RF05] Gestão de Cursos: Permitir que o Administrador (e opcionalmente Empresas parceiras) cadastre cursos de capacitação.

  - Campo ODS 11: Deve incluir a descrição do "Impacto Comunitário" do curso.

- [RF06] Inscrição em Curso: Permitir que Candidatos garantam vaga nos cursos ofertados.

3. Requisitos Não-Funcionais (RNF)

- [RNF01] Usabilidade: O formulário de cadastro deve ser intuitivo, com feedbacks visuais claros (Toast notifications) e validação em tempo real.

- [RNF02] Performance: As listagens de vagas e cursos devem carregar em menos de 2 segundos.

- [RNF04] Interface: O sistema deve utilizar componentes visuais modernos (PrimeNG) e ser responsivo para acesso mobile.
