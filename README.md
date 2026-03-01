## Como Rodar

### Opção 1: Docker (recomendado)

```bash
docker-compose up --build
```

Acesse:

- Frontend: http://localhost:5173
- Backend API: http://localhost:5000/swagger

### Opção 2: Localmente

**Backend** (requer .NET 10 SDK):

```bash
cd backend
dotnet run --project src/GastosResidenciais.API
```

**Frontend** (requer Node.js 18+):

```bash
cd frontend
npm install
npm run dev
```

Acesse:

- Frontend: http://localhost:5173
- Backend API: http://localhost:5000/swagger
