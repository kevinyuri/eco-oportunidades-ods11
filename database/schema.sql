/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 01/12/2025 22:32:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cursos]    Script Date: 01/12/2025 22:32:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cursos](
	[Id] [nvarchar](450) NOT NULL,
	[Nome] [nvarchar](150) NOT NULL,
	[Instituicao] [nvarchar](100) NOT NULL,
	[CargaHoraria] [nvarchar](50) NOT NULL,
	[Modalidade] [nvarchar](50) NOT NULL,
	[DataInicio] [datetime2](7) NOT NULL,
	[FocadoEmSustentabilidade] [bit] NOT NULL,
	[ImpactoComunitario] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Cursos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inscricoes]    Script Date: 01/12/2025 22:32:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inscricoes](
	[Id] [nvarchar](450) NOT NULL,
	[UsuarioId] [nvarchar](450) NOT NULL,
	[VagaId] [nvarchar](450) NULL,
	[CursoId] [nvarchar](450) NULL,
	[DataInscricao] [datetime2](7) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Inscricoes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 01/12/2025 22:32:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [nvarchar](450) NOT NULL,
	[Nome] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Perfil] [nvarchar](50) NOT NULL,
	[Telefone] [nvarchar](20) NOT NULL,
	[SenhaHash] [nvarchar](max) NOT NULL,
	[BairroResidencia] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vagas]    Script Date: 01/12/2025 22:32:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vagas](
	[Id] [nvarchar](450) NOT NULL,
	[Titulo] [nvarchar](150) NOT NULL,
	[Descricao] [nvarchar](max) NOT NULL,
	[Empresa] [nvarchar](100) NOT NULL,
	[Local] [nvarchar](100) NOT NULL,
	[TipoContrato] [nvarchar](50) NOT NULL,
	[DataPublicacao] [datetime2](7) NOT NULL,
	[AceitaRemoto] [bit] NOT NULL,
	[Bairro] [nvarchar](max) NOT NULL,
	[EhVagaVerde] [bit] NOT NULL,
	[ZonaDaCidade] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Vagas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cursos] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Cursos] ADD  DEFAULT (CONVERT([bit],(0))) FOR [FocadoEmSustentabilidade]
GO
ALTER TABLE [dbo].[Cursos] ADD  DEFAULT (N'') FOR [ImpactoComunitario]
GO
ALTER TABLE [dbo].[Inscricoes] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (N'') FOR [SenhaHash]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT (N'') FOR [BairroResidencia]
GO
ALTER TABLE [dbo].[Vagas] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Vagas] ADD  DEFAULT (CONVERT([bit],(0))) FOR [AceitaRemoto]
GO
ALTER TABLE [dbo].[Vagas] ADD  DEFAULT (N'') FOR [Bairro]
GO
ALTER TABLE [dbo].[Vagas] ADD  DEFAULT (CONVERT([bit],(0))) FOR [EhVagaVerde]
GO
ALTER TABLE [dbo].[Vagas] ADD  DEFAULT (N'') FOR [ZonaDaCidade]
GO
ALTER TABLE [dbo].[Inscricoes]  WITH CHECK ADD  CONSTRAINT [FK_Inscricoes_Cursos_CursoId] FOREIGN KEY([CursoId])
REFERENCES [dbo].[Cursos] ([Id])
GO
ALTER TABLE [dbo].[Inscricoes] CHECK CONSTRAINT [FK_Inscricoes_Cursos_CursoId]
GO
ALTER TABLE [dbo].[Inscricoes]  WITH CHECK ADD  CONSTRAINT [FK_Inscricoes_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
GO
ALTER TABLE [dbo].[Inscricoes] CHECK CONSTRAINT [FK_Inscricoes_Usuarios_UsuarioId]
GO
ALTER TABLE [dbo].[Inscricoes]  WITH CHECK ADD  CONSTRAINT [FK_Inscricoes_Vagas_VagaId] FOREIGN KEY([VagaId])
REFERENCES [dbo].[Vagas] ([Id])
GO
ALTER TABLE [dbo].[Inscricoes] CHECK CONSTRAINT [FK_Inscricoes_Vagas_VagaId]
GO
